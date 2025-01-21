using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Models;
using MongoDB.Bson;

namespace BaconGames.PokeUser.Persistence.Mappers
{
    public class UserMapper
    {
        public static User ToDomain(UserDocument userDocument)
        {
            return new User
            {
                Id = userDocument.Id.ToString(),
                Name = userDocument.Name,
                Email = userDocument.Email,
                Password = userDocument.Password,
                Pokemon = userDocument.Pokemon.Select(p => p.ToString()).ToList()
            };
        }

        public static UserDocument ToDocument(User user)
        {
            return new UserDocument
            {
                Id = string.IsNullOrEmpty(user.Id) ? ObjectId.GenerateNewId() : new ObjectId(user.Id),
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Pokemon = user.Pokemon.Select(p => ObjectId.Parse(p)).ToList()
            };
        }
    }
}
