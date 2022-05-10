namespace WebApplication1.DTOs
{
    public class ColeccionRecursos<T>:Recurso where T:Recurso
    {

        public List<T> valores { get; set; }
    }
}
