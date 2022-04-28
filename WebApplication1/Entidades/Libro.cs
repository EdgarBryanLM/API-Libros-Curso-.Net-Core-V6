using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        public string Titulo { get; set; }
        public DateTime? Fecha { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List <AutoresLibros> AutoresLibros { get; set; }

    }
}
