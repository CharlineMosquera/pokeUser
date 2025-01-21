namespace BaconGames.PokeUser.Domain.Entities
{
    public class Pokemon
    {
        public string Id { get; set; } // MongoDB ObjectId como string.
        public string Name { get; set; }
        public object Data { get; set; }  // Almacenar cualquier tipo de datos relacionados con el Pokémon

    }
}
