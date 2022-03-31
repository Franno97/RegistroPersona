using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class TipoDocumentoIdentidad : Entity<string>
    {

        [Required]
        [StringLength(IdentityDocumentTypeConsts.MaxIdLength)]
        public override string Id { get; protected set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string Nombre { get; set; }

        [StringLength(DomainCommonConsts.MaxCodigoMapeoLength)]
        public virtual string CodigoMapeo { get; set; }

    }
}
