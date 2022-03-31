using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IPersonaConfiguracionAppService
    {
        /// <summary>
        /// Obtener las configuraciones
        /// </summary>
        /// <returns></returns>
        Task<PersonaConfiguracionDto> ObtenerAsync();

        /// <summary>
        /// Actualizar las configuraciones
        /// </summary>
        /// <returns></returns>
        Task ActualizarAsync(ActualizarPersonaConfiguracionDto input);
    }

    public class PersonaConfiguracionDto
    {
        public bool ValidarPuntoDeAcceso { get; set; }

        public bool ValidarNacionalidad { get; set; }

        public string NacionalidadesPermitidas { get; set; }

        public bool ValidarMayoriaEdad { get; set; }

        public int EdadMinima { get; set; }

        public bool HabilitarCaptcha { get; set; }

        public DateTime FechaInicialControl { get; set; }

        public DateTime FechaFinalControl { get; set; }

        public int VigenciaInformacion { get; set; }

        public int DuracionCodigoVerificacion { get; set; }

        public int IntentosPermitidos { get; set; }

        public int DuracionBloqueo { get; set; }
    }

    public class ActualizarPersonaConfiguracionDto
    {
        public bool ValidarPuntoDeAcceso { get; set; }

        public bool ValidarNacionalidad { get; set; }

        [Required]
        public string NacionalidadesPermitidas { get; set; }

        public bool ValidarMayoriaEdad { get; set; }

        public int EdadMinima { get; set; }

        public bool HabilitarCaptcha { get; set; }

        public DateTime FechaInicialControl { get; set; }

        public DateTime FechaFinalControl { get; set; }

        public int VigenciaInformacion { get; set; }

        public int DuracionCodigoVerificacion { get; set; }

        public int IntentosPermitidos { get; set; }

        public int DuracionBloqueo { get; set; }
    }
}
