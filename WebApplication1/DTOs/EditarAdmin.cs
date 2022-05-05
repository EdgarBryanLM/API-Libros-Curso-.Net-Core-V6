using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class EditarAdmin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
