using System.Text.RegularExpressions;
using WebApiTiendaVideojuegos.DTOs;
using WebApiTiendaVideojuegos.Interfaces;

namespace WebApiTiendaVideojuegos.Validators
{
    public class UsuarioValidator : IUsuarioValidator
    {
        public bool IsValid(DTOUsuario usuario, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrEmpty(usuario.Email) ||
                !Regex.IsMatch(usuario.Email, @"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
            {
                errors.Add("Invalid Email Address");
            }

            if (string.IsNullOrEmpty(usuario.Password) || usuario.Password.Length < 6)
            {
                errors.Add("Password must be at least 6 characters long");
            }

            return errors.Count == 0;
        }
    }
}



