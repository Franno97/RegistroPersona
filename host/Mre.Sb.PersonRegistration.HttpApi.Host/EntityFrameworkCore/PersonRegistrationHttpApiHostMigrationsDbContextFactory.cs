using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    public class PersonRegistrationHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<PersonRegistrationHttpApiHostMigrationsDbContext>
    {
        public PersonRegistrationHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PersonRegistrationHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("PersonRegistration"));

            return new PersonRegistrationHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
