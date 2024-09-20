using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
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
        public async Task<ActionResult> GetCategorias()
        {
            var Categorias = await context.Categorias.ToArrayAsync();
            return Ok(Categorias);
        }
    }
}
