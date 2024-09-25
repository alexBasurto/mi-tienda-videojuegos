using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.DTOs;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;

        public CategoriasController(MiTiendaVideojuegosContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DTOCategoria>>> GetCategorias()
        {
            try
            {
                List<DTOCategoria> Categorias = await context.Categorias.Select(
                    c => new DTOCategoria
                    {
                        IdCategoria = c.IdCategoria,
                        Genero = c.Genero,
                        Subgenero = c.Subgenero
                    }
                    ).ToListAsync();
                return Ok(Categorias);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Algo falló.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostCategoria([FromBody] DTOCategoriaAgregar categoria)
        {
            bool generoSubgeneroExisten = await context.Categorias.AnyAsync(c => c.Genero == categoria.Genero && c.Subgenero == categoria.Subgenero);
            if (generoSubgeneroExisten)
            {
                return BadRequest($"Ya existe una categoría {categoria.Genero} - {categoria.Subgenero}");
            }

            Categorias newCategoria = new Categorias
            {
                Genero = categoria.Genero,
                Subgenero = categoria.Subgenero
            };

            await context.Categorias.AddAsync(newCategoria);
            await context.SaveChangesAsync();
            return Ok(newCategoria);
        }

        [HttpPut]
        public async Task<ActionResult> PutCategoria([FromBody] DTOCategoria categoria)
        {
            Categorias? updateCategoria = await context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == categoria.IdCategoria);
            if (updateCategoria == null)
            {
                return NotFound("Categoría no encontrada");
            }

            bool generoSubgeneroExisten = await context.Categorias.AnyAsync(c => c.Genero == categoria.Genero && c.Subgenero == categoria.Subgenero);
            if (generoSubgeneroExisten)
            {
                return BadRequest($"Ya existe una categoría {categoria.Genero} - {categoria.Subgenero}");
            }

            updateCategoria.Genero = categoria.Genero;
            updateCategoria.Subgenero = categoria.Subgenero;
            context.Update(updateCategoria);
            await context.SaveChangesAsync();
            return Ok(updateCategoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategoria([FromRoute] int id)
        {
            Categorias? deleteCategoria = await context.Categorias.FindAsync(id);
            if (deleteCategoria == null)
            {
                return NotFound();
            }

            bool tieneJuegos = await context.Juegos.AnyAsync(j => j.IdCategoria == id);
            if (tieneJuegos)
            {
                return BadRequest("La categoría indicada tiene juegos asociados. No se puede borrar");
            }

            context.Categorias.Remove(deleteCategoria);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
