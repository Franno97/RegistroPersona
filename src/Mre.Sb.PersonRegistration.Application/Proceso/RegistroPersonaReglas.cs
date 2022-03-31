using Microsoft.Extensions.Configuration;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Mre.Sb.PersonRegistration.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Settings;
using Microsoft.Extensions.Logging;

namespace Mre.Sb.RegistroPersona.Proceso
{
    /// <summary>
    /// Servicio para validar las reglas de negocio para el registro de ciudadano
    /// </summary>
    public class RegistroPersonaReglas : ApplicationService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IPersonaMdgAppService _personMdgAppService;
        private readonly IClientExternalDmzAppService _clientExternalDmzAppService;
        private readonly IConfiguration _config;
        private readonly ILogger<RegistroPersonaReglas> _logger;

        public RegistroPersonaReglas(ISettingProvider settingProvider,
            IPersonaMdgAppService personMdgAppService,
            IClientExternalDmzAppService clientExternalDmzAppService,
            IConfiguration config,
            ILogger<RegistroPersonaReglas> logger)
        {
            _settingProvider = settingProvider;
            _personMdgAppService = personMdgAppService;
            _clientExternalDmzAppService = clientExternalDmzAppService;
            _config = config;
            _logger = logger;
        }
        public async Task<bool> ValidarPuntoAccesoAsync(string registerNumber)
        {
            //1. Obtener la configuracion de punto de control
            var accessPoint = await _settingProvider.GetAsync<bool>(PersonRegistrationSettings.FaseRegulacion.PuntoControl);
            //2. Si es true, validar que la persona ingreso por punto de control
            if (accessPoint)
            {
                //3. Consultar al servicio de punto de control usando el numero de registro migratorio
                var accessPointEntry = await IngresoPorPuntoRegular(registerNumber);
                if (!accessPointEntry)
                {
                    return accessPointEntry;
                }
            }

            return true;
        }

        public async Task<bool> ValidarNacionalidadAsync(string nationality)
        {
            //1. Obtener configuracion de nacionalidad
            var validarNacionalidad= await _settingProvider.GetAsync<bool>(PersonRegistrationSettings.FaseRegulacion.Nacionalidad);

            //2. Si es true, validar la nacionalidad del ciudadano segun las admitidas
            if(validarNacionalidad)
            {
                var nationalities = await _settingProvider.GetOrNullAsync(PersonRegistrationSettings.FaseRegulacion.NacionalidadesPermitidas);

                var isAllowed = nationalities.Split(',').Any(n => string.Equals(n, nationality, StringComparison.OrdinalIgnoreCase));

                return isAllowed;
            }

            return true;
        }

        public async Task<bool> ValidarMayoriaEdadAsync(DateTime birthDate)
        {
            //1. Obtener configuracion de edad
            var validarEdad = await _settingProvider.GetAsync<bool>(PersonRegistrationSettings.FaseRegulacion.MayoriaEdad);

            //2. Si es true, validar la edad del ciudadano segun la edad minima
            if (validarEdad)
            {
                var legalAge = await _settingProvider.GetOrNullAsync(PersonRegistrationSettings.FaseRegulacion.EdadMinima);

                var age = CalcularEdad(birthDate);

                return age >= Convert.ToInt32(legalAge);
            }

            return true;
        }

        private int CalcularEdad(DateTime birthDate)
        {
            int age = 0;
            age = DateTime.Now.Year - birthDate.Year;

            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age = age - 1;

            return age;
        }

        private async Task<bool> IngresoPorPuntoRegular(string registerNumber)
        {
            _logger.LogInformation("RegistroPersonaReglas - Consultar servicio MDG para validar punto de control");

            ConsultarPuntoControlOutput resultado = new ConsultarPuntoControlOutput();
            resultado = await _clientExternalDmzAppService.ConsultarPuntoControlAsync(registerNumber);

            _logger.LogInformation("RegistroPersonaReglas - Finalizado consultar servicio MDG para validar punto de control");

            return resultado.PuntoIngresoRegular;

        }

    }
}
