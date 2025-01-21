using BaconGames.PokeUser.Domain.Entities;
using BaconGames.PokeUser.Persistence.Repositories;
using Microsoft.AspNetCore.Http;

namespace BaconGames.PokeUser.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        // Constructor que inyecta el repositorio de usuarios.
        public UserService(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        // Método para realizar login de usuario y retornar un JWT.
        public async Task<string> LoginAsync(string email, string password, HttpResponse response)
        {
            // Buscar el usuario por email
            var user = await _userRepository.GetUserByEmailAsync(email);
            
            // Verificar si el usuario existe y si la contraseña coincide
            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null; // Usuario no encontrado o la contraseña no coincide.
            }

            // Si el usuario existe y la contraseña es correcta, generar un token JWT.
            var token = _authService.GenerateJwtToken(user.Id);

            _authService.SetTokenCookie(response, token);

            return token;
        }

        // Método para registrar un nuevo usuario.
        public async Task<bool> RegisterAsync(User user)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return false; // El usuario ya existe
            }

            // Generar un hash con un costo de 12 para la contraseña del usuario
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 12);

            // Guardar el usuario en la base de datos a través del repositorio.
            await _userRepository.CreateUserAsync(user);
            return true; // Usuario creado exitosamente
        }

        // Método privado para verificar la contraseña.
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }

        // Metodo para cerrar sesión e invalidar el token.
        public void Logout(HttpRequest request, HttpResponse response)
        {
            _authService.InvalidateToken(request, response);
        }
    }

    // Define los métodos que el servicio de usuario deberá implementar
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user); // Para registrar un nuevo usuario.
        Task<string> LoginAsync(string email, string password, HttpResponse response); // Para realizar login de usuario y retornar un JWT.
        void Logout(HttpRequest request, HttpResponse response); // Para cerrar sesión e invalidar el token.
    }
}
