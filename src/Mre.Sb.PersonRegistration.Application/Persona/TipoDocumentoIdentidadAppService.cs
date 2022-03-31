using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.PersonRegistration.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Settings;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class TipoDocumentoIdentidadAppService :
        CrudAppService<
            TipoDocumentoIdentidad,
            TipoDocumentoIdentidadDto,
            string>,
        ITipoDocumentoIdentidadAppService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public TipoDocumentoIdentidadAppService(
            IRepository<TipoDocumentoIdentidad, string> repository,
            ISettingProvider settingProvider,
            IStringLocalizer<PersonRegistrationResource> localizer)
            : base(repository)
        {
            _settingProvider = settingProvider;
            _localizer = localizer;
        }

        public async Task<ListResultDto<TipoDocumentoIdentidadDto>> GetLookupAsync()
        {
            await CheckGetListPolicyAsync();

            var list = await Repository.GetListAsync();

            var listDto = ObjectMapper.Map<List<TipoDocumentoIdentidad>, List<TipoDocumentoIdentidadDto>>(list);

            return new ListResultDto<TipoDocumentoIdentidadDto>(listDto);
        }

        public async Task<TipoDocumentoIdentidadDto> BuscarPorNombreAsync(string valor)
        {
            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.Nombre.ToLower().Equals(valor.ToLower()));

            var queryDto = queryable.Select(x => new TipoDocumentoIdentidadDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                CodigoMapeo = x.CodigoMapeo
            });

            var entityDto = await AsyncExecuter.FirstOrDefaultAsync(queryDto);

            return entityDto;
        }

        public async Task<TipoDocumentoIdentidadDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            //Devolver valor por defecto
            if (string.IsNullOrEmpty(valor))
            {
                var defaultValue = await _settingProvider.GetOrNullAsync(PersonRegistrationSettings.Catalogo.TipoDocumentoDefault);
                if (string.IsNullOrEmpty(defaultValue))
                {
                    var message = string.Format(_localizer["RegistroPersonaConfiguracion:NoExisteConfiguracion"], PersonRegistrationSettings.Catalogo.TipoDocumentoDefault);
                    throw new UserFriendlyException(message);
                }

                var identityDocumentType = await Repository.GetAsync(defaultValue);
                var identityDocumentTypeDto = new TipoDocumentoIdentidadDto
                {
                    Id = identityDocumentType.Id,
                    Nombre = identityDocumentType.Nombre,
                    CodigoMapeo = identityDocumentType.CodigoMapeo
                };
                return identityDocumentTypeDto;
            }

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.CodigoMapeo.ToUpper().Equals(valor.ToUpper()));

            var queryDto = queryable.Select(x => new TipoDocumentoIdentidadDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                CodigoMapeo = x.CodigoMapeo
            });

            var entityDto = await AsyncExecuter.SingleOrDefaultAsync(queryDto);

            return entityDto;
        }
    }
}
