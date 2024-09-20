using System;
using System.Collections.Generic;

namespace WebApiTiendaVideojuegos.Models;

public partial class Desarrolladoras
{
    public int IdDesarrolladora { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Indie { get; set; }

    public string? Pais { get; set; }

    public virtual ICollection<Juegos> Juegos { get; set; } = new List<Juegos>();
}
