using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface ITipoVisaAppService : ICrudAppService<TipoVisaDto,string>
    {
        /// <summary>
        /// Buscar el tipo de visa usando el nombre
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<TipoVisaDto> BuscarPorNombreAsync(string valor);

        /// <summary>
        /// Buscar el tipo de visa usando el codigo de mapeo
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<TipoVisaDto> BuscarPorCodigoMapeoAsync(string valor);
    }
}
