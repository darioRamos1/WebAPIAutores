using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "el campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "el campo {0} no debe de tener de {1} caracteres ")]
        // [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        /*[Range(18, 100)]
        [NotMapped] // ESTO PERMITE NO PASAR ESTE ATRIBUTO A LA BD
        public int Edad { get; set; }
        [CreditCard]
        [NotMapped]
        public string TarjetaCredito { get; set; }*/
        public List<Libro> Libros { get; set; }

        [NotMapped]
        public int Mayor { get; set; }

        [NotMapped]
        public int Menor { get; set; }

        // validacion por modelo 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // el yield se utiliza para recolectar las diferentes validaciones arrojarlas 
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                         new string[] { nameof(Nombre) });
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser mas grande que el campo mayor ",
                    new string[] { nameof(Menor) });
            }
        }



    }
}
