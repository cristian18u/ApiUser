using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiUser.Models;

public class UserLogin
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}