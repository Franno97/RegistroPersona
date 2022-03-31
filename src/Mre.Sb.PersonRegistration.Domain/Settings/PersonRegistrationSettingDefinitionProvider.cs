using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Mre.Sb.PersonRegistration.Settings
{
    public class PersonRegistrationSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            /* Define module settings here.
             * Use names from PersonRegistrationSettings class.
             */
            context.Add(
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.PuntoControl, true.ToString(), L("RegulationPhase:AccessPoint"), L("RegulationPhase:AccessPointDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.Nacionalidad, true.ToString(), L("RegulationPhase:Nationality"), L("RegulationPhase:NationalityDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.NacionalidadesPermitidas, "VEN,COL,ESP", L("RegulationPhase:NationalitiesAllowed"), L("RegulationPhase:NationalitiesAllowedDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.MayoriaEdad, true.ToString(), L("RegulationPhase:LegalAge"), L("RegulationPhase:LegalAgeDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.EdadMinima, 18.ToString(), L("RegulationPhase:MinimumAge"), L("RegulationPhase:MinimumAgeDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.CaptchaHabilitado, false.ToString(), L("RegulationPhase:CaptchaEnabled"), L("RegulationPhase:CaptchaEnabledDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.FechaControlInicial, "2013-06-10", L("RegulationPhase:InitialControlDate"), L("RegulationPhase:InitialControlDateDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.FechaControlFinal, "2021-07-30", L("RegulationPhase:FinalControlDate"), L("RegulationPhase:FinalControlDateDescription")),
                new SettingDefinition(PersonRegistrationSettings.FaseRegulacion.VigenciaInformacion, "72", L("RegulationPhase:InformationValidity"), L("RegulationPhase:InformationValidityDescription")),
                new SettingDefinition(PersonRegistrationSettings.CodigoVerificacion.Duracion, "10", L("VerificationCode:Duration"), L("VerificationCode:DurationDescription")),
                new SettingDefinition(PersonRegistrationSettings.CodigoVerificacion.IntentosPermitiddos, "3", L("VerificationCode:AttemptsAllowed"), L("VerificationCode:AttemptsAllowedDescription")),
                new SettingDefinition(PersonRegistrationSettings.CodigoVerificacion.DuracionBloqueo, "30", L("VerificationCode:LockDuration"), L("VerificationCode:LockDurationDescription")),
                new SettingDefinition(PersonRegistrationSettings.CodigoVerificacion.DuracionBloqueo, "30", L("VerificationCode:LockDuration"), L("VerificationCode:LockDurationDescription")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.EstadoCivilDefault, "00", L("Catalogo:EstadoCivilDefault"), L("Catalogo:EstadoCivilDefaultDescripcion")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.NivelEducativoDefault, "00", L("Catalogo:NivelEducativoDefault"), L("Catalogo:NivelEducativoDefaultDescripcion")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.ProfesionDefault, "00", L("Catalogo:ProfesionDefault"), L("Catalogo:ProfesionDefaultDescripcion")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.OcupacionDefault, "00", L("Catalogo:OcupacionDefault"), L("Catalogo:OcupacionDefaultDescripcion")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.TipoDocumentoDefault, "00", L("Catalogo:TipoDocumentoDefault"), L("Catalogo:TipoDocumentoDefaultDescripcion")),
                new SettingDefinition(PersonRegistrationSettings.Catalogo.TipoVisaDefault, "00", L("Catalogo:TipoVisaDefault"), L("Catalogo:TipoVisaDefaultDescripcion"))
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PersonRegistrationResource>(name);
        }
    }
}