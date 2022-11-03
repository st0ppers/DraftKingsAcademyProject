namespace Keyboard.Models.Requests
{
    public class AddOrderRequest
    {
        public int KeyboardID { get; set; }
        public int ClientID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
