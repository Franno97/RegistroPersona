using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public interface IClientExternalDmzAppService : IApplicationService
    {
        /// <summary>
        /// Consultar informacion de ciudadanos
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<InformacionPersonaDto> ObtenerInformacionPersona(string numeroRegistro);

        /// <summary>
        /// Consultar si el ciudadano ingreso al pais por un punto regular
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<ConsultarPuntoControlOutput> ConsultarPuntoControlAsync(string numeroRegistro);

        /// <summary>
        /// Consulta multas de un ciudadano
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<ConsultarMultasRespuesta> ConsultarMultasAsync(string numeroRegistro);

        /// <summary>
        /// Consulta de flujo migratorio del ciudadano
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<ConsultarFlujoMigratorioRespuesta> ConsultarFlujoMigratorioAsync(string numeroRegistro);

        /// <summary>
        /// Consulta de pagos
        /// </summary>
        /// <param name="numeroComprobante"></param>
        /// <returns></returns>
        Task<ConsultarPagoRespuesta> ConsultarPagoAsync(string numeroComprobante);

        /// <summary>
        /// Consulta de discapacidad
        /// </summary>
        /// <param name="numeroCedula"></param>
        /// <returns></returns>
        Task<ConsultarDiscapacidadRespuesta> ConsultarDiscapacidadAsync(string numeroCedula);


    }
}
