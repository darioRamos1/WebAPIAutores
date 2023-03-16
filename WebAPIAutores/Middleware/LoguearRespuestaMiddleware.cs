using Microsoft.Extensions.Logging;

namespace WebAPIAutores.Middleware
{
    public static class LoguearRespuestaMiddlewareExtensions
    {
        // esto permite no exponer la clase directamente solo se usa el metodo que se creo

        public static IApplicationBuilder UseLoguearRespuesta(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaMiddleware>();
        }
    }

    public class LoguearRespuestaMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaMiddleware> logger;

        public LoguearRespuestaMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        // Invoke o InvokeAsync
        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;
                await siguiente(contexto);
                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;
                logger.LogInformation(respuesta);
            }
        }
    }
}
