using System.Text.Json;
using MongoDB.Bson;

namespace BaconGames.PokeUser.External.Services
{
    public class PokeApiService
    {
        // Instancia de la clase HttpClient que permite hacer solicitudes HTTP
        // Con los verbos GET POST, etc
        private readonly HttpClient _httpClient;

        // Constructor de la clase PokeApiService
        public PokeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método asíncrono que obtiene los datos de un Pokémon de la PokeAPI
        // Recibe el nombre del Pokémon como parámetro
        // Retorna un Objeto BsonDocument con la informacion del pokemon o null si no se pudo obtener los datos
        public async Task<BsonDocument?> GetPokemonDataAsync(string pokemonName)
        {
            try
            {
                // Convertir el nombre del Pokémon a minúsculas
                var lowerCasePokemonName = pokemonName.ToLower();

                // Construir la URL de la PokeAPI con interpolacion de cadenas
                var url = $"https://pokeapi.co/api/v2/pokemon/{lowerCasePokemonName}";

                // Hacer la solicitud HTTP GET
                var response = await _httpClient.GetAsync(url);

                // Verificar si la solicitud fue exitosa
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: No se pudo obtener los datos del Pokémon {pokemonName}. Código de estado: {response.StatusCode}");
                    return null;
                }

                // Leer y deserializar la respuesta JSON
                var content = await response.Content.ReadAsStringAsync();
                var pokemonData = BsonDocument.Parse(content);

                return pokemonData;
            }
            catch (Exception ex) // Si ocurre un error captura la excepción
            {
                Console.WriteLine($"Error al obtener datos del Pokémon {pokemonName}: {ex.Message}");
                return null;
            }
        }
    }
}
