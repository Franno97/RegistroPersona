using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class DocumentoIdentidad : Entity
    {
        public DocumentoIdentidad(string tipoDocumentoIdentidadId,
            string numeroDocumento,
            string paisEmisionId,
            DateTime? fechaEmision,
            DateTime? fechaExpiracion,
            string origenId
            )
        {
            TipoDocumentoIdentidadId = tipoDocumentoIdentidadId;
            NumeroDocumento = numeroDocumento;
            PaisEmisionId = paisEmisionId;
            FechaEmision = fechaEmision;
            FechaExpiracion = fechaExpiracion;
            OrigenId = origenId;
        }


        [Required]
        public virtual string TipoDocumentoIdentidadId { get; set; }

        public virtual TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }

        /// <summary>
        /// Numero de documento de viaje
        /// </summary>
        [StringLength(IdentityDocumentConsts.MaxDocumentNumberLength)]
        public virtual string NumeroDocumento { get; set; }

        public virtual string PaisEmisionId { get; set; }

        public virtual DateTime? FechaEmision { get; set; }

        public virtual DateTime? FechaExpiracion { get; set; }

        /// <summary>
        /// Se establece como OrigenId el numero de registro del MDG
        /// </summary>
        [Required]
        public virtual string OrigenId { get; set; }


        public override object[] GetKeys()
        {
            return new object[] { OrigenId };
        }
    }
}
