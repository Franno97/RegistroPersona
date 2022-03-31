using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona
{
    public abstract class PersonRegistrationAppService : ApplicationService
    {
        protected PersonRegistrationAppService()
        {
            LocalizationResource = typeof(PersonRegistrationResource);
            ObjectMapperContext = typeof(PersonRegistrationApplicationModule);
        }
    }
}
