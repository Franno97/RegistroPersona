using System;
using System.Collections.Generic;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class ConsultarFlujoMigratorioRespuesta
    {
        public ICollection<FlujoMigratorioMovimientoDto> FlujosMigratoriosMovimientosDto { get; set; }

        public bool Correcto { get; set; }

        public int Codigo { get; set; }

        public string Detalle { get; set; }

        public string Error { get; set; }
    }

    public class FlujoMigratorioMovimientoDto
    {

        public virtual string ApellidosNombres { get; set; }

        public virtual string CategoriaMigratoria { get; set; }

        public virtual int CodigoError { get; set; }

        public virtual string FechaHoraMovimiento { get; set; }

        public virtual string FechaNacimiento { get; set; }

        public virtual string Genero { get; set; }

        public virtual string Medio { get; set; }

        public virtual string MotivoViaje { get; set; }

        public virtual string NacionalidadDocumentoMovimientoMigratorio { get; set; }

        public virtual string NumeroDocumentoMovimientoMigratorio { get; set; }

        public virtual string PaisDestino { get; set; }

        public virtual string PaisNacimiento { get; set; }

        public virtual string PaisOrigen { get; set; }

        public virtual string PaisResidencia { get; set; }

        public virtual string PuertoRegistro { get; set; }

        public virtual string TarjetaAndina { get; set; }

        public virtual string TiempoDeclarado { get; set; }

        public virtual string TipoDocumentoMovimientoMigratorio { get; set; }

        public virtual string TipoMovimiento { get; set; }

    }

}
