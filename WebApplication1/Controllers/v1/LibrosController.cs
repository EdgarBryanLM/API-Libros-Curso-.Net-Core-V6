using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers.v1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}",Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroConAutores>> Get(int id)
        {
            var libro= await context.Libros.
                Include(libro => libro.AutoresLibros)
                .ThenInclude(autorlibro => autorlibro.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (libro==null)
            {
                return NotFound();

            }

            //Ordenar por orden
            libro.AutoresLibros=libro.AutoresLibros.OrderBy(x=> x.orden).ToList();

            return mapper.Map<LibroConAutores>(libro);

            // return await context.Libros.Include(x=> x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost(Name = "CrearLibro")]
        public async Task<ActionResult> Post(LibroPost libroPost)
        {

            if(libroPost.AutoresIds== null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var AutoresIds = await context.Autores.Where(autor => libroPost.AutoresIds.Contains(autor.id)).Select(x => x.id).ToListAsync();


            if(libroPost.AutoresIds.Count != AutoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados, verifique su informacion");
            }


           var libro= mapper.Map<Libro>(libroPost);
            Orden(libro);



            context.Libros.Add(libro);
            await context.SaveChangesAsync();

            var librolink= mapper.Map<LibroGetDTO>(libro);
         //   return Ok(libro);
            return CreatedAtRoute("ObtenerLibro", new { libro.Id }, librolink);


        }

        [HttpPut("{id:int}" ,Name ="ActualizarLibro")]
        public async Task<ActionResult> Put(int id,LibroPost libroPost)
        {
            var librodb= await context.Libros.Include(x=> x.AutoresLibros).FirstOrDefaultAsync(x=> x.Id == id);


            if (librodb == null)
            {
                return NotFound();
            }
            librodb=mapper.Map(libroPost,librodb);
            Orden(librodb);

            await context.SaveChangesAsync();
            return NoContent();

            

        }

        [HttpPatch("{id:int}",Name ="PatchLibro")]
        public async Task<ActionResult> Patch(int id,JsonPatchDocument<LibroPachtDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libro = await context.Libros.FirstOrDefaultAsync(x=> x.Id == id);

            if(libro == null)
            {
                return NotFound();
            }


            var librobd= mapper.Map<LibroPachtDTO>(libro);

            patchDocument.ApplyTo(librobd,ModelState);


            var esValido = TryValidateModel(librobd);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(librobd, libro);

            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}",Name ="BorrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (existe)
            {
                context.Remove(new Libro() { Id = id });
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        private void Orden(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].orden = i;

                }
            }
        }
    }
}
