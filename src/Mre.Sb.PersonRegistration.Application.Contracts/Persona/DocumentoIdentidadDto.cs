using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class DocumentoIdentidadDto : IEntityDto
    {

        [Required]
        public virtual string TipoDocumentoIdentidadId { get; set; }

        public virtual string TipoDocumentoIdentidad { get; set; }

        [StringLength(IdentityDocumentConsts.MaxDocumentNumberLength)]
        public virtual string NumeroDocumento { get; set; }

        public virtual string PaisEmisionId { get; set; }

        public virtual DateTime? FechaEmision { get; set; }

        public virtual DateTime? FechaExpiracion { get; set; }

        [Required]
        public virtual string OrigenId { get; set; }
    }
}
