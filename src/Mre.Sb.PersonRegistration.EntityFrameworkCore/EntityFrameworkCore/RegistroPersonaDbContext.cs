using Microsoft.EntityFrameworkCore;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(ISettingManagementDbContext))]
    [ConnectionStringName(PersonRegistrationDbProperties.ConnectionStringName)]
    public class RegistroPersonaDbContext : AbpDbContext<RegistroPersonaDbContext>
        , IRegistroPersonaDbContext
        , ISettingManagementDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<NivelEducativo> NivelEducativo { get; set; }
        
        public DbSet<DocumentoIdentidad> DocumentoIdentidad  { get; set; }
        
        public DbSet<TipoDocumentoIdentidad> TipoDocumentoIdentidad  { get; set; }
        
        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        
        public DbSet<Ocupacion> Ocupacion  { get; set; }
        
        public DbSet<Profesion> Profesion { get; set; }
        
        public DbSet<Visa> Visa { get; set; }

        public DbSet<TipoVisa> TipoVisa { get; set; }
        
        public DbSet<Persona.Persona> Persona { get; set; }
        
        public DbSet<PersonaMdg.PersonaMdg> PersonaMdg { get; set; }

        public DbSet<PersonaNacionalidad> PersonaNacionalidad { get; set; }

        public DbSet<CodigoVerificacion> CodigoVerificacion { get; set; }
        
        public DbSet<RegistroTracking> RegistroTracking { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public RegistroPersonaDbContext(DbContextOptions<RegistroPersonaDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePersonRegistration();

            builder.ConfigurarGestionConfiguracion();
        }
    }

    public static class GestionConfiguracionDbContextModelBuilderExtensions
    {
        public static void ConfigurarGestionConfiguracion(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<Setting>(b =>
            {
                b.ToTable(AbpSettingManagementDbProperties.DbTablePrefix + "Configuracion", AbpSettingManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasColumnName("Nombre").HasMaxLength(SettingConsts.MaxNameLength).IsRequired();

                if (builder.IsUsingOracle()) { SettingConsts.MaxValueLengthValue = 2000; }
                b.Property(x => x.Value).HasColumnName("Valor").HasMaxLength(SettingConsts.MaxValueLengthValue).IsRequired();

                b.Property(x => x.ProviderName).HasColumnName("Proveedor").HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasColumnName("ProveedorClave").HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<SettingManagementDbContext>();
        }
    }
}