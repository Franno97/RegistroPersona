namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonMdgConsts
    {
        /// <summary>
        /// Maximum length of the RegisterNumber property.
        /// </summary>
        public const int MaxRegisterNumberLength = 10;

        /// <summary>
        /// Maximum length of the Response property.
        /// </summary>
        public const int MaxResponseLength = 2000;


        public const string SERVICIO_MDG = "MDG";

    }

    public class VerificationCodeConsts
    {
        /// <summary>
        /// Maximum length of the Code property.
        /// </summary>
        public const int MaxCodeLength = 6;


    }

    public class RespuestaMdgConsts
    {
        public const string SI = "SI";
        public const string NO = "NO";
    }

    public class RegistroTrackingConsts
    {
        public const string EVENTO_VERIFICACION_PREVIA = "VERIFICACION_PREVIA";
        public const string EVENTO_ENVIAR_CODIGO_VERIFICACION = "ENVIAR_CODIGO_VERIFICACION";
        public const string EVENTO_VALIDAR_CODIGO_VERIFICACION = "VALIDAR_CODIGO_VERIFICACION";
        public const string EVENTO_OBTENER_INFORMACION_PERSONA = "OBTENER_INFORMACION_PERSONA";
        public const string EVENTO_REGISTRAR_PERSONA = "REGISTRAR_PERSONA";
        public const string EVENTO_CREAR_USUARIO = "CREAR_USUARIO";
        public const string EVENTO_RECHAZAR_REGISTRO = "RECHAZAR_REGISTRO";
        public const string EVENTO_NOTIFICACION = "NOTIFICACION";
        public const string EVENTO_ERROR = "ERROR";
        public const string EVENTO_INTENTO_FALLIDO_FECHA_NACIMIENTO = "INTENTO_FALLIDO_FECHA_NACIMIENTO";
        public const string EVENTO_INTENTO_FALLIDO_CODIGO_VERIFICACION = "INTENTO_FALLIDO_CODIGO_VERIFICACION";
        public const string EVENTO_CODIGO_VERIFICACION_CADUCADO = "CODIGO_VERIFICACION_CADUCADO";
        public const string EVENTO_USUARIO_BLOQUEADO = "USUARIO_BLOQUEADO";
    }

}
