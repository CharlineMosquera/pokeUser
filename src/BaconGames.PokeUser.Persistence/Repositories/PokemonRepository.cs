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

        public PokemonRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pokemon = database.GetCollection<PokemonDocument>("Pokemon");
        }

        public async Task AddPokemonToUserAsync(string userId, PokemonDocument pokemon)
        {
            pokemon.Id = ObjectId.GenerateNewId().ToString();
            await _pokemon.InsertOneAsync(pokemon);
        }

        public async Task<List<PokemonDocument>> GetUserPokemonsAsync(string userId)
        {
            var filter = Builders<PokemonDocument>.Filter.Eq("UserId", userId);
            return await _pokemon.Find(filter).ToListAsync();
        }
    }
}
