using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IOcupacionAppService : ICrudAppService<OcupacionDto, string>
    {
        /// <summary>
        /// Buscar la ocupacion usando el nombre
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<OcupacionDto> BuscarPorNombreAsync(string valor);

        /// <summary>
        /// Buscar la ocupacion usando el codigo de mapeo
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<OcupacionDto> BuscarPorCodigoMapeoAsync(string valor);
    }
}
