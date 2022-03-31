using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class VisaDto : IEntityDto
    {

        public virtual string Numero { get; set; }

        public virtual string TipoVisaId { get; set; }

        public virtual string TipoVisa { get; set; }

        public virtual DateTime? FechaEmision { get; set; }

        public virtual DateTime? FechaExpiracion { get; set; }

        [Required]
        public virtual string OrigenId { get; set; }
    }
}
