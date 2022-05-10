namespace WebApplication1.DTOs
{
    public class DatosHATEOS
    {
        public string Enlace { get; set; }
        public string Descripcion { get; set; }
        public string Metodo { get; set; }

        public DatosHATEOS(string enlace, string descripcion,string metodo)
        {
            Enlace = enlace;
            Descripcion = descripcion;
            Metodo = metodo;
        }

    }
}
