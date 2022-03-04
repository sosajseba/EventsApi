using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventApi.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;
        [BsonElement("title")]
        public string Title { get; set; } = null!;
        [BsonElement("description")]
        public string Description { get; set; } = null!;
        [BsonElement("type")]
        public string Type { get; set; } = null!;
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("location")]
        public string Location { get; set; } = null!;
        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
