﻿namespace WebApiTiendaVideojuegos.DTOs
{
    public class DTODesarrolladoraConJUegos
    {
        public int IdDesarrolladora { get; set; }

        public string Nombre { get; set; }

        public bool Indie { get; set; }

        public string? Pais { get; set; }

        public List<DTOJuegoItem> Juegos { get; set; }

    }
}
