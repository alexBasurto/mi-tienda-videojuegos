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

        [HttpGet]
        public async Task<ActionResult> MisDesarroladoras()
        {
            var Desarrolladoras = await context.Desarrolladoras.ToListAsync();
            return Ok(Desarrolladoras);
            //     TENGO QUE VER ESTE COMENTARIO
            // He añadido otra línea 

        }
    }
}

