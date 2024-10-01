using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTiendaVideojuegos.Models;
using Microsoft.AspNetCore.DataProtection;
using WebApiTiendaVideojuegos.Services;
using WebApiTiendaVideojuegos.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApiTiendaVideojuegos.Interfaces;
using WebApiTiendaVideojuegos.Validators;

namespace WebApiTiendaVideojuegos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly MiTiendaVideojuegosContext context;
        private readonly HashService hashService;
        private readonly TokenService tokenService;
        private readonly IUsuarioValidator usuarioValidator;


        // private readonly IConfiguration configuration;
        // private readonly IDataProtector dataProtector;
        // private readonly ILogger<UsuariosController> logger;

        public UsuariosController(MiTiendaVideojuegosContext context, HashService hashService,
            TokenService tokenService, IUsuarioValidator usuarioValidator)
        {
            this.context = context;
            this.hashService = hashService;
            this.tokenService = tokenService;
            this.usuarioValidator = usuarioValidator;
        }

        // Iniciamos sesión con un Usuario existente:
        [Authorize]
        [HttpPost("hash/checkusuario")]
        public async Task<ActionResult> CheckUsuarioHash([FromBody] DTOUsuario usuario)
        {
            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x =>  x.Email == usuario.Email);
            if (usuarioDB == null)
            {
                return Unauthorized();
            }

            var resultadoHash = hashService.Hash(usuario.Password, usuarioDB.Salt);
            if (usuarioDB.Password == resultadoHash.Hash)
            {
                return Ok(usuarioDB);
            }
            else
            {
                return Unauthorized();
            }
        }

        // Registrar una nueva cuenta de usuario
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] DTOUsuario usuario)
        {
            if (!usuarioValidator.IsValid(usuario, out var errors))
            {
                return BadRequest(new { Errors = errors });
            }

            bool yaRegistrado = await context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (yaRegistrado)
            {
                return BadRequest("Ya existe un usuario registrado con ese email.");
            }

            var resultadoHash = hashService.Hash(usuario.Password);
            var newUsuario = new Usuarios
            {
                Email = usuario.Email,
                Password = resultadoHash.Hash,
                Salt = resultadoHash.Salt
            };

            await context.Usuarios.AddAsync(newUsuario);
            await context.SaveChangesAsync();

            return Ok("Cuenta registrada exitosamente");
        }

        // Iniciar sesión con una cuenta existente
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] DTOUsuario usuario)
        {
            if (!usuarioValidator.IsValid(usuario, out var errors))
            {
                return BadRequest(new { Errors = errors });
            }

            var usuarioDB = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);
            if (usuarioDB == null)
            {
                return Unauthorized("Su cuenta no existe o su contraseña es errónea");
            }
            var resultadoHash = hashService.Hash(usuario.Password, usuarioDB.Salt);
            if (usuarioDB.Password == resultadoHash.Hash)
            {
                var response = tokenService.GenerarToken(usuario);
                return Ok(response);
            }
            else
            {
                return Unauthorized("Su cuenta no existe o su contraseña es errónea");
            }
        }

        // Endpoint HttpGet básico cpn Authorize
        [Authorize]
        [HttpGet("pruebadeautorizacion")]
        public async Task<List<Usuarios>> GetUsuarios()
        {
            return await context.Usuarios.ToListAsync();
        }
    }
}
