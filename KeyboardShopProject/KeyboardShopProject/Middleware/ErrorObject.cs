using System.Text.Json;

namespace Keyboard.ShopProject.Middleware
{
    public class ErrorObject
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
