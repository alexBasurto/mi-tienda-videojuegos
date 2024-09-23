using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.DTOs
{
    public class DTOCategoria
    {
        public int IdCategoria { get; set; }

        public string Genero { get; set; }

        public string? Subgenero { get; set; }

    }

    public class DTOCategoriaAgregar
    {
        public string Genero { get; set; }

        public string? Subgenero { get; set; }
    }
}
