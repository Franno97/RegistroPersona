using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class PersonRegistrationDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PersonRegistrationDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PersonRegistrationResource>("es")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/PersonRegistration");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("PersonRegistration", typeof(PersonRegistrationResource));
            });
        }
    }
}
