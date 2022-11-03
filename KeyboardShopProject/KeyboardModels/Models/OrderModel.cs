namespace Keyboard.Models.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public int KeyboardID { get; set; }
        public int ClientID { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
