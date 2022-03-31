using Mre.Sb.Notificacion.HttpApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class NotificacionMapeador
    {
        public static NotificacionMensajeDto MapeoPersonaRegistroValidacion(string asunto, InformacionPersonaDto personMdgInformation, string mensaje)
        {
            var nombreCompleto = string.IsNullOrEmpty(personMdgInformation.SegundoApellido) ? $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido}" : $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido} {personMdgInformation.SegundoApellido}";
            var salida = new NotificacionMensajeDto();

            salida.Codigo = RegistroPersonaConsts.Notificaciones.PersonaRegistroValidacion;
            salida.Asunto = asunto;
            salida.Destinatarios = personMdgInformation.CorreoElectronico;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("PersonaNombreApellido", nombreCompleto);
            salida.Model.Add("DescripcionError", mensaje);

            return salida;
        }

        public static NotificacionMensajeDto MapeoPersonaRegistroCodigoVerificacion(string asunto, InformacionPersonaDto personMdgInformation, string codigoVerificacion)
        {
            var nombreCompleto = string.IsNullOrEmpty(personMdgInformation.SegundoApellido) ? $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido}" : $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido} {personMdgInformation.SegundoApellido}";
            var salida = new NotificacionMensajeDto();

            salida.Codigo = RegistroPersonaConsts.Notificaciones.PersonaRegistroCodigoVerificacion;
            salida.Asunto = asunto;
            salida.Destinatarios = personMdgInformation.CorreoElectronico;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("PersonaNombreApellido", nombreCompleto);
            salida.Model.Add("CodigoVerificacion", codigoVerificacion);

            return salida;
        }

        public static NotificacionMensajeDto MapeoPersonaRegistroCodigoVerificacion(string personaNombre, string personaApellido, string correoElectronico, string codigoVerificacion)
        {
            var nombreCompleto = $"{personaNombre} {personaApellido}";
            var salida = new NotificacionMensajeDto();

            salida.Codigo = RegistroPersonaConsts.Notificaciones.PersonaRegistroCodigoVerificacion;
            salida.Asunto = "Codigo verificacion";
            salida.Destinatarios = correoElectronico;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("PersonaNombreApellido", nombreCompleto);
            salida.Model.Add("CodigoVerificacion", codigoVerificacion);

            return salida;
        }

        public static NotificacionMensajeDto MapeoPersonaRegistroRechazo(InformacionPersonaDto personMdgInformation, string mensaje)
        {
            var nombreCompleto = string.IsNullOrEmpty(personMdgInformation.SegundoApellido) ? $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido}" : $"{personMdgInformation.Nombres} {personMdgInformation.PrimerApellido} {personMdgInformation.SegundoApellido}";
            var salida = new NotificacionMensajeDto();

            salida.Codigo = RegistroPersonaConsts.Notificaciones.NotificacionGeneral01;
            salida.Asunto = "Notificación registro rechazado";
            salida.Destinatarios = personMdgInformation.CorreoElectronico;
            salida.Model = new Dictionary<string, object>();
            salida.Model.Add("PersonaNombreApellido", nombreCompleto);
            salida.Model.Add("Cuerpo", mensaje);

            return salida;
        }
    }
}
