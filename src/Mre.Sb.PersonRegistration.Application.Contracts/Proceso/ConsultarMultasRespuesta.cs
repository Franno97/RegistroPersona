using Mre.Sb.RegistroPersona.Persona;
using System;
using System.Collections.Generic;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class ConsultarMultasRespuesta
    {
        public MultaDto MultaDto { get; set; }

        public bool Correcto { get; set; }

        public int Codigo { get; set; }

        public string Detalle { get; set; }

        public string Error { get; set; }
    }

    public class MultaDto
    {

        public virtual string ApellidosNombres { get; set; }

        public virtual string FechaNacimiento { get; set; }

        public virtual string Genero { get; set; }

        public virtual string PaisNacimiento { get; set; }

        public virtual string PaisResidencia { get; set; }

        public virtual ICollection<MultaFlujoMigratorio> Multas { get; set; }

        public virtual ICollection<NotificacionSalidaVoluntaria> NotificacionesSalidas { get; set; }

    }

    public class MultaFlujoMigratorio
    {
        public virtual int CodigoError { get; set; }

        public virtual string Estado { get; set; }

        public virtual string FechaRegistro { get; set; }

        public virtual string TipoMulta { get; set; }

    }

    public class NotificacionSalidaVoluntaria
    {
        public virtual int CodigoError { get; set; }

        public virtual string Estado { get; set; }

        public virtual string FechaRegistro { get; set; }

    }
}
