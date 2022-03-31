using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonaDto : IEntityDto<Guid>
    {
        public PersonaDto()
        {
            NacionalidadesId = new List<string>();
            NacionalidadesNombre = new List<string>();
        }

        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string PaisNacimientoId { get; set; }

        public string PaisNacimiento { get; set; }

        public ICollection<string> NacionalidadesId { get; set; }

        public ICollection<string> NacionalidadesNombre { get; set; }

        public string CorreoElectronico { get; set; }

        public bool PoseeDocumentoIdentidad { get; set; }

        public string PoseeDocumentoIdentidadTexto { get; set; }

        public string TipoDocumentoIdentidadId { get; set; }

        public string TipoDocumentoIdentidad { get; set; }

        public string NumeroDocumentoIdentidad { get; set; }

        public string DocumentoIdentidadPaisEmision { get; set; }

        public string DocumentoIdentidadPaisEmisionNombre { get; set; }

        public DateTime? DocumentoIdentidadFechaEmision { get; set; }

        public DateTime? DocumentoIdentidadFechaExpiracion { get; set; }

        public string Genero { get; set; }

        public string EstadoCivilId { get; set; }

        public string EstadoCivil { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string RegionId { get; set; }

        public string Region { get; set; }

        public string Ciudad { get; set; }


        public string NumeroVisa { get; set; }

        public string TipoVisa { get; set; }

        public string TipoVisaId { get; set; }

        public DateTime? VisaFechaEmision { get; set; }

        public DateTime? VisaFechaExpiracion { get; set; }


        public string NivelEducativoId { get; set; }

        public string NivelEducativo { get; set; }


        public string ProfesionId { get; set; }

        public string Profesion { get; set; }


        public string OcupacionId { get; set; }

        public string Ocupacion { get; set; }


        public byte[] Fotografia { get; set; }

        public string FotografiaBase64 { get; set; }


        public string NumeroRegistroPermanencia { get; set; }


        public byte[] HuellasDactilares { get; set; }

        public string HuellasDactilaresBase64 { get; set; }


        public DateTime? FechaIngresoPais { get; set; }

        public bool IngresoPuntoRegular { get; set; }

        public string IngresoPuntoRegularTexto { get; set; }

        public string PaisResidenciaPrevia { get; set; }

        public string PaisResidenciaPreviaTexto { get; set; }

        public string NombreUsuario { get; set; }


        public virtual string Origen { get; set; }

        public virtual string OrigenId { get; set; }

    }
}
