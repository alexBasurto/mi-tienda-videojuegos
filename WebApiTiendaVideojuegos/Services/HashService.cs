using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApiTiendaVideojuegos.Classes;

namespace WebApiTiendaVideojuegos.Services
{
    public class HashService
    {
        // Hash: Clave que no se puede revertir. Perfecto para contraseñas seguras.
        // Se guardará en la tabla de usuarios.
        // Salt: valro aleatorio que se anexa al texto plano al que queremos aplicar la función que genera el Hash.
        // Valor aleatorio = valor único
        // El Salt se debe guardar junto al password para contrastar el login

        // Método para generar el Salt:
        public ResultadoHash Hash(string textoPLano)
        {
            // Salt aleatorio:
            var salt = new byte[16]; // Tamaño justo
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt); // Generar un array aleatorio de bytes
            }

            // Llamamos al método ResultadoHash y retornamos el Hash con el Saltxº
            return Hash(textoPLano, salt);
        }

        public ResultadoHash Hash(string textoPlano, byte[] salt)
        {
            var claveDerivada = KeyDerivation.Pbkdf2(password: textoPlano,
                salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(claveDerivada);

            return new ResultadoHash()
            {
                Hash = hash,
                Salt = salt
            };
        }
    }
}
