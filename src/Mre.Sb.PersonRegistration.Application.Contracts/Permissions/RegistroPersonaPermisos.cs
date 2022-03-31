using Volo.Abp.Reflection;

namespace Mre.Sb.PersonRegistration.Permissions
{
    public class RegistroPersonaPermisos
    {
        public const string GroupName = "RegistroPersona";

        public static class PersonaConfiguracion
        {
            public const string Default = GroupName + ".PersonaConfiguracion";
            public const string Update = Default + ".Update";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(RegistroPersonaPermisos));
        }
    }
}