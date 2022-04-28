using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class LibroPost
    {
        [StringLength(maximumLength: 200, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        [Required]
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }

        public List<int> AutoresIds { get; set; }
    }
}
