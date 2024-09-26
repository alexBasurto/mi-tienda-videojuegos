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
    public class DesarrolladorasController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;
        public DesarrolladorasController(MiTiendaVideojuegosContext context)
        {
            this.context = context;
        }


        // Primer endpoint para obtener todos las desarrolladoras

        [HttpGet(("ObtenerDesarrolladoras"))]
        public async Task<ActionResult> GetCategorias()
        {
            var Categorias = await context.Desarrolladoras.ToArrayAsync();
            return Ok(Categorias);

        }

        /// 

        //[HttpGet("DesarrolladorasConJuegosUsandoDTO")]
        //public async Task<ActionResult> MisDesarroladoras()
        //{

        //    var desarrolladorasjuegos = await (from x in context.Desarrolladoras
        //                                       select new DTOAltaDesarrolladora
        //                                       {
        //                                           IdDesarrolladora = x.IdDesarrolladora,
        //                                           Nombre = x.Nombre,
        //                                           Indie = x.Indie,
        //                                           Pais = x.Pais,

        //                                           // DTOJuegoItem para mostrar id y nombre 
        //                                           Juegos = x.Juegos.Select(y => new DTOJuegoItem
        //                                           {
        //                                               IdJuego = y.IdJuego,
        //                                               Nombre = y.Nombre,
        //                                           }).ToList(),

        //                                       }).ToListAsync();

        //    return Ok(desarrolladorasjuegos);
        //}

        // UPDATE 

        // Aqui no uso un DTO
        [HttpPut("HacerMdoificacionNombre")]
        public async Task<ActionResult> PutNombre([FromBody] Desarrolladoras desarrolladora)
        {
            var desarrolladoraUpdate = await context.Desarrolladoras.AsTracking().
                FirstOrDefaultAsync(x => x.IdDesarrolladora
                                        == desarrolladora.IdDesarrolladora);

            if (desarrolladoraUpdate == null)
            {
                return BadRequest("NO SE HA ENCONTRADO ID DESARROLLADORA " + desarrolladora.IdDesarrolladora);
            }
            desarrolladoraUpdate.Nombre = desarrolladora.Nombre;
            await context.SaveChangesAsync();

            //return NoContent();
            return Ok("El nuevo nombre de id " + desarrolladora.IdDesarrolladora + " ahora es " + desarrolladora.Nombre);
        }

        // HACIENDO A PUT USANDO DTOMdodificacionNombreUsandoDTO

        [HttpPut("HacerMdoificacionNombreUsandoDTO")]
        public async Task<ActionResult> PutNombreDTO([FromBody] DTOModificacionDesarrolladora desarrolladora)
        {
            var desarrolladoraUpdate = await context.Desarrolladoras.AsTracking().
                FirstOrDefaultAsync(x => x.IdDesarrolladora
                                        == desarrolladora.IdDesarrolladora);

            if (desarrolladoraUpdate == null)
            {
                return BadRequest("NO SE HA ENCONTRADO ID DESARROLLADORA " + desarrolladora.IdDesarrolladora);
            }

            // Se ha encontrado ese id 
            // modifico el nombre o la propiedad Indie

            desarrolladoraUpdate.Nombre = desarrolladora.Nombre;
            desarrolladoraUpdate.Indie = desarrolladora.Indie;
            desarrolladoraUpdate.Pais=desarrolladora.Pais;
                        await context.SaveChangesAsync();

            //return NoContent();
            return Ok("El nuevo nombre de id " + desarrolladora.IdDesarrolladora + " ahora es " + desarrolladora.Nombre);
        }

        // CREATE Crear una nueva Desarrolladora

        [HttpPost("nuevaDesarroladoraDTO")]

        public async Task<ActionResult> PostEditorialDTO(DTOAltaDesarrolladora desarrolladora)
        {

            var existeDesarrolladora = await context.Desarrolladoras.AsTracking().
               FirstOrDefaultAsync(x => x.Nombre
                                       == desarrolladora.Nombre);

            if (existeDesarrolladora != null)
            {
                return BadRequest("ERROR " + desarrolladora.Nombre + " YA EXISTE");
            }

            var newDesarrolladora = new Desarrolladoras()
            {
                Nombre = desarrolladora.Nombre,
                Indie = desarrolladora.Indie,
                Pais = desarrolladora.Pais
            };

            await context.AddAsync(newDesarrolladora);
            await context.SaveChangesAsync();

            //return Created();

            return Ok("Se ha creado la nueva desarrolladora " + desarrolladora.Nombre);
        }

        // DELETE 
        // Borrado pasando un id 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteDesarrolladora(int id)
        {

            var desarrolladora = await context.Desarrolladoras.FindAsync(id);

            if (desarrolladora is null)
            {
                return NotFound("No existe esa Desarrolladora");
            }

            var hayjuegos = await context.Juegos.AnyAsync(x => x.IdDesarrolladora == id);

            if (hayjuegos)
            {
                return BadRequest("No se puede borrar tiene juegos relacionados");
            }

            // Hechas las comprobaciones la puedo borrar 

            context.Remove(desarrolladora);
            await context.SaveChangesAsync();
            return Ok("Has borrado la desarrolladora " + id);
        }
    }
}