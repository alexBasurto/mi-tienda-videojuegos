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
    public class PlataformaController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;

        public PlataformaController(MiTiendaVideojuegosContext context)
        {
            this.context = context;

        }


        [HttpGet]
        public async Task<ActionResult> GetPlataformas()
        {
            var plataformas = await context.Plataformas.Select( x=> new  DTOPlataformas
            {
                IdPlataforma = x.IdPlataforma,
                Nombre = x.Nombre
            }).ToListAsync();
            return Ok(plataformas);
        }

        [HttpPost]
        public async Task<ActionResult> PostPlataformas(DTOPlataformas plataforma)
        {
            var nuevaPlataforma = new Plataformas
            {
                Nombre = plataforma.Nombre
            };
            await context.Plataformas.AddAsync(nuevaPlataforma);
            await context.SaveChangesAsync();   
            return Ok(nuevaPlataforma);
        }

        [HttpPut]
        public async Task<ActionResult> PutPlataformas(DTOPlataformas plataforma)
        {
            var cambioPlataforma = await context.Plataformas.FindAsync(plataforma.IdPlataforma);
            if (cambioPlataforma == null)
            {
                return NotFound();
            }
            cambioPlataforma.Nombre = plataforma.Nombre;
            await context.SaveChangesAsync();
            return Ok(cambioPlataforma);

        }

        [HttpDelete("{idPlataforma}")]
        public async Task<ActionResult> DeletePlataformas(int idPlataforma)
        {
            var eliminarPlataforma = await context.Plataformas.FindAsync(idPlataforma);
            if (eliminarPlataforma == null)
            {
                return NotFound();
            }
            context.Plataformas.Remove(eliminarPlataforma);
            await context.SaveChangesAsync();
            return Ok(eliminarPlataforma);

        }

    }
}
