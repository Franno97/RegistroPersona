using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IEstadoCivilAppService : ICrudAppService<EstadoCivilDto, string>
    {
        /// <summary>
        /// Buscar el estado civil usando el nombre
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        Task<EstadoCivilDto> BuscarPorNombreAsync(string searchValue);

        /// <summary>
        /// Buscar el estado civil usando el codigo de mapeo
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        Task<EstadoCivilDto> BuscarPorCodigoMapeoAsync(string searchValue);
    }
}
