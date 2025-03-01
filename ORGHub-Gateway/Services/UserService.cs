using ORGHub_Gateway.Interfaces;

namespace ORGHub_Gateway.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly TimeService _timeService;

        public UserService(IMongoDatabase database, TimeService timeService)
        {
            _userCollection = database.GetCollection<User>("users");
            _timeService = timeService;
        }

        public async Task RefreshLastLogin(string username)
        {
            var user = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
            if (user != null)
            {
                user.LastLogin = _timeService.GetCurrentDatetime();
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
                    user.Attempts += 1;
                    user.LastBlock = _timeService.GetCurrentDatetime();
                }
                else
                {
                    user.Attempts += 1;
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
            if (user != null)
            {
                if (user.Attempts == 3)
                {
                    var fifteenMinutesInMillis = 10 * 60 * 1000;
                    var timeSinceLastBlock = DateTime.UtcNow - user.LastBlock;
                    if (timeSinceLastBlock.TotalMilliseconds > fifteenMinutesInMillis)
                    {
                        await ResetAttempts(username);
                        return false;
                    }
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
