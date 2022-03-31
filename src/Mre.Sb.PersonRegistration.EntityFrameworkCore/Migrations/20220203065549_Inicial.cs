using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mre.Sb.PersonRegistration.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodigoVerificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonaMdgNumeroRegistro = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PersonaMdgCorreoElectronico = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Estado = table.Column<int>(type: "int", maxLength: 80, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoVerificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ProveedorClave = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCivil",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCivil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NivelEducativo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelEducativo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ocupacion",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocupacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonaMdg",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroRegistro = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaisId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", maxLength: 80, nullable: false),
                    FinBloqueo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BloqueoHabilitado = table.Column<bool>(type: "bit", nullable: false),
                    ContadorAccesoFallido = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaMdg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesion",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistroTracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroRegistro = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Evento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultadoValidacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroTracking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumentoIdentidad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentoIdentidad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoVisa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodigoMapeo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoVisa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SegundoApellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaisNacimientoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TieneDocumentoIdentidad = table.Column<bool>(type: "bit", nullable: false),
                    DocumentoIdentidadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EstadoCivilId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    RegionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisaId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NivelEducativoId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    ProfesionId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    OcupacionId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    Fotografia = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    NumeroRegistroPermanencia = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    HuellaDactilar = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FechaIngresoPais = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PuntoAccesoRegular = table.Column<bool>(type: "bit", nullable: false),
                    PaisResidenciaPrevia = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Origen = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    OrigenId = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_EstadoCivil_EstadoCivilId",
                        column: x => x.EstadoCivilId,
                        principalTable: "EstadoCivil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persona_NivelEducativo_NivelEducativoId",
                        column: x => x.NivelEducativoId,
                        principalTable: "NivelEducativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persona_Ocupacion_OcupacionId",
                        column: x => x.OcupacionId,
                        principalTable: "Ocupacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persona_Profesion_ProfesionId",
                        column: x => x.ProfesionId,
                        principalTable: "Profesion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoIdentidad",
                columns: table => new
                {
                    OrigenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoDocumentoIdentidadId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaisEmisionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoIdentidad", x => x.OrigenId);
                    table.ForeignKey(
                        name: "FK_DocumentoIdentidad_TipoDocumentoIdentidad_TipoDocumentoIdentidadId",
                        column: x => x.TipoDocumentoIdentidadId,
                        principalTable: "TipoDocumentoIdentidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visa",
                columns: table => new
                {
                    OrigenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TipoVisaId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visa", x => x.OrigenId);
                    table.ForeignKey(
                        name: "FK_Visa_TipoVisa_TipoVisaId",
                        column: x => x.TipoVisaId,
                        principalTable: "TipoVisa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonaNacionalidad",
                columns: table => new
                {
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaisId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaNacionalidad", x => new { x.PersonaId, x.PaisId });
                    table.ForeignKey(
                        name: "FK_PersonaNacionalidad_Persona_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodigoVerificacion_PersonaMdgNumeroRegistro",
                table: "CodigoVerificacion",
                column: "PersonaMdgNumeroRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Configuracion_Nombre_Proveedor_ProveedorClave",
                table: "Configuracion",
                columns: new[] { "Nombre", "Proveedor", "ProveedorClave" },
                unique: true,
                filter: "[Proveedor] IS NOT NULL AND [ProveedorClave] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoIdentidad_TipoDocumentoIdentidadId",
                table: "DocumentoIdentidad",
                column: "TipoDocumentoIdentidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_EstadoCivilId",
                table: "Persona",
                column: "EstadoCivilId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_NivelEducativoId",
                table: "Persona",
                column: "NivelEducativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_OcupacionId",
                table: "Persona",
                column: "OcupacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_ProfesionId",
                table: "Persona",
                column: "ProfesionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaMdg_NumeroRegistro",
                table: "PersonaMdg",
                column: "NumeroRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaNacionalidad_PersonaId_PaisId",
                table: "PersonaNacionalidad",
                columns: new[] { "PersonaId", "PaisId" });

            migrationBuilder.CreateIndex(
                name: "IX_Visa_TipoVisaId",
                table: "Visa",
                column: "TipoVisaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigoVerificacion");

            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "DocumentoIdentidad");

            migrationBuilder.DropTable(
                name: "PersonaMdg");

            migrationBuilder.DropTable(
                name: "PersonaNacionalidad");

            migrationBuilder.DropTable(
                name: "RegistroTracking");

            migrationBuilder.DropTable(
                name: "Visa");

            migrationBuilder.DropTable(
                name: "TipoDocumentoIdentidad");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "TipoVisa");

            migrationBuilder.DropTable(
                name: "EstadoCivil");

            migrationBuilder.DropTable(
                name: "NivelEducativo");

            migrationBuilder.DropTable(
                name: "Ocupacion");

            migrationBuilder.DropTable(
                name: "Profesion");
        }
    }
}
