using System.Net;

namespace Keyboard.Models.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
