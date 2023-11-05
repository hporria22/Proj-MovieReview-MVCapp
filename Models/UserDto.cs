using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


[BsonIgnoreExtraElements]
public class UserDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get;set;}

  
    [BsonElement("role")]
    public string Role{get ; set; }

    [BsonElement("fullname")]
    public string? FullName{ get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("passwordHash")]
     public byte[] PasswordHash { get; set; }

     [BsonElement("passwordSalt")]
     public byte[] PasswordSalt { get; set; }
}