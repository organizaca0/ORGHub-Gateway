using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ORGHub_Gateway.Enums;

namespace ORGHub_Gateway.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("secondName")]
        public string SecondName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("status")]
        public UserStatus Status { get; set; } 

        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [BsonElement("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("lastLogin")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastLogin { get; set; }

        [BsonElement("lastBlock")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastBlock { get; set; }

        [BsonElement("attempts")]
        public int Attempts { get; set; } = 0;
    }
}
