using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public interface IFamiliarProcesoAppService : IApplicationService
    {

        /// <summary>
        /// Realizar validaciones de información de la persona
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input);

        /// <summary>
        /// Generar y enviar codigo de verificacion
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro);

        /// <summary>
        /// Validar codigo de verificacíon
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input);

        /// <summary>
        /// Obtienes los datos de la persona
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<PersonaDto> ObtenerInformacionPersonaAsync(string numeroRegistro);

        /// <summary>
        /// Registrar la persona
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<PersonaDto> GuardarPersonaAsync(string numeroRegistro);

    }
}
