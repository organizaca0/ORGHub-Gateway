using MongoDB.Driver;
using ORGHub_Gateway.Interfaces;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private TimeSpan LockDuration = TimeSpan.FromMinutes(10);

        public UserService(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("users");
        }

        public async Task RefreshLastLogin(string username)
        {
            var user = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task UpdateAttempts(string username)
        {
            var user = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
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
                await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
        }

        public async Task ResetAttempts(string username)
        {
            var user = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Attempts = 0;
                await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
        }

        public async Task<bool> IsLocked(string username)
        {
            var user = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
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
