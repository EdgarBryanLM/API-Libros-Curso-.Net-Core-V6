using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class LibroPachtDTO
    {
        [StringLength(maximumLength: 200, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        [Required]
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }

    }
}
