using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.PersonRegistration.Settings;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Settings;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class TipoVisaAppService :
        CrudAppService<
            TipoVisa,
            TipoVisaDto,
            string>,
        ITipoVisaAppService
    {
        private readonly TipoVisaManager _visaTypeManager;
        private readonly ISettingProvider _settingProvider;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public TipoVisaAppService(IRepository<TipoVisa, string> repository,
            TipoVisaManager visaTypeManager,
            ISettingProvider settingProvider,
            IStringLocalizer<PersonRegistrationResource> localizer)
            : base(repository)
        {
            _visaTypeManager = visaTypeManager;
            _settingProvider = settingProvider;
            _localizer = localizer;

        }

        public override async Task<TipoVisaDto> CreateAsync(TipoVisaDto input)
        {
            await CheckCreatePolicyAsync();

            await _visaTypeManager.ValidarCreacionAsync(input.Id);

            var entity = await MapToEntityAsync(input);

            TryToSetTenantId(entity);

            await Repository.InsertAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<TipoVisaDto> BuscarPorNombreAsync(string valor)
        {
            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.Nombre.ToLower().Equals(valor.ToLower()));

            var queryDto = queryable.Select(x => new TipoVisaDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                CodigoMapeo = x.CodigoMapeo
            });

            var entityDto = await AsyncExecuter.FirstOrDefaultAsync(queryDto);

            return entityDto;
        }

        public async Task<TipoVisaDto> BuscarPorCodigoMapeoAsync(string valor)
        {
            //Devolver valor por defecto
            if(string.IsNullOrEmpty(valor))
            {
                var defaultValue= await _settingProvider.GetOrNullAsync(PersonRegistrationSettings.Catalogo.TipoVisaDefault);
                if(string.IsNullOrEmpty(defaultValue))
                {
                    var message = string.Format(_localizer["RegistroPersonaConfiguracion:NoExisteConfiguracion"], PersonRegistrationSettings.Catalogo.TipoVisaDefault);
                    throw new UserFriendlyException(message);
                }

                var visaType= await Repository.GetAsync(defaultValue);
                var visaTypeDto = new TipoVisaDto
                {
                    Id = visaType.Id,
                    Nombre = visaType.Nombre,
                    CodigoMapeo = visaType.CodigoMapeo
                };
                return visaTypeDto;
            }

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.CodigoMapeo.ToUpper().Equals(valor.ToUpper()));

            var queryDto = queryable.Select(x => new TipoVisaDto
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