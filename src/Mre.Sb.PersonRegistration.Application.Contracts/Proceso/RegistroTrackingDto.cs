using System;
using System.ComponentModel.DataAnnotations;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class RegistroTrackingDto : IEntityDto<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public virtual string NumeroRegistro { get; set; }

        [Required]
        public virtual string Evento { get; set; }

        public virtual string ResultadoValidacion { get; set; }

        public virtual string Mensaje { get; set; }

        [StringLength(PersonMdgConsts.MaxResponseLength)]
        public virtual DateTime FechaEvento { get; set; }
    }
}
