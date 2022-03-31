using Localization.Resources.AbpUi;
using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(PersonRegistrationApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]

    [DependsOn(
        typeof(AbpIdentityModelModule))]
    public class PersonRegistrationHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PersonRegistrationHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PersonRegistrationResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
