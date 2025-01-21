using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Models;

namespace BaconGames.PokeUser.Persistence.Repositories
{
    public interface IPokemonRepository
    {
        Task AddPokemonToUserAsync(string userId, PokemonDocument pokemon);
        Task<List<PokemonDocument>> GetUserPokemonsAsync(string userId);
    }
}