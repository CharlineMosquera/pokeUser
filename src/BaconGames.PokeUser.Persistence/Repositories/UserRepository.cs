using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Configurations;
using BaconGames.PokeUser.Persistence.Mappers;
using BaconGames.PokeUser.Persistence.Models;
using MongoDB.Driver;

namespace BaconGames.PokeUser.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserDocument> _users;

        public UserRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<UserDocument>(settings.UsersCollectionName);
        }

        public async Task CreateUserAsync(User user)
        {
            var userDocument = UserMapper.ToDocument(user);
            await _users.InsertOneAsync(userDocument);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var userDocument = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (userDocument == null)
            {
                return null;
            }

            return UserMapper.ToDomain(userDocument);
        }
    }
}
