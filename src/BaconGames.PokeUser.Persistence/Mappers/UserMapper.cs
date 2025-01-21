using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Models;

namespace BaconGames.PokeUser.Persistence.Mappers
{
    public class UserMapper
    {
        public static User ToDomain(UserDocument userDocument)
        {
            return new User
            {
                Id = userDocument.Id,
                Name = userDocument.Name,
                Email = userDocument.Email,
                Password = userDocument.Password,
                Pokemon = userDocument.Pokemon
            };
        }

        public static UserDocument ToDocument(User user)
        {
            return new UserDocument
            {
                Id = string.IsNullOrEmpty(user.Id) ? null : user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Pokemon = user.Pokemon
            };
        }
    }
}
