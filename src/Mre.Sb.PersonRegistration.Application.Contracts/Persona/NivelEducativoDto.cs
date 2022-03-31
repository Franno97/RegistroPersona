using Mre.Sb.RegistroPersona.Persona;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class NivelEducativoDto : IEntityDto<string>
    {
        [Required]
        [StringLength(EducationLevelConsts.MaxIdLength)]
        public string Id { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string Nombre { get; set; }

        [StringLength(DomainCommonConsts.MaxCodigoMapeoLength)]
        public virtual string CodigoMapeo { get; set; }

    }
}
