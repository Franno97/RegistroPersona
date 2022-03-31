using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{

    //[RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    //[Area("PersonRegistration")]
    //[Route("api/PersonRegistration/visa")]
    //public partial class VisaController : RegistroPersonaBaseController, IVisaAppService
    //{
    //    public IVisaAppService VisaAppService { get; }


    //    public VisaController(IVisaAppService visaAppService)
    //    {
    //        VisaAppService = visaAppService;
    //    }


    //    [HttpGet]
    //    public virtual Task<PagedResultDto<VisaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    //    {
    //        return VisaAppService.GetListAsync(input);
    //    }

    //    [HttpGet]
    //    [Route("{id}")]
    //    public virtual Task<VisaDto> GetAsync(string id)
    //    {
    //        return VisaAppService.GetAsync(id);
    //    }

    //    [HttpPost]
    //    public virtual Task<VisaDto> CreateAsync(VisaDto input)
    //    {
    //        return VisaAppService.CreateAsync(input);
    //    }

    //    [HttpPut]
    //    [Route("{id}")]
    //    public virtual Task<VisaDto> UpdateAsync(string id, VisaDto input)
    //    {
    //        return VisaAppService.UpdateAsync(id, input);
    //    }

    //    [HttpDelete]
    //    [Route("{id}")]
    //    public virtual Task DeleteAsync(string id)
    //    {
    //        return VisaAppService.DeleteAsync(id);
    //    }
    //}
}
