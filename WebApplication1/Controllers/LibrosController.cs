using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly AplicationDbContext context;

        public LibrosController(AplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(x=> x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {

            var existe= await context.Autores.AnyAsync(x=> x.id==libro.AutorId);

            if (existe)
            {
                context.Libros.Add(libro);
                await context.SaveChangesAsync();
                return Ok(libro);
            }
            else
            {
                return BadRequest($"No existe el autor de ID {libro.AutorId}");
            }

        }
    }
}
