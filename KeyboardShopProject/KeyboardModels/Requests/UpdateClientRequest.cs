namespace Keyboard.Models.Requests
{
    public class UpdateClientRequest 
    {
        public int ClientID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string OrderID { get; set; }
    }
}
