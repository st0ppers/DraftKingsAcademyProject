using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace Keyboard.ShopProject.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error from {e.Source} with message {e.Message}");
                await Handler(context, e);
            }
        }

        public async Task Handler(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ErrorObject()
            {
                Message = "Internal Error",
                StatusCode = context.Response.StatusCode
            }.ToString()!);
        }

    }
}
