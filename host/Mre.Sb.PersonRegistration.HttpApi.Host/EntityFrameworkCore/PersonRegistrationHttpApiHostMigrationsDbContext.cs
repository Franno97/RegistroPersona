using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    public class PersonRegistrationHttpApiHostMigrationsDbContext : AbpDbContext<PersonRegistrationHttpApiHostMigrationsDbContext>
    {
        public PersonRegistrationHttpApiHostMigrationsDbContext(DbContextOptions<PersonRegistrationHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersonRegistration();
        }
    }
}
