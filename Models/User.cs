using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiUser.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Direction { get; set; } = null!;
    public string Age { get; set; } = null!;
}
