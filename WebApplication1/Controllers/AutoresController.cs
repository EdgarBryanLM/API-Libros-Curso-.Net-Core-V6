using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy ="EsAdmin")]

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


        [HttpGet] //api/autores
        [AllowAnonymous] //Permite que usuario no autentificados la puedan consumir 
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
           

            var autores = await context.Autores.ToListAsync();
            var autor = mapper.Map<List<AutorDTO>>(autores);
            return autor;
            // return await context.Autores.Include(x => x.Libro).ToListAsync();
        }




        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        public async Task <ActionResult<AutorDTOconLibros>> GetAutorID(int id)
        {
            var autor= await context.Autores.Include(Libro=> Libro.AutoresLibros).ThenInclude(autorLibro=>autorLibro.Libro).FirstOrDefaultAsync(x=> x.id == id);
            if (autor == null) { return new NotFoundResult(); } 
            else {



                return mapper.Map<AutorDTOconLibros>(autor);
            }
        }



        [HttpGet("/{nombre?}")] //Enviar diferentes numeros de parametros y eleguir cual es opcional y cual no 
        public async Task<ActionResult<List<AutorDTO>>> GetAutorNombre(string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            var autor = mapper.Map<List<AutorDTO>>(autores);

            return autor;
        }





        [HttpPost]
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
            return CreatedAtRoute("ObtenerAutor",new { autor.id }, autordto);
        }







        [HttpPut("{id:int}")]
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







        [HttpDelete("{id:int}")]
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
