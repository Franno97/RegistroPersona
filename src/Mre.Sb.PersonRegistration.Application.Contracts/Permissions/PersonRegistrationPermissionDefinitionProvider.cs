using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mre.Sb.PersonRegistration.Permissions
{
    public class PersonRegistrationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(RegistroPersonaPermisos.GroupName, L("Permiso:RegistroPersona"));

            var registroPersonaConfPermission = moduleGroup.AddPermission(RegistroPersonaPermisos.PersonaConfiguracion.Default, L("Permiso:AdministracionRegistroPersonaConfig"));
            registroPersonaConfPermission.AddChild(RegistroPersonaPermisos.PersonaConfiguracion.Update, L("Permiso:Editar"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PersonRegistrationResource>(name);
        }
    }
}