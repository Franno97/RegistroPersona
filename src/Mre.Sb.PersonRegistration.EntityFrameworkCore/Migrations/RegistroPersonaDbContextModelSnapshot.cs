﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mre.Sb.RegistroPersona.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.PersonRegistration.Migrations
{
    [DbContext(typeof(RegistroPersonaDbContext))]
    partial class RegistroPersonaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.CodigoVerificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<int>("Estado")
                        .HasMaxLength(80)
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaExpiracion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaGeneracion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("PersonaMdgCorreoElectronico")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("PersonaMdgNumeroRegistro")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("PersonaMdgNumeroRegistro");

                    b.ToTable("CodigoVerificacion");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.DocumentoIdentidad", b =>
                {
                    b.Property<string>("OrigenId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("FechaEmision")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaExpiracion")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumeroDocumento")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PaisEmisionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDocumentoIdentidadId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("OrigenId");

                    b.HasIndex("TipoDocumentoIdentidadId");

                    b.ToTable("DocumentoIdentidad");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.EstadoCivil", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("EstadoCivil");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.NivelEducativo", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("NivelEducativo");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Ocupacion", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Ocupacion");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Persona", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorreoElectronico")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DeleterId");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DeletionTime");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("DocumentoIdentidadId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EstadoCivilId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.Property<DateTime>("FechaIngresoPais")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Fotografia")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Genero")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<byte[]>("HuellaDactilar")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("NivelEducativoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("NombreUsuario")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NumeroRegistroPermanencia")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("OcupacionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Origen")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("OrigenId")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("PaisNacimientoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaisResidenciaPrevia")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("ProfesionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.Property<bool>("PuntoAccesoRegular")
                        .HasColumnType("bit");

                    b.Property<string>("RegionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SegundoApellido")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<bool>("TieneDocumentoIdentidad")
                        .HasColumnType("bit");

                    b.Property<string>("VisaId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoCivilId");

                    b.HasIndex("NivelEducativoId");

                    b.HasIndex("OcupacionId");

                    b.HasIndex("ProfesionId");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.PersonaNacionalidad", b =>
                {
                    b.Property<Guid>("PersonaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PaisId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PersonaId", "PaisId");

                    b.HasIndex("PersonaId", "PaisId");

                    b.ToTable("PersonaNacionalidad");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Profesion", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Profesion");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.TipoDocumentoIdentidad", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("TipoDocumentoIdentidad");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.TipoVisa", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CodigoMapeo")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("TipoVisa");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Visa", b =>
                {
                    b.Property<string>("OrigenId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("FechaEmision")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaExpiracion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Numero")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("TipoVisaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("OrigenId");

                    b.HasIndex("TipoVisaId");

                    b.ToTable("Visa");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.PersonaMdg.PersonaMdg", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("BloqueoHabilitado")
                        .HasColumnType("bit");

                    b.Property<int>("ContadorAccesoFallido")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<int>("Estado")
                        .HasMaxLength(80)
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FinBloqueo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("NumeroRegistro")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PaisId")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Respuesta")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NumeroRegistro");

                    b.ToTable("PersonaMdg");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.PersonaMdg.RegistroTracking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Evento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaEvento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mensaje")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroRegistro")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ResultadoValidacion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RegistroTracking");
                });

            modelBuilder.Entity("Volo.Abp.SettingManagement.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("Nombre");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("ProveedorClave");

                    b.Property<string>("ProviderName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("Proveedor");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)")
                        .HasColumnName("Valor");

                    b.HasKey("Id");

                    b.HasIndex("Name", "ProviderName", "ProviderKey")
                        .IsUnique()
                        .HasFilter("[Proveedor] IS NOT NULL AND [ProveedorClave] IS NOT NULL");

                    b.ToTable("Configuracion");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.DocumentoIdentidad", b =>
                {
                    b.HasOne("Mre.Sb.RegistroPersona.Persona.TipoDocumentoIdentidad", "TipoDocumentoIdentidad")
                        .WithMany()
                        .HasForeignKey("TipoDocumentoIdentidadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TipoDocumentoIdentidad");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Persona", b =>
                {
                    b.HasOne("Mre.Sb.RegistroPersona.Persona.EstadoCivil", "EstadoCivil")
                        .WithMany()
                        .HasForeignKey("EstadoCivilId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mre.Sb.RegistroPersona.Persona.NivelEducativo", "NivelEducativo")
                        .WithMany()
                        .HasForeignKey("NivelEducativoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mre.Sb.RegistroPersona.Persona.Ocupacion", "Ocupacion")
                        .WithMany()
                        .HasForeignKey("OcupacionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mre.Sb.RegistroPersona.Persona.Profesion", "Profesion")
                        .WithMany()
                        .HasForeignKey("ProfesionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EstadoCivil");

                    b.Navigation("NivelEducativo");

                    b.Navigation("Ocupacion");

                    b.Navigation("Profesion");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.PersonaNacionalidad", b =>
                {
                    b.HasOne("Mre.Sb.RegistroPersona.Persona.Persona", null)
                        .WithMany("Nacionalidades")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Visa", b =>
                {
                    b.HasOne("Mre.Sb.RegistroPersona.Persona.TipoVisa", "TipoVisa")
                        .WithMany()
                        .HasForeignKey("TipoVisaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TipoVisa");
                });

            modelBuilder.Entity("Mre.Sb.RegistroPersona.Persona.Persona", b =>
                {
                    b.Navigation("Nacionalidades");
                });
#pragma warning restore 612, 618
        }
    }
}
