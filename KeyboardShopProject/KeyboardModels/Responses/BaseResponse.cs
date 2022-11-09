using System.Net;
using MessagePack;

namespace Keyboard.Models.Responses
{
    [MessagePackObject()]
    public class BaseResponse
    {
        [Key(4)]
        public HttpStatusCode StatusCode { get; set; }
        [Key(5)]
        public string? Message { get; set; }
    }
}
