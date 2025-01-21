using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Configurations;
using BaconGames.PokeUser.Persistence.Mappers;
using BaconGames.PokeUser.Persistence.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BaconGames.PokeUser.Persistence.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly IMongoCollection<PokemonDocument> _pokemon;
        private readonly IMongoCollection<UserDocument> _users;

        public PokemonRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pokemon = database.GetCollection<PokemonDocument>("Pokemon");
            _users = database.GetCollection<UserDocument>(settings.UsersCollectionName);
        }

        public async Task AddPokemonToUserAsync(string userId, PokemonDocument pokemon)
        {
            await _pokemon.InsertOneAsync(pokemon);

            var filter = Builders<UserDocument>.Filter.Eq(user => user.Id, new ObjectId(userId));
            var update = Builders<UserDocument>.Update.Push(user => user.Pokemon, pokemon.Id);
            await _users.UpdateOneAsync(filter, update);
        }

        public async Task<List<PokemonDocument>> GetUserPokemonsAsync(string userId)
        {
            //var filter = Builders<PokemonDocument>.Filter.Eq("UserId", userId);
            //return await _pokemon.Find(filter).ToListAsync();

            var userFilter = Builders<UserDocument>.Filter.Eq(u => u.Id, new ObjectId(userId));
            var user = await _users.Find(userFilter).FirstOrDefaultAsync();

            if (user == null || user.Pokemon == null || user.Pokemon.Count == 0)
            {
                return new List<PokemonDocument>();
            }

            var pokemonFilter = Builders<PokemonDocument>.Filter.In(p => p.Id, user.Pokemon);
            return await _pokemon.Find(pokemonFilter).ToListAsync();
        }
    }
}
