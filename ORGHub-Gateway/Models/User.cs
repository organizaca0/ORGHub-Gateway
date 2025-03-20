using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ORGHub_Gateway.Enums;
using System.ComponentModel.DataAnnotations;

namespace ORGHub_Gateway.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userName")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string UserName { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [BsonElement("secondName")]
        [StringLength(100, ErrorMessage = "Second name cannot exceed 100 characters.")]
        public string SecondName { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [BsonElement("status")]
        [Required(ErrorMessage = "Status is required.")]
        public UserStatus Status { get; set; }

        [BsonElement("roles")]
        [Required(ErrorMessage = "Roles are required.")]
        public Dictionary<string, List<Role>> Roles { get; set; }

        [BsonElement("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        [BsonElement("passwordHash")]
        [Required(ErrorMessage = "Password hash is required.")]
        public string PasswordHash { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("lastLogin")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastLogin { get; set; }

        [BsonElement("lastBlock")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastBlock { get; set; } = DateTime.MinValue;

        [BsonElement("attempts")]
        public int Attempts { get; set; } = 0;

        public bool HasAccessToProject(string project)
        {
            if (GetRolesForProject(project).Count == 0)
                return false;
            return true;
        }
        public List<Role> GetRolesForProject(string project)
        {
            if (HasAccessToProject(project))
                return this.Roles.GetValueOrDefault(project);
            return null;
        }

    }
}
