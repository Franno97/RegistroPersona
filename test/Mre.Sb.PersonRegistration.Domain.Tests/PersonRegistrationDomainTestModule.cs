using Mre.Sb.RegistroPersona.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Mre.Sb.RegistroPersona
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(PersonRegistrationEntityFrameworkCoreTestModule)
        )]
    public class PersonRegistrationDomainTestModule : AbpModule
    {
        
    }
}
