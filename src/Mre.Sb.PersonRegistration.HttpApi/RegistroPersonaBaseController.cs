using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Mre.Sb.RegistroPersona
{
    public abstract class RegistroPersonaBaseController : AbpController
    {
        protected RegistroPersonaBaseController()
        {
            LocalizationResource = typeof(PersonRegistrationResource);
        }
    }
}
