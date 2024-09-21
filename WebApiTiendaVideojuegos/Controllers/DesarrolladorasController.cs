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
            var hayDesarrolladoras = await context.Desarrolladoras.ToListAsync();
            return Ok(hayDesarrolladoras);
            // primer punto de acceso get todo
        }
    }
}

