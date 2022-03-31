using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.RegistroPersona.Persona
{
    public enum EstadoPersonaMdg
    {
        [Display(Name = "Registrado")]
        Registrado = 1,

        [Display(Name = "Procesado")]
        Procesado = 2,
    }
}
