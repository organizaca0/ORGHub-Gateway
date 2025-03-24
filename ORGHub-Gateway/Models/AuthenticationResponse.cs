using Newtonsoft.Json;
using ORGHub_Gateway.Enums;

namespace ORGHub_Gateway.Models
{
    public class AuthenticationResponse
    {
        [JsonProperty("jwt")]
        public string Jwt { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("secondName")]
        public string SecondName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("roles")]
        public Dictionary<string, List<string>> Roles { get; set; }

        [JsonProperty("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public AuthenticationResponse() { }

        public AuthenticationResponse(User user, string jwt)
        {
            Jwt = jwt;
            UserName = user.UserName;
            Name = user.Name;
            SecondName = user.SecondName;
            Email = user.Email;
            Roles = user.Roles;
            ProfilePictureUrl = user.ProfilePictureUrl;
        }

        public string AuthenticationError() => Error;
    }
}
