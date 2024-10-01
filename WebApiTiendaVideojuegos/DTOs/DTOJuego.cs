using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApiTiendaVideojuegos.DTOs
{
    public class DTOJuego
    {
        public int IdJuego { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal? Precio { get; set; }

        public bool Disponible { get; set; }

        public DateTime Lanzamiento { get; set; }

        public int Pegi { get; set; }

        public string? Caratula { get; set; }

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }

        public int IdPlataforma { get; set; }
        public string NombrePlataforma { get; set; }

        public int IdDesarrolladora { get; set; }
        public string NombreDesarrolladora { get; set; }
    }
}
