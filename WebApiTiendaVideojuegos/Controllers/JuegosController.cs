using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.DTOs;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;

        public JuegosController (MiTiendaVideojuegosContext context)
        {
            this.context = context;
        }

        // CRUD inicial
        // Obtener todos los juegos
        [HttpGet]
        public async Task<ActionResult> GetJuegos ()
        {
            var juegos = await context.Juegos.Select(x => new DTOJuegos
            {
                IdJuego = x.IdJuego,
                Nombre = x.Nombre,
                Precio = x.Precio,
                Disponible = x.Disponible,
                Lanzamiento = x.Lanzamiento,
                Pegi = x.Pegi,
                IdCategoria = x.IdCategoria,
                IdDesarrolladora = x.IdDesarrolladora,
                IdPlataforma = x.IdPlataforma
            }).ToListAsync();

            return Ok(juegos);
        }

        // Crear un juego
        [HttpPost]
        public async Task<ActionResult> PostJuego ()
        {
            var nuevoJuego = await context.Juegos.Select( x => new )
        }

    }
}
