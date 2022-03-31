using Volo.Abp.Application.Dtos;

namespace Mre.Sb.PersonRegistration.Parameterization
{
    public class ParameterizationDto : IEntityDto<int>
    {
        public int Id { get; set ; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsAccessPointValidationEnabled { get; set; }

        public virtual bool IsNationalityValidationEnabled { get; set; }

        public virtual string AllowedNationalities { get; set; }

        public virtual bool IsLegalAgeValidationEnabled { get; set; }

        public virtual int MinimumAllowedAge { get; set; }

        public virtual int CodeDuration { get; set; }

        public virtual int AllowedAttempts { get; set; }
    }
}
