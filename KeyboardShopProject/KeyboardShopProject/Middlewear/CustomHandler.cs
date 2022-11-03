namespace Keyboard.ShopProject.Middlewear
{
    public class CustomHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomHandler> _logger;

        public CustomHandler(RequestDelegate next, ILogger<CustomHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogError($"Request is from {context.Request.Host.Port.Value} port from {context.Request.Method}");
            await _next(context);
        }
    }
}
