using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Mre.Sb.Geographical;
using Volo.Abp.IdentityModel;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(PersonRegistrationDomainModule),
        typeof(PersonRegistrationApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    [DependsOn(
        typeof(AbpIdentityModelModule))]
    public class PersonRegistrationApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PersonRegistrationApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PersonRegistrationApplicationModule>(validate: true);
            });

            //Data Geographical
            context.Services.AddSingleton<CountryData>();
        }
    }
}
