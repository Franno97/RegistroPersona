using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.RegistroPersona.Persona
{

    public class TipoVisaManager : DomainService
    {
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        private readonly IRepository<TipoVisa, string> _repository;


        public TipoVisaManager(IRepository<TipoVisa, string> repository,
            IStringLocalizer<PersonRegistrationResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;

        }

        public async Task ValidarCreacionAsync(string input)
        {

            var exist = await _repository.AnyAsync(e => e.Id.ToUpper() == input.ToUpper());
            if (exist)
            {
                var msg = string.Format(_localizer["VisaType:Exist"], input);
                throw new UserFriendlyException(msg);
            }
        }
    }


}
