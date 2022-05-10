using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers.v1
{
    [ApiController]
    [Route("api/v1/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController: ControllerBase
    {

        //Contexto de nuestra base de datos
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(AplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

    /*    [ServiceFilter(typeof(MiFiltrodeAccion))]
        [HttpGet("GUID")]
        [ResponseCache(Duration =10)]
        public ActionResult Obtener()
        {
            
            return Ok(new
            {
                AutoresControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = servicioA.ObtenerTransient(),
                AutoresControllerScope = servicioScope.Guid,
                ServicioA_Scope = servicioA.ObtenerScope(),
                AutoresControlerSingleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicioA.ObtenerSingleton(),

            });

        } */
    /// <summary>
    /// Obtiene autores
    /// </summary>
    /// <returns></returns>

        [HttpGet(Name ="ObtenerAutoresv1")] //api/autores
        [AllowAnonymous] //Permite que usuario no autentificados la puedan consumir 
        public async Task<ColeccionRecursos<AutorDTO>> Get()
        {
           

            var autores = await context.Autores.ToListAsync();
            var dtos = mapper.Map<List<AutorDTO>>(autores);
            dtos.ForEach(dt => GenerarEnlaces(dt));
            var resultado= new ColeccionRecursos<AutorDTO> { valores=dtos};
            resultado.Enlaces.Add(new DatosHATEOS(
                enlace: Url.Link("ObtenerAutorv1", new { }), descripcion: "self", metodo: "GET"));

            resultado = new ColeccionRecursos<AutorDTO> { valores = dtos };
            resultado.Enlaces.Add(new DatosHATEOS(
                enlace: Url.Link("CrearAutorv1", new { }), descripcion: "self", metodo: "POST"));

            return resultado;
            // return await context.Autores.Include(x => x.Libro).ToListAsync();
        }






        [HttpGet("{id:int}", Name = "ObtenerAutorv1")]
        public async Task <ActionResult<AutorDTOconLibros>> GetAutorID(int id)
        {
            var autor= await context.Autores.Include(Libro=> Libro.AutoresLibros).ThenInclude(autorLibro=>autorLibro.Libro).FirstOrDefaultAsync(x=> x.id == id);
            if (autor == null) { return new NotFoundResult(); } 
            else {
                var dto= mapper.Map<AutorDTOconLibros>(autor);
                GenerarEnlaces(dto);
                return dto;
            }   
        }


        private void GenerarEnlaces(AutorDTO autorDTO)
        {
            autorDTO.Enlaces.Add(new DatosHATEOS(Url.Link("ObtenerAutoresv1", new { id = autorDTO.id }), descripcion: "Get-autores", metodo: "GET"));

            autorDTO.Enlaces.Add(new DatosHATEOS(Url.Link("ActualizarAutorv1", new { id = autorDTO.id }), descripcion: "put-autores", metodo: "PUT"));


            autorDTO.Enlaces.Add(new DatosHATEOS(Url.Link("BorrarAutorv1", new { id = autorDTO.id }), descripcion: "delete-autores", metodo: "DELETE"));

        }



        [HttpGet("/{nombre?}",Name ="ObtenerAutorPorNombrev1")] //Enviar diferentes numeros de parametros y eleguir cual es opcional y cual no 
        public async Task<ActionResult<List<AutorDTO>>> GetAutorNombre(string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            var autor = mapper.Map<List<AutorDTO>>(autores);

            return autor;
        }





        [HttpPost(Name = "CrearAutorv1")]
        public async Task<ActionResult> Post(AutorCreacionDTO autorDTO)
        {
            var existeAutor= await context.Autores.AnyAsync(x=> x.Nombre== autorDTO.Nombre);

            if (existeAutor)
            {
                return BadRequest($"El autor con nombre {autorDTO.Nombre} ya existe, registra otro autor");
            }

            var autor = mapper.Map<Autor>(autorDTO);
            //Agregamos un autor a la base de datos
            context.Add(autor);

            //Guardamos los cambios en la base de datos
            await context.SaveChangesAsync();
            var autordto= mapper.Map<AutorDTO>(autor);
            // return Ok();
            return CreatedAtRoute("ObtenerAutorv1",new { autor.id }, autordto);
        }







        [HttpPut("{id:int}",Name ="ActualizarAutorv1")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorDTO,int id)
        {

            //If para comprovar que el id del autor y el id de que mandamos sea el mismo
         /*   if(autor.id != id)
            {
                return BadRequest("El ID del autor no coincide con el ID de la URL");
            } */

            //Comprueva si existe un registro con ese id
            var existe = await context.Autores.AnyAsync(x => x.id == id);

            //Si existe lleva acabo la operacion
            if (existe)
            {

                var autor = mapper.Map<Autor>(autorDTO);
                autor.id = id;

                context.Update(autor);

                await context.SaveChangesAsync();
                return Ok();
            }
            //Si no existe retorna un 404 no found
            else
            {
                return NotFound();
            }
           
        }






        /// <summary>
        /// Borra un autor de la base de datos
        /// </summary>
        /// <param name="id"> ID del autor que se desea borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}",Name ="BorrarAutorv1")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.id==id);

            if (existe)
            {
                context.Remove(new Autor() {id = id});
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
