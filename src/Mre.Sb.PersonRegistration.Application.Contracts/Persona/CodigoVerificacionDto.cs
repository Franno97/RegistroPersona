using System;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class CodigoVerificacionDto : IEntityDto<int>
    {
        public int Id { get; set; }

        public virtual string Codigo { get; set; }

        public virtual DateTime FechaGeneracion { get; set; }

        public virtual DateTime FechaExpiracion { get; set; }

        public virtual string PersonaMdgNumeroRegistro { get; set; }

        public virtual string PersonaMdgCorreoElectronico { get; set; }

        public virtual EstadoCodigoVerificacion Estado { get; set; }
    }
}
