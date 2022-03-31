using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/persona")]
    public class PersonaController : RegistroPersonaBaseController, IPersonaAppService
    {
        public IPersonaAppService PersonAppService { get; }


        public PersonaController(IPersonaAppService entityAppService)
        {
            PersonAppService = entityAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<PersonaDto>> GetListAsync(ObtenerPersonaInputDto input)
        {
            return PersonAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PersonaDto> GetAsync(Guid id)
        {
            return PersonAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<PersonaDto> CreateAsync(CrearActualizarPersonaDto input)
        {
            return PersonAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PersonaDto> UpdateAsync(Guid id, CrearActualizarPersonaDto input)
        {
            return PersonAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return PersonAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("usuario/{nombreUsuario}")]
        public Task<PersonaDto> ObtenerPersonaPorNombreUsuarioAsync(string nombreUsuario)
        {
            return PersonAppService.ObtenerPersonaPorNombreUsuarioAsync(nombreUsuario);
        }

        [HttpGet]
        [Route("actual")]
        public Task<PersonaDto> ObtenerPersonaActualAsync()
        {
            return PersonAppService.ObtenerPersonaActualAsync();
        }
    }
}
