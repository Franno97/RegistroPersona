using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class CrearActualizarPersonaDto
    {
        public CrearActualizarPersonaDto()
        {
            Nacionalidades = new List<string>();
        }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string PrimerApellido { get; set; }

        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string SegundoApellido { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string PaisNacimientoId { get; set; }

        [Required]
        public List<string> Nacionalidades { get; set; }

        [Required]
        public string CorreoElectronico { get; set; }

        public bool TieneDocumentoIdentidad { get; set; }

        public string TipoDocumentoIdentidadId { get; set; }

        [MaxLength(IdentityDocumentConsts.MaxDocumentNumberLength)]
        public string NumeroDocumentoIdentidad { get; set; }

        public string PaisEmisionDocumentoIdentidad { get; set; }

        public DateTime? FechaEmisionDocumentoIdentidad { get; set; }

        public DateTime? FechaExpiracionDocumentoIdentidad { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxGenderLength)]
        public string Genero { get; set; }

        [Required]
        public string EstadoCivilId { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPhoneNumberLength)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxAddressLength)]
        public string Direccion { get; set; }

        [Required]
        public string RegionId { get; set; }

        [Required]
        public string Ciudad { get; set; }

        public string NumeroVisa { get; set; }

        public string TipoVisaId { get; set; }

        public DateTime? FechaEmisionVisa { get; set; }

        public DateTime? FechaExpiracionVisa { get; set; }

        [Required]
        public string NivelEducativoId { get; set; }

        [Required]
        public string ProfesionId { get; set; }

        [Required]
        public string OcupacionId { get; set; }

        [Required]
        public byte[] Fotografia { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPermanenceRecordNumberLength)]
        public string NumeroRegistroPermanencia { get; set; }

        [Required]
        public byte[] HuellaDactilar { get; set; }

        [Required]
        public DateTime FechaIngresoPais { get; set; }

        public bool PuntoAccesoRegular { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxLastResidenceCountryLength)]
        public string PaisResidenciaPrevia { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxOriginLength)]
        public virtual string Origen { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxOriginIdLength)]
        public virtual string OrigenId { get; set; }

        [StringLength(PersonConsts.MaxUserNameLength)]
        public virtual string NombreUsuario { get; set; }
    }
}
