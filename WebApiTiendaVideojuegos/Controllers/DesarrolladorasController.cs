using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesarrolladorasController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;
        private string íd;

        public DesarrolladorasController(MiTiendaVideojuegosContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetDesarroladoras()
        {
            var Desarrolladoras = await context.Desarrolladoras.ToListAsync();
            return Ok(Desarrolladoras);
             //     TENGO QUE VER ESTE COMENTARIO
             // He añadido otra línea 

        }

        [HttpGet("obtenerDesarrolladoras")]
        public async Task<ActionResult> MisDesarroladoras()
        {
            var Desarrolladoras = await context.Desarrolladoras.ToListAsync();
            return Ok(Desarrolladoras);
         
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteDesarrolladora(int id)
        {
            var hayjuegos = await context.Juegos.AnyAsync(x => x.IdDesarrolladora == id);
           
            
            if (hayjuegos)
            {
                return BadRequest("N se puede borrar "+" tiene juegos relacionados");
            }
            var desarrolladora = await context.Desarrolladoras.FindAsync(id);

            if (desarrolladora is null)
            {
                return NotFound("No existe esa Desarrolladora");
            }

            context.Remove(desarrolladora);
            await context.SaveChangesAsync();
            return Ok("Has borrado la desarrolladora " + id);
        }




    }
}

