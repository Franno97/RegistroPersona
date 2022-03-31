using System;
using System.ComponentModel.DataAnnotations;
using Mre.Sb.RegistroPersona.Persona;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public class CrearActualizarPersonaMdgDto
    {
        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public string NumeroRegistro { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxNameLength)]

        public string PaisId { get; set; }

        public string Respuesta { get; set; }
    }
}
