using MongoDB.Bson.Serialization.Attributes;

namespace ORGHub_Gateway.Models
{
    public class AuthenticationRequest
    {
        [BsonElement("username")]
        public required string Username { get; set; }

        [BsonElement("password")]
        public required string Password { get; set; }
    }
}
