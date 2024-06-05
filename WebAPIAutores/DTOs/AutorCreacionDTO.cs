using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "el campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "el campo {0} no debe de tener de {1} caracteres ")]
        [PrimeraLetraMayuscula] 
        public string Nombre { get; set; }
    }
}
