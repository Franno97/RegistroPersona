using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class TipoVisa : Entity<string>
    {
        protected TipoVisa()
        {

        }

        public TipoVisa(string id) : base(id)
        {

        }


        [Required]
        [StringLength(VisaTypeConsts.MaxIdLength)]
        public override string Id { get; protected set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string Nombre { get; set; }

        [StringLength(DomainCommonConsts.MaxCodigoMapeoLength)]
        public virtual string CodigoMapeo { get; set; }

    }
}
