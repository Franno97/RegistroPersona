using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(PersonRegistrationHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PersonRegistrationConsoleApiClientModule : AbpModule
    {
        
    }
}
