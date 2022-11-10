using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Keyboard.BL.CommandHandler;
using Keyboard.ShopProject.CustomHealthChecks;
using Keyboard.ShopProject.ExtensionMethods;
using Keyboard.ShopProject.HealthChecks;
using Keyboard.ShopProject.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;


var logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);

builder.Services.RegisterRepositories().RegisterServices().RegisterIHostedServices().RegisterIOptionsMonitor(builder).AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

//HealthCheck
builder.Services.AddHealthChecks().AddCheck<SqlHealthCheck>("SQL Server");
builder.Services.AddHealthChecks().AddCheck<MongoHealthCheck>("Mongo connection");

builder.Services.AddMediatR(typeof(CreateClientCommandHandler).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var token = new OpenApiSecurityScheme()
    {
        Description = "Standard Authorization header",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(token.Reference.Id, token);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {token,Array.Empty<string>()}
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
var app = builder.Build();

app.RegisterHealthCheck();
app.MapHealthChecks("/healthz");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
