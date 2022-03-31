using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Mre.Sb.PersonRegistration.Permissions;
using Mre.Sb.PersonRegistration.Settings;
using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonaConfiguracionAppService : ApplicationService, IPersonaConfiguracionAppService
    {
        protected ISettingManager SettingManager { get; }

        public PersonaConfiguracionAppService(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        public async Task<PersonaConfiguracionDto> ObtenerAsync()
        {


            var personaConfiguracion = new PersonaConfiguracionDto
            {
                ValidarPuntoDeAcceso = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.PuntoControl)),
                ValidarNacionalidad = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.Nacionalidad)),
                NacionalidadesPermitidas = await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.NacionalidadesPermitidas),
                ValidarMayoriaEdad = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.MayoriaEdad)),
                EdadMinima = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.EdadMinima)),
                HabilitarCaptcha = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.CaptchaHabilitado)),
                FechaInicialControl = Convert.ToDateTime(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlInicial)),
                FechaFinalControl = Convert.ToDateTime(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlFinal)),
                VigenciaInformacion = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.FaseRegulacion.VigenciaInformacion)),
                DuracionCodigoVerificacion = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.Duracion)),
                IntentosPermitidos = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.IntentosPermitiddos)),
                DuracionBloqueo = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.DuracionBloqueo))
            };

            return await Task.FromResult(personaConfiguracion);
        }

        [Authorize(RegistroPersonaPermisos.PersonaConfiguracion.Update)]
        public async Task ActualizarAsync(ActualizarPersonaConfiguracionDto input)
        {
            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.PuntoControl, input.ValidarPuntoDeAcceso.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.Nacionalidad, input.ValidarNacionalidad.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.NacionalidadesPermitidas, input.NacionalidadesPermitidas);

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.MayoriaEdad, input.ValidarMayoriaEdad.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.EdadMinima, input.EdadMinima.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.CaptchaHabilitado, input.HabilitarCaptcha.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlInicial, input.FechaInicialControl.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlFinal, input.FechaFinalControl.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.FaseRegulacion.VigenciaInformacion, input.VigenciaInformacion.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.Duracion, input.DuracionCodigoVerificacion.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.IntentosPermitiddos, input.IntentosPermitidos.ToString());

            await SettingManager.SetGlobalAsync(PersonRegistrationSettings.CodigoVerificacion.DuracionBloqueo, input.DuracionBloqueo.ToString());
        }

    }
}
