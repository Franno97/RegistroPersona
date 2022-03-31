using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/Profesion")]
    public class ProfesionController : RegistroPersonaBaseController, IProfesionAppService
    {
        public IProfesionAppService ProfessionAppService { get; }


        public ProfesionController(IProfesionAppService professionAppService)
        {
            ProfessionAppService = professionAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<ProfesionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return ProfessionAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<ProfesionDto> GetAsync(string id)
        {
            return ProfessionAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<ProfesionDto> CreateAsync(ProfesionDto input)
        {
            return ProfessionAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<ProfesionDto> UpdateAsync(string id, ProfesionDto input)
        {
            return ProfessionAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return ProfessionAppService.DeleteAsync(id);
        }

        [HttpGet("buscarPorNombre/{valor}")]
        public Task<ProfesionDto> BuscarPorNombreAsync(string valor)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("buscarPorCodigoMapeo/{valor}")]
        public Task<ProfesionDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            throw new System.NotImplementedException();
        }
    }
}
