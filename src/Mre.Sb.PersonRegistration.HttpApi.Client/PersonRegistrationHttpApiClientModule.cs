using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(PersonRegistrationApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class PersonRegistrationHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "PersonRegistration";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PersonRegistrationApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
