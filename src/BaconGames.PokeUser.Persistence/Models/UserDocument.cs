using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BaconGames.PokeUser.Persistence.Models
{
    public class UserDocument
    {
        [BsonId] // Para marcar que esta propiedad es el Id de la colección MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Para indicar que el tipo de dato es un ObjectId
        public string Id { get; set; }

        [BsonElement("name")] // Asigna el nombre del campo en la colección MongoDB
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("pokemon")]
        public List<string> Pokemon { get; set; } // Relación con los Pokémon capturados por el usuario.
    }
}
