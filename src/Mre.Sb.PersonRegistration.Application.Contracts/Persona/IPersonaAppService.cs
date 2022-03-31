using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IPersonaAppService : ICrudAppService<PersonaDto, Guid, ObtenerPersonaInputDto, CrearActualizarPersonaDto>
    {
        /// <summary>
        /// Devuelve informacion de la persona segun el nombre de usuario
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        public Task<PersonaDto> ObtenerPersonaPorNombreUsuarioAsync(string nombreUsuario);


        /// <summary>
        /// Devuelve información propia de la persona que se encuentra logueada en el sistema
        /// </summary>
        /// <returns></returns>
        public Task<PersonaDto> ObtenerPersonaActualAsync();
    }
}
