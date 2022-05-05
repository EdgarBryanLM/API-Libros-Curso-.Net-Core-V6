using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CredencialesDTOcs
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }

}