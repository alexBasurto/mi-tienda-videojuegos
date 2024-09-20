using System;
using System.Collections.Generic;

namespace WebApiTiendaVideojuegos.Models;

public partial class Plataformas
{
    public int IdPlataforma { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Juegos> Juegos { get; set; } = new List<Juegos>();
}
