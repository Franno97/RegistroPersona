using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    //[RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    //[Area("PersonRegistration")]
    //[Route("api/PersonRegistration/DocumentoIdentidad")]
    //public class DocumentoIdentidadController : RegistroPersonaBaseController, IDocumentoIdentidadAppService
    //{
    //    public IDocumentoIdentidadAppService IdentityDocumentAppService { get; }


    //    public DocumentoIdentidadController(IDocumentoIdentidadAppService identityDocumentAppService)
    //    {
    //        IdentityDocumentAppService = identityDocumentAppService;
    //    }


    //    [HttpGet]
    //    public virtual Task<PagedResultDto<DocumentoIdentidadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    //    {
    //        return IdentityDocumentAppService.GetListAsync(input);
    //    }

    //    [HttpGet]
    //    [Route("{id}")]
    //    public virtual Task<DocumentoIdentidadDto> GetAsync(string id)
    //    {
    //        return IdentityDocumentAppService.GetAsync(id);
    //    }

    //    [HttpPost]
    //    public virtual Task<DocumentoIdentidadDto> CreateAsync(DocumentoIdentidadDto input)
    //    {
    //        return IdentityDocumentAppService.CreateAsync(input);
    //    }

    //    [HttpPut]
    //    [Route("{id}")]
    //    public virtual Task<DocumentoIdentidadDto> UpdateAsync(string id, DocumentoIdentidadDto input)
    //    {
    //        return IdentityDocumentAppService.UpdateAsync(id, input);
    //    }

    //    [HttpDelete]
    //    [Route("{id}")]
    //    public virtual Task DeleteAsync(string id)
    //    {
    //        return IdentityDocumentAppService.DeleteAsync(id);
    //    }
    //}
}
