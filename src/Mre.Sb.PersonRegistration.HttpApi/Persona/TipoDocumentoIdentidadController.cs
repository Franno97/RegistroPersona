using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/TipoDocumentoIdentidad")]
    public class TipoDocumentoIdentidadController : RegistroPersonaBaseController, ITipoDocumentoIdentidadAppService
    {
        public ITipoDocumentoIdentidadAppService IdentityDocumentTypeAppService { get; }


        public TipoDocumentoIdentidadController(ITipoDocumentoIdentidadAppService identityDocumentTypeDtoAppService)
        {
            IdentityDocumentTypeAppService = identityDocumentTypeDtoAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<TipoDocumentoIdentidadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return IdentityDocumentTypeAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<TipoDocumentoIdentidadDto> GetAsync(string id)
        {
            return IdentityDocumentTypeAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<TipoDocumentoIdentidadDto> CreateAsync(TipoDocumentoIdentidadDto input)
        {
            return IdentityDocumentTypeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<TipoDocumentoIdentidadDto> UpdateAsync(string id, TipoDocumentoIdentidadDto input)
        {
            return IdentityDocumentTypeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return IdentityDocumentTypeAppService.DeleteAsync(id);
        }

        [HttpGet("buscarPorNombre/{valor}")]
        public async Task<TipoDocumentoIdentidadDto> BuscarPorNombreAsync(string valor)
        {
            return await IdentityDocumentTypeAppService.BuscarPorNombreAsync(valor);
        }

        [HttpGet("lookup")]
        public async Task<ListResultDto<TipoDocumentoIdentidadDto>> GetLookupAsync()
        {
            return await IdentityDocumentTypeAppService.GetLookupAsync();
        }

        [HttpGet("buscarPorCodigoMapeo/{valor}")]
        public Task<TipoDocumentoIdentidadDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            throw new System.NotImplementedException();
        }
    }
}
