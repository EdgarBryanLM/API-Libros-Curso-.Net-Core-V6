using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entidades
{
    public class Comentario
    {

        public int id { get; set; }

        public string Contenido { get; set; }

        public int LibroId { get; set; }

        public Libro Libro { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

    }
}
