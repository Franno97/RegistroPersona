using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class Visa : Entity
    {
        public Visa(string numero,
            string tipoVisaId,
            DateTime? fechaEmision,
            DateTime? fechaExpiracion,
            string origenId
            )
        {
            Numero = numero;
            TipoVisaId = tipoVisaId;
            FechaEmision = fechaEmision;
            FechaExpiracion = fechaExpiracion;
            OrigenId = origenId;
        }


        /// <summary>
        /// Numero documento de viaje
        /// </summary>
        [StringLength(VisaConsts.MaxVisaNumberLength)]
        public virtual string Numero { get; set; }

        [Required]
        public virtual string TipoVisaId { get; set; }

        public virtual TipoVisa TipoVisa { get; set; }

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
