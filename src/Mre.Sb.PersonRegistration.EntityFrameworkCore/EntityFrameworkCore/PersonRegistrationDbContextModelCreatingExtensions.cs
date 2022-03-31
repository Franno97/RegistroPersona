using System;
using Microsoft.EntityFrameworkCore;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    public static class PersonRegistrationDbContextModelCreatingExtensions
    {
        public static void ConfigurePersonRegistration(
            this ModelBuilder builder,
            Action<PersonRegistrationModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PersonRegistrationModelBuilderConfigurationOptions(
                PersonRegistrationDbProperties.DbTablePrefix,
                PersonRegistrationDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<NivelEducativo>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "NivelEducativo", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<TipoDocumentoIdentidad>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "TipoDocumentoIdentidad", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<DocumentoIdentidad>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "DocumentoIdentidad", options.Schema);

                b.ConfigureByConvention();

                b.HasOne(u => u.TipoDocumentoIdentidad).WithMany().HasForeignKey(ur => ur.TipoDocumentoIdentidadId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasKey(u => new { u.OrigenId });

            });

            builder.Entity<EstadoCivil>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "EstadoCivil", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<Ocupacion>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Ocupacion", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<Profesion>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Profesion", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<CodigoVerificacion>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "CodigoVerificacion", options.Schema);

                b.ConfigureByConvention();

                b.HasIndex(p => new { p.PersonaMdgNumeroRegistro });
            });

            builder.Entity<TipoVisa>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "TipoVisa", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<Visa>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Visa", options.Schema);

                b.ConfigureByConvention();

                b.HasOne(u => u.TipoVisa).WithMany().HasForeignKey(ur => ur.TipoVisaId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasKey(u => new { u.OrigenId });
            });

            builder.Entity<PersonaMdg.PersonaMdg>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "PersonaMdg", options.Schema);

                b.ConfigureByConvention();

                b.HasIndex(p => new { p.NumeroRegistro });

            });

            builder.Entity<PersonaNacionalidad>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "PersonaNacionalidad", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(pn => new { pn.PersonaId, pn.PaisId});

                b.HasIndex(pn => new { pn.PersonaId, pn.PaisId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Persona.Persona>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Persona", options.Schema);

                b.ConfigureByConvention();

                b.HasMany(u => u.Nacionalidades).WithOne().HasForeignKey(ur => ur.PersonaId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.EstadoCivil).WithMany().HasForeignKey(ur => ur.EstadoCivilId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.NivelEducativo).WithMany().HasForeignKey(ur => ur.NivelEducativoId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.Profesion).WithMany().HasForeignKey(ur => ur.ProfesionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.Ocupacion).WithMany().HasForeignKey(ur => ur.OcupacionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<RegistroTracking>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "RegistroTracking", options.Schema);

                b.ConfigureByConvention();

            });

        }
    }
}