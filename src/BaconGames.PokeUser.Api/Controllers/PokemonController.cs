using BaconGames.PokeUser.Api.Models;
using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.External.Services;
using BaconGames.PokeUser.Persistence.Mappers;
using BaconGames.PokeUser.Persistence.Models;
using BaconGames.PokeUser.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaconGames.PokeUser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly PokeApiService _pokeApiService;

        public PokemonController(IPokemonRepository pokemonRepository, PokeApiService pokeApiService)
        {
            _pokemonRepository = pokemonRepository;
            _pokeApiService = pokeApiService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddPokemon([FromBody] AddPokemonRequest request)
        {
            var userId = HttpContext.Items["User"]?.ToString();
            if (userId == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }

            // Consulta la PokeAPI para obtener los datos del Pokémon
            var pokemonData = await _pokeApiService.GetPokemonDataAsync(request.PokemonName);
            if (pokemonData == null)
            {
                return NotFound(new { message = $"No se pudo encontrar el Pokémon {request.PokemonName}" });
            }

            // Crea el documento de Pokémon
            var pokemonDocument = new PokemonDocument
            {
                Name = request.PokemonName,
                Data = pokemonData
            };
            await _pokemonRepository.AddPokemonToUserAsync(userId, pokemonDocument);
            return Ok(new { message = $"Pokémon agregado exitosamente: {pokemonDocument.Name}" });
        }

        [Authorize]
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var userId = HttpContext.Items["User"]?.ToString();
            if (userId == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }
            var pokemonDocument = await _pokemonRepository.GetUserPokemonsAsync(userId);
            var pokemon = pokemonDocument.ConvertAll(PokemonMapper.ToDomain);
            return Ok(pokemon);
        }
    }
}
