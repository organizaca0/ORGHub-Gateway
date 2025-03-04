using Microsoft.AspNetCore.Identity;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Security
{
    public class PasswordEncoder
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordEncoder()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public bool Verify(string password, string passwordHash)
        {
            var dummyUser = new User();

            var result = _passwordHasher.VerifyHashedPassword(dummyUser, passwordHash, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
