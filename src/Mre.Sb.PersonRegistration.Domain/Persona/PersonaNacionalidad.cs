using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonaNacionalidad : Entity
    {
        public PersonaNacionalidad(Guid personaId, string paisId)
        {
            PersonaId = personaId;
            PaisId = paisId;
        }

        [Required]
        public Guid PersonaId { get; set; }

        [Required]
        public string PaisId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { PersonaId, PaisId };
        }
    }
}
