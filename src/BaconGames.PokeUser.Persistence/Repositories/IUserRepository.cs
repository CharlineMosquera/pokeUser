using BaconGames.PokeUser.Domain.Entities;

namespace BaconGames.PokeUser.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}