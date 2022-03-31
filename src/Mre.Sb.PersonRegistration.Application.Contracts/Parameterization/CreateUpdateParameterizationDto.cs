using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.PersonRegistration.Parameterization
{
    public class CreateUpdateParameterizationDto
    {
        [Required]
        public virtual bool IsActive { get; set; }

        [Required]
        public virtual bool IsAccessPointValidationEnabled { get; set; }

        [Required]
        public virtual bool IsNationalityValidationEnabled { get; set; }

        /// <summary>
        /// Comma separated allowed nationalities
        /// </summary>
        public virtual string AllowedNationalities { get; set; }

        [Required]
        public virtual bool IsLegalAgeValidationEnabled { get; set; }

        [Required]
        public virtual int MinimumAllowedAge { get; set; }

        /// <summary>
        /// Duration for verification code in minutes
        /// </summary>
        public virtual int CodeDuration { get; set; }

        /// <summary>
        /// Maximum allowed attempts for verification code before Lockut
        /// </summary>
        public virtual int AllowedAttempts { get; set; }
    }
}
