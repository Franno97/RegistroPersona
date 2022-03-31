using System;
using System.ComponentModel.DataAnnotations;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp.Domain.Entities.Auditing;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    /// <summary>
    /// Clase for information recovered from government ministry's Persona Service 
    /// </summary>
    public class PersonaMdg : AuditedEntity<Guid>
    {
        protected PersonaMdg()
        {

        }

        public PersonaMdg(Guid id, string numeroRegistro, DateTime fechaNacimiento, string nacionalidad, string respuesta, EstadoPersonaMdg estado)
        {
            Id = id;
            NumeroRegistro = numeroRegistro;
            FechaNacimiento = fechaNacimiento;
            PaisId = nacionalidad;
            Estado = estado;
            Respuesta = respuesta;
            BloqueoHabilitado = false;
        }

        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public virtual string NumeroRegistro { get; set; }

        [Required]
        public virtual DateTime FechaNacimiento { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual string PaisId { get; set; }

        public virtual string Respuesta { get; set; }

        [StringLength(DomainCommonConsts.MaxNameLength)]
        public virtual EstadoPersonaMdg Estado { get; set; }

        /// <summary>
        /// Campo para establecer la fecha de fin de bloqueo
        /// </summary>
        public virtual DateTime FinBloqueo { get; set; }

        /// <summary>
        /// Campo para establecer si la persona esta o no bloqueada
        /// true: esta bloqueado
        /// false: no esta bloqueado
        /// </summary>
        public virtual bool BloqueoHabilitado { get; set; }

        /// <summary>
        /// Campo para establecer el numero de intentos fallidos de validacion del codigo de verificacion
        /// </summary>
        public virtual int ContadorAccesoFallido { get; set; }

        public void IncrementarAccesoFallido()
        {
            ContadorAccesoFallido = ContadorAccesoFallido + 1;
        }

        public void BloquearUsuario(double lockDuration)
        {
            var currentDate = DateTime.Now;
            FinBloqueo = currentDate.AddMinutes(lockDuration);
            BloqueoHabilitado = true;
        }

        public void DesbloquearUsuario()
        {
            BloqueoHabilitado = false;
            ContadorAccesoFallido = 0;
        }

        public void ResetearAccesoFallido()
        {
            ContadorAccesoFallido = 0;
        }

    }
}
