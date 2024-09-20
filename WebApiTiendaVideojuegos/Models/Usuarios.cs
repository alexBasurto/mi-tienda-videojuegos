using System;
using System.Collections.Generic;

namespace WebApiTiendaVideojuegos.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Salt { get; set; }
}
