using System;
using System.Collections.Generic;

namespace WebApiTiendaVideojuegos.Models;

public partial class Juegos
{
    public int IdJuego { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? Precio { get; set; }

    public bool Disponible { get; set; }

    public DateTime Lanzamiento { get; set; }

    public int Pegi { get; set; }

    public string? Caratula { get; set; }

    public int IdCategoria { get; set; }

    public int IdPlataforma { get; set; }

    public int IdDesarrolladora { get; set; }

    public virtual Categorias IdCategoriaNavigation { get; set; } = null!;

    public virtual Desarrolladoras IdDesarrolladoraNavigation { get; set; } = null!;

    public virtual Plataformas IdPlataformaNavigation { get; set; } = null!;
}
