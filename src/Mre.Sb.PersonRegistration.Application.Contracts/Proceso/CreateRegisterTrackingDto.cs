using System;
using System.ComponentModel.DataAnnotations;
using Mre.Sb.RegistroPersona.Persona;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class CreateRegisterTrackingDto
    {
        public CreateRegisterTrackingDto()
        {
            EventDate = DateTime.Now;
        }

        [Required]
        [MaxLength(PersonMdgConsts.MaxRegisterNumberLength)]
        public  string RegisterNumber { get; set; }

        [Required]
        public  string Event { get; set; }

        public  string ValidationResult { get; set; }

        public  string Message { get; set; }

        public  DateTime EventDate { get; set; }

    }
}
