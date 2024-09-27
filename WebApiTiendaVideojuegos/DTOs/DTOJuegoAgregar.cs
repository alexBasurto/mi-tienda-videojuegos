﻿namespace WebApiTiendaVideojuegos.DTOs
{
    public class DTOJuegoAgregar
    {
        public string Nombre { get; set; }

        public decimal? Precio { get; set; }

        public bool Disponible { get; set; }

        public DateTime Lanzamiento { get; set; }

        public int Pegi { get; set; }

        public int IdCategoria { get; set; }

        public int IdPlataforma { get; set; }

        public int IdDesarrolladora { get; set; }
    }
}
