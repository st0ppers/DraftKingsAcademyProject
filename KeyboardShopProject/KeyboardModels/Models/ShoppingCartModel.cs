using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Keyboard.Models.Models
{
    public class ShoppingCartModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public IEnumerable<KeyboardModel> Keyboards { get; set; }
        public int ClientId { get; set; }
    }
}
