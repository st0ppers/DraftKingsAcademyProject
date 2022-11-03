namespace Keyboard.Models.Requests
{
    public class UpdateOrderRequest
    {
        public int OrderID { get; set; }
        public int KeyboardID { get; set; }
        public int ClientID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
