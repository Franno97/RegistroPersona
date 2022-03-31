using System;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public class PersonaMdgDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }

        public string NumeroRegistro { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string PaisId { get; set; }

        public string Respuesta { get; set; }

        public EstadoPersonaMdg Estado { get; set; }

        public DateTime FinBloqueo { get; set; }

        public bool BloqueoHabilitado { get; set; }

        public int ContadorAccesoFallido { get; set; }      
    }
}
