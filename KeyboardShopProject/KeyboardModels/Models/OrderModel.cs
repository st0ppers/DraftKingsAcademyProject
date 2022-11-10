using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Keyboard.Models.Models
{
    public class OrderModel 
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid OrderID { get; set; } = Guid.NewGuid();
        public List<KeyboardModel> Keyboards { get; set; }
        public int ClientId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
