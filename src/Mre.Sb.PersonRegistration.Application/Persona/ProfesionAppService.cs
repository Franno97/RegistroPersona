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
    public class ProfesionAppService :
        CrudAppService<
            Profesion,
            ProfesionDto,
            string>,
        IProfesionAppService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public ProfesionAppService(
            IRepository<Profesion, string> repository,
            ISettingProvider settingProvider,
            IStringLocalizer<PersonRegistrationResource> localizer)
            : base(repository)
        {
            _settingProvider = settingProvider;
            _localizer = localizer;
        }

        public async Task<ProfesionDto> BuscarPorNombreAsync(string searchValue)
        {
            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.Nombre.ToLower().Equals(searchValue.ToLower()));

            var queryDto = queryable.Select(x => new ProfesionDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                CodigoMapeo = x.CodigoMapeo
            });

            var entityDto = await AsyncExecuter.FirstOrDefaultAsync(queryDto);

            return entityDto;
        }

        public async Task<ProfesionDto> BuscarPorCodigoMapeoAsync(string searchValue)
        {
            //Devolver valor por defecto
            if (string.IsNullOrEmpty(searchValue))
            {
                var defaultValue = await _settingProvider.GetOrNullAsync(PersonRegistrationSettings.Catalogo.ProfesionDefault);
                if (string.IsNullOrEmpty(defaultValue))
                {
                    var message = string.Format(_localizer["RegistroPersonaConfiguracion:NoExisteConfiguracion"], PersonRegistrationSettings.Catalogo.ProfesionDefault);
                    throw new UserFriendlyException(message);
                }

                var profession = await Repository.GetAsync(defaultValue);
                var professionDto = new ProfesionDto
                {
                    Id = profession.Id,
                    Nombre = profession.Nombre,
                    CodigoMapeo = profession.CodigoMapeo
                };
                return professionDto;
            }

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.CodigoMapeo.ToUpper().Equals(searchValue.ToUpper()));

            var queryDto = queryable.Select(x => new ProfesionDto
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
