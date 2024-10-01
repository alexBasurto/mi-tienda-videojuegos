using WebApiTiendaVideojuegos.Interface;

namespace WebApiTiendaVideojuegos.Services
{
    public class GestorArchivos : IGestorArchivos
    {
        private readonly IWebHostEnvironment env; // Para poder localizar wwwroot
        private readonly IHttpContextAccessor httpContextAccessor; // Para conocer la configuración del servidor para construir la url de la imagen

        public GestorArchivos(IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string carpeta)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(env.WebRootPath, carpeta, nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string ruta,
            string contentType)
        {
            await BorrarArchivo(ruta, carpeta);
            return await GuardarArchivo(contenido, extension, carpeta, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta,
            string contentType)
        {
            // Creamos un nombre aleatorio con la extensión
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            // La ruta será wwwroot/carpeta (en este caso imagenes)
            string folder = Path.Combine(env.WebRootPath, carpeta);

            // Si no existe la carpeta la creamos
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // La ruta donde dejaremos el archivo será la concatenación de la ruta de la carpeta y el nombre del archivo
            string ruta = Path.Combine(folder, nombreArchivo);
            // Guardamos el archivo
            await File.WriteAllBytesAsync(ruta, contenido);

            // La url de la ímagen será http o https://dominio/carpeta/nombreimagen
            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, carpeta, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
        }
    }
}
