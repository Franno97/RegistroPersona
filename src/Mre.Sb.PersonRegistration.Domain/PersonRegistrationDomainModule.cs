using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.RegistroPersona
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(PersonRegistrationDomainSharedModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class PersonRegistrationDomainModule : AbpModule
    {

    }
}
