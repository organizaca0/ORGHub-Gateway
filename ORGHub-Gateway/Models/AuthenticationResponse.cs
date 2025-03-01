using MongoDB.Driver;

namespace ORGHub_Gateway.Models
{
    public class AuthenticationResponse
    {
        private string Jwt;
        private User User;
        public string Error { get; set; }

        public AuthenticationResponse() { }

        public AuthenticationResponse(User user, string jwt ) 
        {
            this.User = user;
            this.Jwt = jwt;
        }
        public string AuthenticationError() => this.Error;
    }
}
