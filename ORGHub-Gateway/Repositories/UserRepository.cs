using MongoDB.Driver;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<User>("Users");
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> FindByUserNameAsync(string username)
        {
            return await _usersCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
        }

        public async Task AddAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task DeleteAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
