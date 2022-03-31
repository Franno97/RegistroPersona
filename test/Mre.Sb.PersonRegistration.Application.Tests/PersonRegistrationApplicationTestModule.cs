using Volo.Abp.Modularity;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(PersonRegistrationApplicationModule),
        typeof(PersonRegistrationDomainTestModule)
        )]
    public class PersonRegistrationApplicationTestModule : AbpModule
    {

    }
}
