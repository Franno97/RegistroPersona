using System;
using System.ComponentModel.DataAnnotations;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    /// <summary>
    /// Clase para registrar informacion de validaciones aplicadas en el proceso de registro de un ciudadano
    /// </summary>
    public class RegistroTracking : Entity<int>
    {
        public RegistroTracking()
        {

        }

        public RegistroTracking(string registerNumber, string eventParameter, string validationResult,
            string message, DateTime eventDate)
        {
            NumeroRegistro = registerNumber;
            Evento = eventParameter;
            ResultadoValidacion = validationResult;
            Mensaje = message;
            FechaEvento = eventDate;
        }

        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public virtual string NumeroRegistro { get; set; }

        [Required]
        public virtual string Evento { get; set; }

        public virtual string ResultadoValidacion { get; set; }

        public virtual string Mensaje { get; set; }

        public virtual DateTime FechaEvento { get; set; }
    }
}
