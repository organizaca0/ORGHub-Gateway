using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest authenticationRequest);
    }
}
