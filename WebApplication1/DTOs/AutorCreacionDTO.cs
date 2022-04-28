using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        public string Nombre { get; set; }
    }
}
