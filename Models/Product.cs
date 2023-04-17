using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SpectruMineAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string ObjUrl { get; set; } = string.Empty;
    }
}
