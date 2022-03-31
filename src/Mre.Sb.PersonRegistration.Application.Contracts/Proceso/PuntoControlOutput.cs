using System;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class ConsultarPuntoControlOutput
    {
        public ConsultarPuntoControlOutput()
        {
            PuntoIngresoRegular = false;
            Correcto = false;
        }

        /// <summary>
        /// Indica si el ciudadano ingreso por punto regular.
        /// True si ingreso por punto regular, Falso en caso contrario
        /// </summary>
        public bool PuntoIngresoRegular { get; set; }
        
        /// <summary>
        /// Indica si el resultado fue correcto
        /// True si es correcto, False si existio error
        /// </summary>
        public bool Correcto { get; set; }
        

    }

}
