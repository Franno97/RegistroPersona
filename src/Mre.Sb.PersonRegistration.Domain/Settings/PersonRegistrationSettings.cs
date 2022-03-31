namespace Mre.Sb.PersonRegistration.Settings
{
    public static class PersonRegistrationSettings
    {
        public const string ModuleName = "RegistroPersona";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */


        public static class FaseRegulacion
        {
            public const string GroupName = ModuleName + ".FaseRegulacion";
            public const string PuntoControl = GroupName + ".PuntoControl";
            public const string Nacionalidad = GroupName + ".Nacionalidad";
            public const string NacionalidadesPermitidas = GroupName + ".NacionalidadesPermitidas";
            public const string MayoriaEdad = GroupName + ".MayorEdad";
            public const string EdadMinima = GroupName + ".EdadMinima";
            public const string CaptchaHabilitado = GroupName + ".CaptchaHabilitado";
            public const string FechaControlInicial = GroupName + ".FechaControlInicial";
            public const string FechaControlFinal = GroupName + ".FechaControlFinal";
            public const string VigenciaInformacion = GroupName + ".VigenciaInformacion";
        }

        public static class CodigoVerificacion
        {
            public const string GroupName = ModuleName + ".CodigoVerificacion";
            public const string Duracion = GroupName + ".Duracion";
            public const string IntentosPermitiddos = GroupName + ".IntentosPermitiddos";
            public const string DuracionBloqueo = GroupName + ".DuracionBloqueo";
        }

        public static class Catalogo
        {
            public const string GroupName = ModuleName + ".Catalogo";
            public const string EstadoCivilDefault = GroupName + ".EstadoCivilDefault";
            public const string NivelEducativoDefault = GroupName + ".NivelEducativoDefault";
            public const string ProfesionDefault = GroupName + ".ProfesionDefault";
            public const string OcupacionDefault = GroupName + ".OcupacionDefault";
            public const string TipoDocumentoDefault = GroupName + ".TipoDocumentoDefault";
            public const string TipoVisaDefault = GroupName + ".TipoVisaDefault";
        }
    }
}