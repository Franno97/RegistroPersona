using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class CodigoVerificacion : AuditedEntity<int>
    {
        protected CodigoVerificacion()
        {

        }

        public CodigoVerificacion(string codigo, DateTime fechaGeneracion, DateTime fechaExpiracion, string personaMdgNumeroRegistro, string personaMdgCorreoElectronico, EstadoCodigoVerificacion estado)
        {
            Codigo = codigo;
            FechaGeneracion = fechaGeneracion;
            FechaExpiracion = fechaExpiracion;
            PersonaMdgNumeroRegistro = personaMdgNumeroRegistro;
            PersonaMdgCorreoElectronico = personaMdgCorreoElectronico;
            Estado = estado;
        }

        [Required]
        [StringLength(VerificationCodeConsts.MaxCodeLength)]
        public virtual string Codigo { get; set; }

        [Required]
        public virtual DateTime FechaGeneracion { get; set; }

        [Required]
        public virtual DateTime FechaExpiracion { get; set; }

        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public virtual string PersonaMdgNumeroRegistro { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxEmailLength)]
        public virtual string PersonaMdgCorreoElectronico { get; set; }

        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual EstadoCodigoVerificacion Estado { get; set; }


        public void CambiarEstado(EstadoCodigoVerificacion estado)
        {
            Estado = estado;
        }
    }
}
