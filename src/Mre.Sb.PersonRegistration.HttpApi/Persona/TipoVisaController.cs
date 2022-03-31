using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/TipoVisa")]
    public class TipoVisaController : RegistroPersonaBaseController, ITipoVisaAppService
    {
        public ITipoVisaAppService VisaTypeAppService { get; }


        public TipoVisaController(ITipoVisaAppService visaTypeAppService)
        {
            VisaTypeAppService = visaTypeAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<TipoVisaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return VisaTypeAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<TipoVisaDto> GetAsync(string id)
        {
            return VisaTypeAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<TipoVisaDto> CreateAsync(TipoVisaDto input)
        {
            return VisaTypeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<TipoVisaDto> UpdateAsync(string id, TipoVisaDto input)
        {
            return VisaTypeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return VisaTypeAppService.DeleteAsync(id);
        }

        [HttpGet("buscarPorNombre/{valor}")]
        public Task<TipoVisaDto> BuscarPorNombreAsync(string valor)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("buscarPorCodigoMapeo/{valor}")]
        public Task<TipoVisaDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            throw new System.NotImplementedException();
        }
    }
}
