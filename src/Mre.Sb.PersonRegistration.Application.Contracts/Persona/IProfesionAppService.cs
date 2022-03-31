using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IProfesionAppService : ICrudAppService<ProfesionDto,string>
    {
        /// <summary>
        /// Buscar la profesion usando el nombre
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<ProfesionDto> BuscarPorNombreAsync(string valor);

        /// <summary>
        /// Buscar la profesion usando el codigo de mapeo
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        Task<ProfesionDto> BuscarPorCodigoMapeoAsync(string valor);
    }
}
