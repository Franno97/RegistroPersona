using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Geographical;
using Mre.Sb.Notificacion.HttpApi;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Mre.Sb.PersonRegistration.Settings;
using Mre.Sb.PersonRegistration.Util;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Tracing;
using Volo.Abp.Users;
using Volo.Abp.IdentityModel;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class FamiliarProcesoAppService : ApplicationService, IFamiliarProcesoAppService
    {
        private readonly IPersonaMdgAppService _personMdgAppService;
        private readonly IVerificationCodeAppService _verificationCodeAppService;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;
        private readonly IPersonaAppService _personService;
        private readonly RegistroPersonaReglas _personRegistrationRules;
        private readonly IRegistroTrackingAppService _registerTrackingAppService;
        private readonly ITipoDocumentoIdentidadAppService _identityDocumentTypeAppService;
        private readonly IEstadoCivilAppService _maritalStatusService;
        private readonly ITipoVisaAppService _visaTypeService;
        private readonly INivelEducativoAppService _educationLevelService;
        private readonly IProfesionAppService _professionService;
        private readonly IOcupacionAppService _occupationService;
        private readonly INotificadorClient _notificadorClient;
        private readonly IIdentityModelAuthenticationService identityModelAuthenticationService;
        private readonly IPersonaMapeadorAppService _personaMapeadorAppService;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly AbpCorrelationIdOptions _abpCorrelationIdOptions;
        
        private readonly ILogger<RegistroPersonaProcesoAppService> _logger;
        private readonly CountryData _countryData;
        private readonly ICurrentUser _currentUser;

        public AbpIdentityClientOptions ClientOptions { get; }

        public FamiliarProcesoAppService(
            IPersonaMdgAppService personMdgAppService,
            IVerificationCodeAppService verificationCodeAppService,
            IPersonaAppService personService,
            RegistroPersonaReglas personRegistrationRules,
            IStringLocalizer<PersonRegistrationResource> localizer,
            IRegistroTrackingAppService registerTrackingAppService,
            ITipoDocumentoIdentidadAppService identityDocumentTypeAppService,
            IEstadoCivilAppService maritalStatusService,
            ITipoVisaAppService visaTypeService,
            INivelEducativoAppService educationLevelService,
            IProfesionAppService professionService,
            IOcupacionAppService occupationService,
            INotificadorClient notificadorClient,
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IOptions<AbpIdentityClientOptions> abpIdentityClientOptions,
            IPersonaMapeadorAppService personaMapeadorAppService,
            ICorrelationIdProvider correlationIdProvider,
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions,
            CountryData countryData,
            ICurrentUser currentUser,
            ILogger<RegistroPersonaProcesoAppService> logger)

        {
            _personMdgAppService = personMdgAppService;
            _verificationCodeAppService = verificationCodeAppService;
            _personService = personService;
            _personRegistrationRules = personRegistrationRules;
            _localizer = localizer;
            _registerTrackingAppService = registerTrackingAppService;
            _identityDocumentTypeAppService = identityDocumentTypeAppService;
            _maritalStatusService = maritalStatusService;
            _visaTypeService = visaTypeService;
            _educationLevelService = educationLevelService;
            _professionService = professionService;
            _occupationService = occupationService;
            _notificadorClient = notificadorClient;
            this.identityModelAuthenticationService = identityModelAuthenticationService;
            ClientOptions = abpIdentityClientOptions.Value;
            _personaMapeadorAppService = personaMapeadorAppService;
            this._correlationIdProvider = correlationIdProvider;
            this._abpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            _logger = logger;
            _countryData = countryData;
            _currentUser = currentUser;
        }

        public async Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input)
        {
            var result = new RegistroPersonaOutput();

            try
            {
                _logger.LogInformation("RegistroPersona - Obtener informacion de personaMdg");
                var respuestaPersonaMdg = await _personMdgAppService.ObtenerPorNumeroRegistroAsync(input.NumeroRegistro);

                if(!respuestaPersonaMdg.Success)
                {
                    _logger.LogError(respuestaPersonaMdg.Error);
                    result.Success = respuestaPersonaMdg.Success;
                    result.Error = respuestaPersonaMdg.Error;
                    return result;
                }

                var personMdg = respuestaPersonaMdg.PersonaMdgDto;

                // Validar fecha de nacimiento
                _logger.LogInformation("RegistroPersona - Iniciar la validacion de la fecha de nacimiento");
                var respuestaFechaNacimiento = await VerificarFechaNacimiento(personMdg, input.FechaNacimiento);
                if (!respuestaFechaNacimiento.Success)
                {
                    return respuestaFechaNacimiento;
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
                result.Success = false;
            }

            return result;
        }

        public async Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro)
        {
            var result = new RegistroPersonaOutput();

            try
            {
                var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);

                //1. Generar código de verificación
                _logger.LogInformation("RegistroPersona - Generar codigo de verificacion");
                
                var codigoVerificacion = CodigoVerificacionGenerador.GenerateVerificationCode();

                var response = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                //2. Almacenar codigo de verificacion en la base de datos
                //2.1 Obtener la duracion del codigo desde la configuracion
                var codeDurationTime =
                    await SettingProvider.GetOrNullAsync(PersonRegistrationSettings.CodigoVerificacion.Duracion);

                //2.2 Generar el objeto para persistir
                var actualDate = DateTime.Now;
                var createUpdateVerificationCodeDto = new CrearActualizarCodigoVerificacionDto
                {
                    Codigo = codigoVerificacion,
                    FechaGeneracion = actualDate,
                    FechaExpiracion = actualDate.AddMinutes(Convert.ToInt32(codeDurationTime)),
                    PersonaMdgNumeroRegistro = personMdg.NumeroRegistro,
                    PersonaMdgCorreoElectronico = response.CorreoElectronico
                };

                var verificationCodeDto = await _verificationCodeAppService.GetByRegisterNumber(numeroRegistro);

                if (verificationCodeDto == null)
                {
                    await _verificationCodeAppService.CreateAsync(createUpdateVerificationCodeDto);
                }
                else
                {
                    createUpdateVerificationCodeDto.Id = verificationCodeDto.Id;
                    await _verificationCodeAppService.UpdateAsync(createUpdateVerificationCodeDto);
                }

                var personMdgInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                //3. Enviar codigo de verificacion al correo electronico del ciudadano
                _logger.LogInformation("RegistroPersona - Obtener token para envio de correo electronico");

                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


                _notificadorClient.SetAccessToken(token);
                _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

                //El codigo de verificacion se envia al correo electronico de la persona que hace la solicitud
                var personaNombre = _currentUser.Name;
                var personaApellido = _currentUser.SurName;
                var personaCorreo = _currentUser.Email;

                var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroCodigoVerificacion(personaNombre, personaApellido, personaCorreo, codigoVerificacion);

                _logger.LogInformation("RegistroPersona - Enviar codigo de verificacion al correo electronico");
                var notificado = await _notificadorClient.NotificadorAsync(notificacionDto);

                _logger.LogInformation("RegistroPersona - Finalizado enviar codigo de verificacion al correo electronico");

                if (!notificado)
                {
                    result.Error = string.Format(_localizer["PersonRegistration:VerificationCodeFailSend"]);
                    return result;
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
                result.Success = false;
            }

            return result;
        }

        public async Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input)
        {
            _logger.LogInformation("RegistroPersona - Iniciar validacion de codigo de verificacion");

            var result = new RegistroPersonaOutput();
            try
            {
                var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(input.NumeroRegistro);

                if (personMdg == null)
                {
                    result.Error = string.Format(_localizer["PersonRegistration:PersonNotExist"]);
                    return result;
                }

                //2. Validar el codigo ingresado por el usuario
                var verificationCode =
                    await _verificationCodeAppService.GetVerificationCodeByRegisterNumber(input.NumeroRegistro);

                if (verificationCode.Codigo != input.CodigoVerificacion)
                {
                    result.Error = string.Format(_localizer["PersonRegistration:VerificationCodeNotCorrect"]);
                    return result;
                }

                //issues/113
                var resultCompare =
                    DateTime.Compare(DateTime.Now, verificationCode.FechaExpiracion);

                //Codigo de verifiacion caducado
                if (resultCompare > 0)
                {
                    await _verificationCodeAppService.ChangeStateAsync(input.CodigoVerificacion, input.NumeroRegistro,
                        EstadoCodigoVerificacion.Caducado);

                    result.Error = string.Format(_localizer["PersonRegistration:VerificationCodeExpired"]);
                    return result;
                }

                await _verificationCodeAppService.ChangeStateAsync(input.CodigoVerificacion, input.NumeroRegistro,
                    EstadoCodigoVerificacion.Procesado);

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
                result.Success = false;
            }

            _logger.LogInformation("RegistroPersona - Finalizar validacion de codigo de verificacion");

            return result;
        }

        public async Task<PersonaDto> ObtenerInformacionPersonaAsync(string numeroRegistro)
        {
            var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
            if (personMdg == null)
            {
                throw new UserFriendlyException(string.Format(_localizer["PersonRegistration:VerificationNumberFail"]));
            }

            var personInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

            var personaDto = await _personaMapeadorAppService.MapeoPersonaDesdePersonaMdgAsync(personInformation);

            return personaDto;
        }

        public async Task<PersonaDto> GuardarPersonaAsync(string numeroRegistro)
        {
            _logger.LogInformation("RegistroPersona - Iniciar registro de persona");

            var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
            if (personMdg == null)
            {
                throw new UserFriendlyException(string.Format(_localizer["PersonRegistration:VerificationNumberFail"]));
            }

            await _personMdgAppService.ChangeStateAsync(numeroRegistro, EstadoPersonaMdg.Procesado);

            var personInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

            var entityCreateDto = await _personaMapeadorAppService.MapeoPersonaAsync(personInformation);

            //No establecer nombre de usuario puesto que no se va a crear un usuario en el sistema para esta persona
            entityCreateDto.NombreUsuario = "";

            //Crear persona
            var result= await _personService.CreateAsync(entityCreateDto);

            _logger.LogInformation("RegistroPersona - Finalizar registro de persona");

            return result;
 
        }


        private async Task<RegistroPersonaOutput> VerificarFechaNacimiento(PersonaMdgDto personMdg, DateTime fechaNacimiento)
        {
            var result = new RegistroPersonaOutput();

            var birthDateIsEqual = DateTime.Compare(personMdg.FechaNacimiento.Date, fechaNacimiento.Date);
            if (birthDateIsEqual != 0)
            {
                result.Error = string.Format(_localizer["PersonRegistration:BirthDateNotCorrect"]);
                result.Success = false;
                return result;
            }

            result.Success = true;
            return result;
        }


        private IdentityClientConfiguration GetClientConfiguration(string identityClientName = null)
        {
            if (identityClientName.IsNullOrEmpty())
            {
                return ClientOptions.IdentityClients.Default;
            }

            return ClientOptions.IdentityClients.GetOrDefault(identityClientName) ??
                   ClientOptions.IdentityClients.Default;
        }
    }
}