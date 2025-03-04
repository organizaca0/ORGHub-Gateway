using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Repositories;
using ORGHub_Gateway.Security;

namespace ORGHub_Gateway.Services
{
    public class AuthService
    {
        private readonly PasswordEncoder _passwordEncoder;
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(PasswordEncoder passwordEncoder, UserRepository userRepository, JwtService jwtService)
        {
            _passwordEncoder = passwordEncoder;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse response = new AuthenticationResponse();

            User user = await _userRepository.FindByUserNameAsync(authenticationRequest.Username);
            if (user == null)
            {
                response.Error = "Not found";
                return response;
            }

            if (user.Status == UserStatus.Inactive || user.Status == UserStatus.Suspended)
            {
                response.Error = "Usuário desativado.";
                return null;
            }

            if (!_passwordEncoder.Verify(authenticationRequest.Password, user.PasswordHash))
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user);
            return new AuthenticationResponse(user, token);
        }
    }
}
