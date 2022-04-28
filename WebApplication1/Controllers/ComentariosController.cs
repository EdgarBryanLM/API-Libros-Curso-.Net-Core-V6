using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{

    [ApiController] 
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


    /*    [HttpGet("/all")]
        public async Task<ActionResult<List<ComentarioDTO>>> Getall(int libroId)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existe)
            {

                return BadRequest($"El ID:{libroId} no esta relacionado a ningun libro");
            }
            var existe2 = await context.Comentarios.AnyAsync(x => x.LibroId == libroId);
            if (!existe2)
            {

                return NotFound($"El ID:{libroId} no esta relacionado comentario");
            }

            var comentarios = await context.Comentarios.ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        } */

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Geta(int libroId)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existe)
            {
             
                return NotFound($"El ID:{libroId} no esta relacionado a ningun libro");
            }

            var existe2 = await context.Comentarios.AnyAsync(x => x.LibroId == libroId);
            if (!existe2)
            {

                return NotFound($"El ID:{libroId} no esta relacionado a ningun libro");
            }


            var comentarios= await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }


        [HttpGet("{id:int}",Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> Get(int libroId, int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existe)
            {

                return NotFound($"El ID:{libroId} no esta relacionado a ningun libro");
            }

            var existe2 = await context.Comentarios.AnyAsync(x => x.LibroId == libroId);
            if (!existe2)
            {

                return NotFound($"El ID:{libroId} no esta relacionado a ningun libro");
            }


            var comentario = context.Comentarios.FirstOrDefault(z => z.id == id);

            return mapper.Map<ComentarioDTO>(comentario);
        }







        [HttpPost]
        public async Task<IActionResult> Post(int libroId, ComentarioPostDTO comentarioPostDTO)
        {

            var existe = await context.Libros.AnyAsync(x=> x.Id== libroId);
            if (!existe)
            {
                return NotFound();
                //return BadRequest($"El ID:{libroId} no esta relacionado a ningun libro");
            }

            var comentario = mapper.Map<Comentario>(comentarioPostDTO);
            comentario.LibroId = libroId;

            context.Add(comentario);
            context.SaveChanges();

            var com = mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { comentario.LibroId,comentario.id }, com);

            // return Ok();
        }




        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId,ComentarioPostDTO comentarioDTO, int id)
        {
            var existel = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existel)
            {
                return NotFound();
                //return BadRequest($"El ID:{libroId} no esta relacionado a ningun libro");
            }

            var existe = await context.Comentarios.AnyAsync(x => x.id == id);

            //Si existe lleva acabo la operacion
            if (existe)
            {

                var comentario = mapper.Map<Comentario>(comentarioDTO);
                comentario.id = id;
                comentario.LibroId=libroId;
                context.Update(comentario);

                await context.SaveChangesAsync();
                return NoContent();
            }
            //Si no existe retorna un 404 no found
            else
            {
                return NotFound();
            }
        }


    }
}
