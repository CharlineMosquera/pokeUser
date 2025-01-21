using System.ComponentModel.DataAnnotations;

namespace BaconGames.PokeUser.Domain.Entities
{
    public class User
    {
        // MongoDB ObjectId como string para que la implementacion
        // de la base de datos no influya
        public string Id { get; set; } 

        public string Name { get; set; }

        [EmailAddress] // Atributo de validación de email
        public string Email { get; set; }

        [MinLength(8)] // Atributo de validación de longitud mínima
        public string Password { get; set; }

        public List<string> Pokemon { get; set; } = new List<string>(); // Almacena una lista de IDs de Pokémon.

    }
}
