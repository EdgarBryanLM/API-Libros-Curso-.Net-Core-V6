using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1
{
    public class AplicationDbContext : IdentityDbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutoresLibros>().HasKey(x => new {x.AutorId,x.LibroId});
        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutoresLibros> AutoresLibros { get; set; }


    }
}
