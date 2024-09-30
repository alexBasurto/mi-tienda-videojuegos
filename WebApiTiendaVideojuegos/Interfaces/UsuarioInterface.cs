using WebApiTiendaVideojuegos.DTOs;

namespace WebApiTiendaVideojuegos.Interfaces
{
    public interface IUsuarioValidator
    {
        bool IsValid(DTOUsuario usuario, out List<string> errors);
    }
}
