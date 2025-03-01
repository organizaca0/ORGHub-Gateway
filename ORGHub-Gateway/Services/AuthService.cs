using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Services
{
    public class AuthService
    {
        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse response =  new AuthenticationResponse();

            User user = _userRepository.FindByUserName(authenticationRequest.Username);
            if (user == null)
            {
                response.Error = "Not found";
                return response;
            }

            if (user.Status == UserStatus.Inactive || user.Status == UserStatus.Suspended)
            {
                return null;
            }

            if (!_passwordEncoder.Verify(authenticationRequest.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate a JWT token and return the response
            var token = _jwtService.GenerateToken(user);
            return new AuthenticationResponse(user, token);
        }
    }
}
