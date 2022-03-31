using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/Ocupacion")]
    public class OcupacionController : RegistroPersonaBaseController, IOcupacionAppService
    {
        public IOcupacionAppService OcupacionAppService { get; }


        public OcupacionController(IOcupacionAppService ocupacionAppService)
        {
            OcupacionAppService = ocupacionAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<OcupacionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return OcupacionAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<OcupacionDto> GetAsync(string id)
        {
            return OcupacionAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<OcupacionDto> CreateAsync(OcupacionDto input)
        {
            return OcupacionAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<OcupacionDto> UpdateAsync(string id, OcupacionDto input)
        {
            return OcupacionAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return OcupacionAppService.DeleteAsync(id);
        }

        [HttpGet("buscarPorNombre/{valor}")]
        public async Task<OcupacionDto> BuscarPorNombreAsync(string valor)
        {
            return await OcupacionAppService.BuscarPorNombreAsync(valor);
        }
        
        [HttpGet("buscarPorCodigoMapeo/{valor}")]
        public Task<OcupacionDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            throw new System.NotImplementedException();
        }
    }
}
