using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers.v1
{
    [ApiController]
    [Route("api/v1")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class RootController:ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }


        [HttpGet(Name ="ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatosHATEOS>>> get()
        {
            var datos=new List<DatosHATEOS>();

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            datos.Add(new DatosHATEOS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "saelf", metodo: "GET"));


            datos.Add(new DatosHATEOS(enlace: Url.Link("ObtenerAutores", new {}),descripcion:"autores",metodo: "GET"));


            if (esAdmin.Succeeded)
            {
                datos.Add(new DatosHATEOS(enlace: Url.Link("CrearAutor", new { }), descripcion: "autores-crear", metodo: "POST"));

                datos.Add(new DatosHATEOS(enlace: Url.Link("CrearLibro", new { }), descripcion: "libros-crear", metodo: "POST"));
            }



            return datos;
        }
    }
}
