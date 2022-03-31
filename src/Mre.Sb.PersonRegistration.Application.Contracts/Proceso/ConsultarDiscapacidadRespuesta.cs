using System;
using System.Collections.Generic;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class ConsultarDiscapacidadRespuesta
    {
        public int Codigo { get; set; }

        public string Cedula { get; set; }

        public string Apellidos { get; set; }

        public string Nombres { get; set; }

        public string CodigoConadis { get; set; }

        public string GradoDiscapacidad { get; set; }

        public string PorcentajeDiscapacidad { get; set; }

        public string TipoDiscapacidadPredomina { get; set; }

        public string Mensaje { get; set; }

    }

}
