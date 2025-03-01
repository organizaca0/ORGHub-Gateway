using ORGHub_Gateway.Enums;

namespace ORGHub_Gateway.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public int Attempts { get; set; } = 0;
        public List<string> Roles { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastBlock { get; set; }
        public string PasswordHash { get; set; }
    }
}
