namespace Keyboard.Models.Requests
{
    public class UpdateKeyboardRequest
    {
        public int KeyboardID { get; set; }
        public string Size { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
    }
}
