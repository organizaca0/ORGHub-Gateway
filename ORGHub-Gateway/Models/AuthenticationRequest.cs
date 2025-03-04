using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ORGHub_Gateway.Models
{
    public class AuthenticationRequest
    {
        [BsonElement("username")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
