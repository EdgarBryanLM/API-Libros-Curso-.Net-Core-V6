using WebApplication1.Entidades;

namespace WebApplication1.DTOs
{
    public class LibroGetDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; }
        public DateTime? Fecha { get; set; }




        // public List<ComentarioDTO> Comentarios { get; set; }
    }
}
