using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class Persona : FullAuditedEntity<Guid>
    {
        protected Persona()
        {
        }

        public Persona(Guid id, string nombre, string primerApellido, string segundoApellido,
            DateTime fechaNacimiento, string paisNacimientoId, string correoElectronico,
            bool tieneDocumentoIdentidad, string documentoIdentidadId, string genero,
            string estadoCivilId, string telefono, string direccion,
            string regionId, string ciudad, string visaId,
            string nivelEducativoId, string profesionId, string ocupacionId,
            byte[] fotografia, string numeroRegistroPermanencia, byte[] huellaDactilar,
            DateTime fechaIngresoPais, bool puntoAccesoRegular, string paisResidenciaPrevia,
            string origen, string origenId, string nombreUsuario,
            List<string> nacionalidades)
        {
            Id = id;
            Nombre = nombre;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            FechaNacimiento = fechaNacimiento;
            PaisNacimientoId = paisNacimientoId;
            CorreoElectronico = correoElectronico;
            TieneDocumentoIdentidad = tieneDocumentoIdentidad;
            DocumentoIdentidadId = documentoIdentidadId;
            Genero = genero;
            EstadoCivilId = estadoCivilId;
            Telefono = telefono;
            Direccion = direccion;
            RegionId = regionId;
            Ciudad = ciudad;
            VisaId = visaId;
            NivelEducativoId = nivelEducativoId;
            ProfesionId = profesionId;
            OcupacionId = ocupacionId;
            Fotografia = fotografia;
            FechaIngresoPais = fechaIngresoPais;
            NumeroRegistroPermanencia = numeroRegistroPermanencia;
            HuellaDactilar = huellaDactilar;
            PuntoAccesoRegular = puntoAccesoRegular;
            PaisResidenciaPrevia = paisResidenciaPrevia;
            Origen = origen;
            OrigenId = origenId;
            NombreUsuario = nombreUsuario;
            Nacionalidades = new List<PersonaNacionalidad>();
            AgregarNacionalidades(nacionalidades);
        }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string Nombre { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string PrimerApellido { get; set; }

        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string SegundoApellido { get; set; }

        [Required] 
        public virtual DateTime FechaNacimiento { get; set; }

        [Required] 
        public virtual string PaisNacimientoId { get; set; }

        [Required] 
        public virtual ICollection<PersonaNacionalidad> Nacionalidades { get; protected set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxEmailLength)]
        public virtual string CorreoElectronico { get; set; }

        [Required] 
        public virtual bool TieneDocumentoIdentidad { get; set; }

        public virtual string DocumentoIdentidadId { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxGenderLength)]
        public virtual string Genero { get; set; }

        [Required] 
        public virtual string EstadoCivilId { get; set; }

        public virtual EstadoCivil EstadoCivil { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPhoneNumberLength)]
        public virtual string Telefono { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxAddressLength)]
        public virtual string Direccion { get; set; }

        [Required] 
        public virtual string RegionId { get; set; }


        [Required] 
        public virtual string Ciudad { get; set; }

        public virtual string VisaId { get; set; }


        [Required] 
        public virtual string NivelEducativoId { get; set; }

        public virtual NivelEducativo NivelEducativo { get; set; }

        [Required] 
        public virtual string ProfesionId { get; set; }

        public virtual Profesion Profesion { get; set; }

        [Required]
        public virtual string OcupacionId { get; set; }

        public virtual Ocupacion Ocupacion { get; set; }

        [Required]
        public virtual byte[] Fotografia { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPermanenceRecordNumberLength)]
        public virtual string NumeroRegistroPermanencia { get; set; }

        [Required] 
        public virtual byte[] HuellaDactilar { get; set; }

        /// <summary>
        /// Fecha ingreso al Ecuador
        /// </summary>
        [Required]
        public virtual DateTime FechaIngresoPais { get; set; }

        /// <summary>
        /// Persona ingreso al pais por punto regular
        /// </summary>
        [Required]
        public virtual bool PuntoAccesoRegular { get; set; }

        /// <summary>
        /// Pais residencia previa
        /// </summary>
        [Required]
        [StringLength(PersonConsts.MaxLastResidenceCountryLength)]
        public virtual string PaisResidenciaPrevia { get; set; }

        /// <summary>
        /// Fuente origen del registro de datos
        /// </summary>
        [Required]
        [StringLength(DomainCommonConsts.MaxOriginLength)]
        public virtual string Origen { get; set; }

        /// <summary>
        /// Identificador del registro de datos en el origen
        /// </summary>
        [Required]
        [StringLength(DomainCommonConsts.MaxOriginIdLength)]
        public virtual string OrigenId { get; set; }

        /// <summary>
        /// El nombre de usuario en caso que la persona tenga un usuario del sistema
        /// </summary>
        [StringLength(PersonConsts.MaxUserNameLength)]
        public virtual string NombreUsuario { get; set; }


        public void AgregarNacionalidades(string paisId)
        {
            var exist = Nacionalidades.Any(f => f.PaisId == paisId);

            if (exist)
            {
                //TODO: #issues/23
                throw new UserFriendlyException($"Country Exist {paisId}");
            }

            Nacionalidades.Add(new PersonaNacionalidad(Id, paisId));
        }

        public void AgregarNacionalidades(List<string> nacionalidades)
        {
            foreach (var paisId in nacionalidades)
            {
                Nacionalidades.Add(new PersonaNacionalidad(Id, paisId));
            }
        }
    }
}