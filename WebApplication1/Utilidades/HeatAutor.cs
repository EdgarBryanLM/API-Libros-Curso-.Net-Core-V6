using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Utilidades
{
    public class HeatAutor : HeateFiltroAttribute
    {

        /*    public override async Task OnResultExecitionAsunc(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                var debeincluir = debeIncluir(context);


                if (!debeincluir)
                {
                    await next();
                    return;
                }
                var resultado = context.Result as ObjectResult;
                await next();
                return;
            }*/
    }
}
