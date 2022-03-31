using System;
using System.ComponentModel.DataAnnotations;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class CrearActualizarCodigoVerificacionDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(VerificationCodeConsts.MaxCodeLength)]
        public string Codigo { get; set; }

        [Required]
        public DateTime FechaGeneracion { get; set; }

        [Required]
        public DateTime FechaExpiracion { get; set; }

        [Required]
        [StringLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public string PersonaMdgNumeroRegistro { get; set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxEmailLength)]
        public string PersonaMdgCorreoElectronico { get; set; }
    }
}
