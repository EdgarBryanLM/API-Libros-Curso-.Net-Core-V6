using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {

        //Contexto de nuestra base de datos
        private readonly AplicationDbContext context;

        public AutoresController(AplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet] //api/autores
        [HttpGet ("listado")] //api/autores/listado
        [HttpGet("/listado")]//listado
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(x => x.Libro).ToListAsync();
        }


        [HttpGet("{id:int}")]
        public async Task <ActionResult<Autor>> GetAutorID(int id)
        {
            var autor= await context.Autores.FirstOrDefaultAsync(x=> x.id == id);
            if (autor == null) { return new NotFoundResult(); } else { return autor; }
        }

        [HttpGet("{nombre}/{nombre?}")] //Enviar diferentes numeros de parametros y eleguir cual es opcional y cual no 
        public async Task<ActionResult<Autor>> GetAutorNombre(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (autor == null) { return new NotFoundResult(); } else { return autor; }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            //Agregamos un autor a la base de datos
            context.Add(autor);

            //Guardamos los cambios en la base de datos
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor,int id)
        {

            //If para comprovar que el id del autor y el id de que mandamos sea el mismo
            if(autor.id != id)
            {
                return BadRequest("El ID del autor no coincide con el ID de la URL");
            }
            //Comprueva si existe un registro con ese id
            var existe = await context.Autores.AnyAsync(x => x.id == id);

            //Si existe lleva acabo la operacion
            if (existe)
            {
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
