using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class MiFiltrodeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltrodeAccion> logger;

        public MiFiltrodeAccion(ILogger<MiFiltrodeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de jecutar la accion");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de jecutar la accion");
        }


    }
}
