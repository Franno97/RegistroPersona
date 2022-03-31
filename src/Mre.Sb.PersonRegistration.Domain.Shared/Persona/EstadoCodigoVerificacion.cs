using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.RegistroPersona.Persona
{
    public enum EstadoCodigoVerificacion
    {
        [Display(Name = "Registrado")]
        Registrado = 1,

        [Display(Name = "Verificado")]
        Procesado = 2,

        [Display(Name = "Caducado")]
        Caducado = 3
    }
}
