using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BaconGames.PokeUser.Application
{
    // Clase para la autenticación del usuario, generar tokens JWT y establecer cookies.
    public class AuthService : IAuthService
    {
        // Guarda la clave secreta para firmar los tokens JWT.
        private readonly string _jwtSecretKey;

        //  Emisor del token JWT.
        private readonly string _issuer;

        // Público al que está destinado el token.
        private readonly string _audience;

        // Constructor recibe la interfaz IConfiguration para obtener la configuración.
        public AuthService(IConfiguration configuration)
        {
            // Carga la clave secreta de la configuración.
            _jwtSecretKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        // Metodo para generar el token jwt.
        public string GenerateJwtToken(string userId)
        {
            // Crea un array de claims(declaraciones) que contienen información sobre el usuario.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId), // id del usuario
            };

            // Crear la clave de seguridad para la firma
            // Convierte la clave secreta en un array de bytes.
            // Usa la clave de bytes para crear una clave simétrica.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));

            // Crea las credenciales de firma con la clave y el algoritmo HmacSha256.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token con las propiedades definidas
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Expiración de 1 hora
                signingCredentials: creds);

            // Convertir el token en un string y lo retorna.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void InvalidateToken(HttpRequest request, HttpResponse response)
        {
            // Verifica si la cookie "tokenjwt" existe en la solicitud
            if (request.Cookies.ContainsKey("tokenjwt"))
            {
                // Elimina la cookie de la respuesta
                response.Cookies.Delete("jwt"); ;
            }
            return;
        }

        // Metodo para establecer la cookie con el token jwt.
        public void SetTokenCookie(HttpResponse response, string token)
        {
            // Configurar cookie con HttpOnly y Secure (solo en producción)
            response.Cookies.Append("tokenjwt", token, new CookieOptions
            {
                HttpOnly = true, // La cookie solo puede ser accedida por el servidor.
                Secure = false, // La cookie solo se enviará a través de HTTPS.
                SameSite = SameSiteMode.Strict, // Evitar CSRF, la cookie solo se envia en solicitudes del mismo origen.
                Expires = DateTime.Now.AddHours(1) // Duración de la cookie
            });
        }
    }

    public interface IAuthService
    {
        string GenerateJwtToken(string userId);
        void SetTokenCookie(HttpResponse response, string token);
        void InvalidateToken(HttpRequest request, HttpResponse response);
    }
}
