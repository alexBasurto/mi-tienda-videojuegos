using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiTiendaVideojuegos.Filters
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> logger;
        private readonly IWebHostEnvironment env;

        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);

            var path = $@"{env.ContentRootPath}\wwwroot\error.txt";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(context.Exception.Message);
            }

            base.OnException(context);
        }
    }
}
