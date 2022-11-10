using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Keyboard.Models.Models
{
    public class ShoppingCartModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<KeyboardModel> Keyboards { get; set; } 
        public int ClientId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
