using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiUser.Models;

public class Family
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? FamilyId { get; set; }
    public string Name { get; set; } = null!;
    public string Parentesco { get; set; } = null!;
}
