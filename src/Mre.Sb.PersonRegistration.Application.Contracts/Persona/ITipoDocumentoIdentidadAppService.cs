using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface ITipoDocumentoIdentidadAppService : ICrudAppService<TipoDocumentoIdentidadDto, string>
    {
        /// <summary>
        /// Lookup de tipos de documento de identidad
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<TipoDocumentoIdentidadDto>> GetLookupAsync();


        /// <summary>
        /// Buscar el tipo de documento usando el nombre
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<TipoDocumentoIdentidadDto> BuscarPorNombreAsync(string valor);

        /// <summary>
        /// Buscar el tipo de documento usando el codigo de mapeo
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<TipoDocumentoIdentidadDto> BuscarPorCodigoMapeoAsync(string valor);
    }
}
