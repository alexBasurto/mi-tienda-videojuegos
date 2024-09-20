using System;
using System.Collections.Generic;

namespace WebApiTiendaVideojuegos.Models;

public partial class Categorias
{
    public int IdCategoria { get; set; }

    public string Genero { get; set; } = null!;

    public string? Subgenero { get; set; }

    public virtual ICollection<Juegos> Juegos { get; set; } = new List<Juegos>();
}
