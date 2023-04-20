using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpectruMineAPI.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string UUID { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string _username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? NewPassword { get; set; }
    public string Email { get; set; } = null!;
    public bool Verified { get; set; } = false;
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public List<MailCode> MailCodes { get; set; } = new List<MailCode>();
    public List<ObjectId> Inventory { get; set; } = new List<ObjectId>();
}