using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Models;
using MongoDB.Bson;

namespace BaconGames.PokeUser.Persistence.Mappers
{
    public class PokemonMapper
    {
        public static PokemonDocument ToDocument(Pokemon pokemon)
        {
            return new PokemonDocument
            {
                Id = string.IsNullOrEmpty(pokemon.Id) ? ObjectId.Empty : ObjectId.Parse(pokemon.Id),
                Name = pokemon.Name,
                Data = pokemon.Data
            };
        }

        public static Pokemon ToDomain(PokemonDocument pokemonDocument)
        {
            return new Pokemon
            {
                Id = pokemonDocument.Id.ToString(),
                Name = pokemonDocument.Name,
                Data = pokemonDocument.Data
            };
        }
    }
}
