using System.ComponentModel.DataAnnotations;

namespace BaconGames.PokeUser.Api.Models
{
    public class AddPokemonRequest
    {
        [Required(ErrorMessage = "El campo pokemonName es obligatorio.")]
        public string PokemonName { get; set; }
    }
}
