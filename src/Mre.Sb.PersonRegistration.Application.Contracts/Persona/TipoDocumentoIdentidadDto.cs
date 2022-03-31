using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class TipoDocumentoIdentidadDto : IEntityDto<string>
    {
        [Required]
        [StringLength(EducationLevelConsts.MaxIdLength)]
        public string Id { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public string Nombre { get; set; }

        [StringLength(DomainCommonConsts.MaxCodigoMapeoLength)]
        public virtual string CodigoMapeo { get; set; }

    }
}
