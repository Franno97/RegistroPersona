using System;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class InformacionPersonaDto
    {
        public InformacionPersonaDto()
        {
            
        }

        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string PaisNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string OtraNacionalidad { get; set; }
        public string CorreoElectronico { get; set; }
        
        public string PoseeDocumentoIdentidad { get; set; }
        public string TipoDocumentoIdentidad { get; set; }
        public string NumeroDocumentoViaje { get; set; }
        public string PaisEmisionDocumentoIdentidad { get; set; }
        public DateTime FechaEmisionDocumentoIdentidad { get; set; }
        public DateTime FechaExpiracionDocumentoIdentidad { get; set; }

        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        
        public string NumeroVisa { get; set; }
        public string TipoVisa { get; set; }
        public DateTime? FechaEmisionVisa { get; set; }
        public DateTime? FechaExpiracionVisa { get; set; }

        public string NivelEducativo { get; set; }
        public string Profesion { get; set; }
        public string Ocupacion { get; set; }
        public string NumeroRegistroPermanencia { get; set; }

        public byte[] Fotografia { get; set; }
        public byte[] HuellasDactilares { get; set; }
        
        public DateTime FechaIngresoPais { get; set; }
        public string IngresoPorPuntoRegular { get; set; }
        public string PaisResidenciaPrevia { get; set; }

        public string CodigoRegistro { get; set; }
        
    }
}
