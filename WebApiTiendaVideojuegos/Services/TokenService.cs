using WebApiTiendaVideojuegos.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiTiendaVideojuegos.Services
{
    public class TokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DTOLoginResponse GenerarToken(DTOUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, credencialesUsuario.Email),
                new Claim("lo que yo quiera", "cualquier otro valor")
            };

            var clave = Environment.GetEnvironmentVariable("CLAVE_JWT");
            var claveKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var signingCredentials = new SigningCredentials(claveKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new DTOLoginResponse()
            {
                Token = tokenString,
                Email = credencialesUsuario.Email
            };
        }
    }
}
