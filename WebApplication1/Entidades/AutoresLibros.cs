namespace WebApplication1.Entidades
{
    public class AutoresLibros
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }

        public int orden { get; set; }
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }

    }
}
