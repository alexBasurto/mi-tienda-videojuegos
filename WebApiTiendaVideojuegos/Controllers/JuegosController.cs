using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApiTiendaVideojuegos.DTOs;
using WebApiTiendaVideojuegos.Interface;
using WebApiTiendaVideojuegos.Models;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;
        private readonly IGestorArchivos gestorArchivos;


        public JuegosController (MiTiendaVideojuegosContext context, IGestorArchivos gestorArchivos)
        {
            this.context = context;
            this.gestorArchivos = gestorArchivos;
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
                Caratula = x.Caratula,
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
                var nuevoJuego = new Juegos()
                {
                    Nombre = juego.Nombre,
                    Precio = juego.Precio,
                    Disponible = juego.Disponible,
                    Lanzamiento = juego.Lanzamiento,
                    Pegi = juego.Pegi,
                    Caratula = null,
                    IdCategoria = juego.IdCategoria,
                    IdDesarrolladora = juego.IdDesarrolladora,
                    IdPlataforma = juego.IdPlataforma
                };


                if (juego.Caratula != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await juego.Caratula.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(juego.Caratula.FileName);
                        nuevoJuego.Caratula = await gestorArchivos.GuardarArchivo(contenido, extension, "imagenes",
                            juego.Caratula.ContentType);
                    }
                }

                await context.AddAsync(nuevoJuego);
                await context.SaveChangesAsync();
                //return Created();

                // Obtener el URI del nuevo recurso,
                // se devuelve en la cabecera de la respuesta
                string uri = $"/api/juegos/{nuevoJuego.IdJuego}";

                return Created(uri, new { juego = nuevoJuego });
            }
        }

        // Editar un juego
        [HttpPut]
        public async Task<ActionResult> PutJuego([FromForm] DTOJuegoModificar juego) 
        {
            var juegoModificar = await context.Juegos.AsTracking().FirstOrDefaultAsync(x => x.IdJuego == juego.IdJuego);
            if (juegoModificar == null)
            {
                return NotFound($"No existe un ID de juego {juego.IdJuego}");
            }

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

                if (juego.Caratula != null)
                {
                    await gestorArchivos.BorrarArchivo(juegoModificar.Caratula, "imagenes");
                    using (var memoryStream = new MemoryStream())
                    {
                        // Extraemos la imagen de la petición
                        await juego.Caratula.CopyToAsync(memoryStream);
                        // La convertimos a un array de bytes que es lo que necesita el método de guardar
                        var contenido = memoryStream.ToArray();
                        // La extensión la necesitamos para guardar el archivo
                        var extension = Path.GetExtension(juego.Caratula.FileName);
                        // Recibimos el nombre del archivo
                        // El servicio Transient GestorArchivos instancia el servicio y cuando se deja de usar se destruye
                        juegoModificar.Caratula = await gestorArchivos.GuardarArchivo(contenido, extension, "imagenes",
                            juego.Caratula.ContentType);
                    }
                }
                else
                {
                    if(juego.EliminarCaratula)
                    {
                        await gestorArchivos.BorrarArchivo(juegoModificar.Caratula, "imagenes");
                        juegoModificar.Caratula = null;
                    }
                }

                context.Update(juegoModificar);
                await context.SaveChangesAsync();
                return NoContent();
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

            await gestorArchivos.BorrarArchivo(juegoEliminar.Caratula, "imagenes");

            context.Remove(juegoEliminar);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
