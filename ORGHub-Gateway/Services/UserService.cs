using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Interfaces;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Repositories;
using ORGHub_Gateway.Security;

namespace ORGHub_Gateway.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordEncoder _passwordEncoder;

        private TimeSpan LockDuration = TimeSpan.FromMinutes(10);


        public UserService(UserRepository userRepository, PasswordEncoder passwordEncoder)
        {
            _userRepository = userRepository;
            _passwordEncoder = passwordEncoder;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var existingUser = await _userRepository.FindByUserNameAsync(username);

            if (existingUser != null)
            {
                return existingUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreateUser(User user)
        {
            var existingUser = await _userRepository.FindByUserNameAsync(user.UserName);

            if (existingUser != null)
            {
                return false;
            }
            else
            {
                user.PasswordHash = _passwordEncoder.Encode(user.PasswordHash); 
                user.Status = UserStatus.Active;

                await _userRepository.AddAsync(user);
                return true;
            }
        }


        public async Task RefreshLastLogin(string username)
        {
            var user = await _userRepository.FindByUserNameAsync(username);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task UpdateAttempts(string username)
        {
            var user = await _userRepository.FindByUserNameAsync(username);
            if (user != null)
            {
                if (user.Attempts + 1 == 3)
                {
                    ++user.Attempts;
                    user.LastBlock = DateTime.UtcNow;
                }
                else
                {
                    ++user.Attempts;
                }
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task ResetAttempts(string username)
        {
            var user = await _userRepository.FindByUserNameAsync(username);
            if (user != null)
            {
                user.Attempts = 0;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<bool> IsLocked(string username)
        {
            var user = await _userRepository.FindByUserNameAsync(username);
            if (user == null) return true; 

            if (user.Attempts < 3) return false; 

            var timeSinceLastBlock = DateTime.UtcNow - user.LastBlock;

            if (timeSinceLastBlock > LockDuration)
            {
                await ResetAttempts(username); 
                return false;
            }

            return true;
        }
    }
}
