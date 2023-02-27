using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpectruMineAPI.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string _username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public bool verified { get; set; } = false;
}
