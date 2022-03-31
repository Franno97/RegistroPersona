using Mre.Sb.RegistroPersona.Persona;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public interface IPersonaMapeadorAppService : IApplicationService
    {

        /// <summary>
        /// Mapear persona que se visualiza en pantalla antes de crear
        /// </summary>
        /// <param name="personaInformacionDto"></param>
        /// <returns></returns>
        Task<PersonaDto> MapeoPersonaDesdePersonaMdgAsync(InformacionPersonaDto personaInformacionDto);

        /// <summary>
        /// Mapear persona para persistir en la base de datos
        /// </summary>
        /// <param name="personInformationDto"></param>
        /// <returns></returns>
        Task<CrearActualizarPersonaDto> MapeoPersonaAsync(InformacionPersonaDto personInformationDto);

        /// <summary>
        /// Completar propiedades de persona
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        Task<PersonaDto> MapeoPersonaAsync(PersonaDto personaDto);
    }

}
