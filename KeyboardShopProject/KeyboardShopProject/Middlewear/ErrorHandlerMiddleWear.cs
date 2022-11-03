using System.Net;

namespace Keyboard.ShopProject.Middlewear
{
    public class ErrorHandlerMiddleWear
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleWear> _logger;

        public ErrorHandlerMiddleWear(RequestDelegate next, ILogger<ErrorHandlerMiddleWear> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (error)
                {
                    case CustomException e:
                        response.StatusCode = (int)HttpStatusCode.BadGateway;
                        _logger.LogError($"Error {context.Request.Host.Port.Value} with message {e.Message}");
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogError($"Error {context.Request.Host.Port.Value}  with message {e.Message}");
                        break;
                    default: 
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError($"Error {context.Request.Host.Port.Value} with message {error.Message}");
                        break;
                }
            }
        }
    }
}
