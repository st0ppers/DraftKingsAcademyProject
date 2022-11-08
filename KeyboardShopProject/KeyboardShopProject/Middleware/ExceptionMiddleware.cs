using System.Net;
using Microsoft.AspNetCore.Http;

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

        public async Task Handler(HttpContext context, Exception error)
        {
            switch (error)
            {
                case CustomException e:
                    await context.Response.WriteAsync(new ErrorObject()
                    {
                        Message = "Bad request",
                        StatusCode = context.Response.StatusCode = (int)HttpStatusCode.BadRequest
                    }.ToString());
                    break;
                case KeyNotFoundException e:
                    await context.Response.WriteAsync(new ErrorObject()
                    {
                        Message = "Not found",
                        StatusCode = context.Response.StatusCode = (int)HttpStatusCode.NotFound
                    }.ToString());
                    break;
                default:
                    await context.Response.WriteAsync(new ErrorObject()
                    {
                        Message = "Internal Error",
                        StatusCode = context.Response.StatusCode = (int)HttpStatusCode.InternalServerError
                    }.ToString());
                    break;
            }
        }
    }
}
