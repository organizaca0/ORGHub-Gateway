using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(User user);
        Task RefreshLastLogin(string username);
        Task UpdateAttempts(string username);
        Task ResetAttempts(string username);
        Task<bool> IsLocked(string username);
    }
}
