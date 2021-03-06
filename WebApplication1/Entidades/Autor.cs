using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entidades
{
    public class Autor
    {
        public int id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength (maximumLength:20,ErrorMessage ="El campo {0} no debe de tener mas de {1} caracteres")]
        public string Nombre { get; set; }
        public List<AutoresLibros> AutoresLibros { get; set; }




        /*  Validacion de numeros de rangos
         *  [Range(18,120)]
            public int edad { get; set; }
        */

        /*  [CreditCard]
          public string TarjetaDeCredito { get; set; }
        */
    }
}
