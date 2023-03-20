using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filtros
{
    
    public class MiFiltrodeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltrodeAccion> logger;

        public MiFiltrodeAccion(ILogger<MiFiltrodeAccion> logger)
        {
            this.logger = logger;
        }
        // esto se ejecunta antes de ejecutar la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("antes de ejecutar la accion");
        }
        // despues de ejecutar la accion
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("despues de ejecutar la accion");

        }



    }
}
