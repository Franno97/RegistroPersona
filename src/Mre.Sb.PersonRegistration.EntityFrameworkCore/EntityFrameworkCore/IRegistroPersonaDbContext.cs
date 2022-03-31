using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    [ConnectionStringName(PersonRegistrationDbProperties.ConnectionStringName)]
    public interface IRegistroPersonaDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}