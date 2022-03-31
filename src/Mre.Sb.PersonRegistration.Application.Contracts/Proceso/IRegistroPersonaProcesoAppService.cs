using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public interface IRegistroPersonaProcesoAppService : IApplicationService
    {
        
        /// <summary>
        /// Realizar validaciones de información de la persona
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input);

        /// <summary>
        /// Generar y enviar codigo de verificacion
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro);

        /// <summary>
        /// Validar codigo de verificacíon
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input);

        /// <summary>
        /// Obtener los datos de la persona
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> ObtenerInformacionPersonaAsync(string numeroRegistro);

        /// <summary>
        /// Registrar la persona
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<RegistroPersonaOutput> RegistrarPersonaAsync(string numeroRegistro);


        /// <summary>
        /// Gestionar el rechazo de registro del ciudadano
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        Task<bool> RechazarRegistroAsync(string numeroRegistro);

    }


    public class ChequeoPrevioInput
    {
        [Required]
        [MaxLength(10)]
        public string NumeroRegistro { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }
    }

    public class RegistroPersonaOutput
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public PersonaDto PersonaDto { get; set; }
    }

    public class EnviarCodigoVerificacionInput
    {
        [Required]
        public string NumeroRegistro { get; set; }

        public string PersonaMdgEmail { get; set; }
    }

    public class ValidarCodigoVerificacionInput
    {
        [Required]
        public string NumeroRegistro { get; set; }
        [Required]
        public string CodigoVerificacion { get; set; }
    }

    public class RegistroPersonaInput
    {
        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string FirtsSurname { get; set; }

        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string SecondSurname { get; set; }

        public virtual DateTime BirthDate { get; set; }

        [Required]
        public virtual string BirthCountryId { get; set; }

        [Required]
        public string Nationalities { get; protected set; }

        [Required]
        public virtual string EmailAddress { get; set; }

        public virtual bool HasIdentityDocument { get; set; }

        public virtual string IdentityDocumentId { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxGenderLength)]
        public virtual string Gender { get; set; }

        [Required]
        public virtual string MaritalStatusId { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxAddressLength)]
        public virtual string Address { get; set; }

        [Required]
        public virtual string RegionId { get; set; }

        [Required]
        public virtual string City { get; set; }

        [Required]
        public virtual string EducationLevelId { get; set; }

        [Required]
        public virtual string ProfessionId { get; set; }

        [Required]
        public virtual string OccupationId { get; set; }

        [Required]
        public virtual byte[] Photograph { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxPermanenceRecordNumberLength)]
        public virtual string PermanenceRecordNumber { get; set; }

        public virtual byte[] FingerPrint { get; set; }

        public virtual DateTime CountryEntryDate { get; set; }

        public virtual bool RegularPointAccess { get; set; }

        [Required]
        [StringLength(PersonConsts.MaxLastResidenceCountryLength)]
        public virtual string LastResidenceCountry { get; set; }
    }


    public class PersonaMdgOutput
    {
        public PersonaMdgDto PersonaMdgDto { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }
    }

}
