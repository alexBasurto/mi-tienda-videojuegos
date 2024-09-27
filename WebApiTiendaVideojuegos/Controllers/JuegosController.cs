using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.DTOs;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Authorize]
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
            var juegos = await context.Juegos.Select(x => new DTOJuego
            {
                IdJuego = x.IdJuego,
                Nombre = x.Nombre,
                Precio = x.Precio,
                Disponible = x.Disponible,
                Lanzamiento = x.Lanzamiento,
                Pegi = x.Pegi,
                IdCategoria = x.IdCategoria,
                NombreCategoria = x.IdCategoriaNavigation.Genero,
                IdDesarrolladora = x.IdDesarrolladora,
                NombreDesarrolladora = x.IdDesarrolladoraNavigation.Nombre,
                IdPlataforma = x.IdPlataforma,
                NombrePlataforma = x.IdPlataformaNavigation.Nombre
            }).ToListAsync();

            return Ok(juegos);
        }

        // Crear un juego
        [HttpPost]
        public async Task<ActionResult> PostJuego ([FromForm] DTOJuegoAgregar juego)
        {
            var nuevoJuego = new Juegos()
            {
                Nombre = juego.Nombre,
                Precio = juego.Precio,
                Disponible = juego.Disponible,
                Lanzamiento = juego.Lanzamiento,
                Pegi = juego.Pegi,
                IdCategoria = juego.IdCategoria,
                IdDesarrolladora = juego.IdDesarrolladora,
                IdPlataforma = juego.IdPlataforma
            };

            var categoriaValida = await context.Categorias.FindAsync(juego.IdCategoria);
            var desarrolladoraValida = await context.Desarrolladoras.FindAsync(juego.IdDesarrolladora);
            var plataformaValida = await context.Plataformas.FindAsync(juego.IdPlataforma);

            if (categoriaValida == null)
            {
                return NotFound($"No existe una categoría con el ID introducido {nuevoJuego.IdCategoria}");
            }
            else if (desarrolladoraValida == null)
            {
                return NotFound($"No existe una desarrolladora con el ID introducido {nuevoJuego.IdDesarrolladora}");
            }
            else if (plataformaValida == null)
            {
                return NotFound($"No existe una plataforma con el ID introducido {nuevoJuego.IdPlataforma}");
            }
            else
            {
                await context.AddAsync(nuevoJuego);
                await context.SaveChangesAsync();
                return Created();
            }
        }

        // Editar un juego
        [HttpPut]
        public async Task<ActionResult> PutJuego([FromBody] DTOJuego juego) 
        {
            var juegoModificar = await context.Juegos.AsTracking().FirstOrDefaultAsync(x => x.IdJuego == juego.IdJuego);
            if (juegoModificar == null)
            {
                return NotFound($"No existe un ID de juego {juego.IdJuego}");
            }
            else
            {
                var categoriaValida = await context.Categorias.FindAsync(juego.IdCategoria);
                var desarrolladoraValida = await context.Desarrolladoras.FindAsync(juego.IdDesarrolladora);
                var plataformaValida = await context.Plataformas.FindAsync(juego.IdPlataforma);
                
                if (categoriaValida == null)
                {
                    return NotFound($"No existe una categoría con el ID introducido {juego.IdCategoria}");
                }
                else if (desarrolladoraValida == null)
                {
                    return NotFound($"No existe una desarrolladora con el ID introducido {juego.IdDesarrolladora}");
                }
                else if (plataformaValida == null)
                {
                    return NotFound($"No existe una plataforma con el ID introducido {juego.IdPlataforma}");
                }
                else
                {
                    juegoModificar.Nombre = juego.Nombre;
                    juegoModificar.Precio = juego.Precio;
                    juegoModificar.Disponible = juego.Disponible;
                    juegoModificar.Lanzamiento = juego.Lanzamiento;
                    juegoModificar.Pegi = juego.Pegi;
                    juegoModificar.IdCategoria = juego.IdCategoria;
                    juegoModificar.IdPlataforma = juego.IdPlataforma;
                    juegoModificar.IdDesarrolladora = juego.IdDesarrolladora;

                    context.Update(juegoModificar);
                    await context.SaveChangesAsync();
                    return NoContent();
                }
            }
        }

        // Eliminar juego
        [HttpDelete("{idJuego}")]
        public async Task<ActionResult> DeleteJuego([FromRoute] int idJuego)
        {
            var juegoEliminar = await context.Juegos.FirstOrDefaultAsync(x=>x.IdJuego ==  idJuego);
            if (juegoEliminar == null)
            {
                return NotFound($"No existe un juego con el ID introducido {idJuego}");
            }
            else
            {
                context.Remove(juegoEliminar);
                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
