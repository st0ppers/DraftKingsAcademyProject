using FluentValidation;
using FluentValidation.AspNetCore;
using KafkaServices.KafkaSettings;
using Keyboard.BL.CommandHandler;
using Keyboard.ShopProject.CustomHealthChecks;
using Keyboard.ShopProject.ExtensionMethods;
using Keyboard.ShopProject.HealthChecks;
using Keyboard.ShopProject.Middleware;
using MediatR;
using Serilog;


var logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);

builder.Services.RegisterRepositories().RegisterServices().AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

//optionsMonitor
builder.Services.Configure<KafkaSettingsForKeyboard>(builder.Configuration.GetSection(nameof(KafkaSettingsForKeyboard)));
//HealthCheck
builder.Services.AddHealthChecks().AddCheck<SqlHealthCheck>("SQL Server");

builder.Services.AddMediatR(typeof(CreaterClientCommandHandler).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.RegisterHealthCheck();
app.MapHealthChecks("/healthz");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
