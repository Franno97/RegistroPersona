using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface INivelEducativoAppService : ICrudAppService<NivelEducativoDto,string>
    {
        /// <summary>
        /// Buscar el nivel educativo usando el nombre
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        Task<NivelEducativoDto> BuscarPorNombreAsync(string searchValue);

        /// <summary>
        /// Buscar el nivel educativo usando el codigo de mapeo
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        Task<NivelEducativoDto> BuscarPorCodigoMapeoAsync(string searchValue);
    }
}
