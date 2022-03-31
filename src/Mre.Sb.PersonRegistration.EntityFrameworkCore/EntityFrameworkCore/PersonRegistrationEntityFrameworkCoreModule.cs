using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    [DependsOn(
        typeof(PersonRegistrationDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
    )]
    public class PersonRegistrationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RegistroPersonaDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });

            //Sin prefijos las tablas, de los modulos abp
            AbpSettingManagementDbProperties.DbTablePrefix = string.Empty;
            
        }
    }
}