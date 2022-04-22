namespace WebApplication1.Entidades
{
    public class Autor
    {
        public int id { get; set; }
        public string Nombre { get; set; }

        public List<Libro> Libro { get; set; }

    }
}
