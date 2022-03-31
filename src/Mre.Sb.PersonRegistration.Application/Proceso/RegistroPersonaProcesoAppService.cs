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
using Volo.Abp.IdentityModel;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class RegistroPersonaProcesoAppService : ApplicationService, IRegistroPersonaProcesoAppService
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

        public AbpIdentityClientOptions ClientOptions { get; }

        public RegistroPersonaProcesoAppService(
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
            IConfiguration config,
            CountryData countryData,
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
        }

        public async Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input)
        {
            var result = new RegistroPersonaOutput();

            try
            {
                _logger.LogInformation("RegistroPersona - Obtener informacion de personaMdg, numeroRegistro: {numeroRegistro}", input.NumeroRegistro);
                var respuestaPersonaMdg = await _personMdgAppService.ObtenerPorNumeroRegistroAsync(input.NumeroRegistro);

                if (!respuestaPersonaMdg.Success)
                {
                    _logger.LogError(respuestaPersonaMdg.Error);
                    result.Success = respuestaPersonaMdg.Success;
                    result.Error = respuestaPersonaMdg.Error;
                    return result;
                }

                //1. Validar si la persona existe y ya fue procesada
                if (respuestaPersonaMdg.PersonaMdgDto.Estado == EstadoPersonaMdg.Procesado)
                {
                    var mensaje = string.Format(_localizer["PersonRegistration:PersonExist"]);
                    _logger.LogError(mensaje);
                    result.Error = mensaje;
                    return result;
                }

                // Validar bloqueos
                _logger.LogInformation("RegistroPersona - Verificar si la persona tiene bloqueo por intentos fallidos, numeroRegistro: {numeroRegistro}", input.NumeroRegistro);
                var personMdg = respuestaPersonaMdg.PersonaMdgDto;

                var respuestaBloqueo = await VerificarBloqueoPersona(personMdg);
                if (!respuestaBloqueo.Success)
                {
                    return respuestaBloqueo;
                }

                // Validar fecha de nacimiento
                _logger.LogInformation("RegistroPersona - Iniciar la validacion de la fecha de nacimiento, numeroRegistro: {numeroRegistro}", input.NumeroRegistro);
                var respuestaFechaNacimiento = await VerificarFechaNacimiento(personMdg, input.FechaNacimiento);
                if (!respuestaFechaNacimiento.Success)
                {
                    //Registrar en tracking
                    var registerTrackingDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = input.NumeroRegistro,
                        Event = RegistroTrackingConsts.EVENTO_INTENTO_FALLIDO_FECHA_NACIMIENTO,
                        Message = $"{input.FechaNacimiento}, {respuestaFechaNacimiento.Error}"
                    };
                    await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                    _logger.LogError("RegistroPersona. Intento fallido de validar fecha nacimiento, numeroRegistro: {numeroRegistro}", input.NumeroRegistro);

                    return respuestaFechaNacimiento;
                }

                var personMdgInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                //3. Validar reglas segun configuracion
                _logger.LogInformation("RegistroPersona - Iniciar llamada a validacion de reglas, numeroRegistro: {numeroRegistro}", input.NumeroRegistro);
                var resultadoValidarReglas = await ValidateRules(personMdgInformation);
                if (!resultadoValidarReglas.Success)
                {
                    return resultadoValidarReglas;
                }

                //Restablecer contador
                await _personMdgAppService.UnlockUserAsync(personMdg.NumeroRegistro);

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $" Verificacion Previa. Numero Registro: {input.NumeroRegistro}");

                // Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = input.NumeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
            }

            _logger.LogInformation("RegistroPersona - Finalizar chequeo previo");

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = input.NumeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_VERIFICACION_PREVIA,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            return result;
        }

        public async Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro)
        {
            var result = new RegistroPersonaOutput();

            try
            {
                var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
                var personMdgInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                // Validar bloqueos
                _logger.LogInformation("RegistroPersona - Verificar si la persona tiene bloqueo por intentos fallidos");
                var respuestaBloqueo = await VerificarBloqueoPersona(personMdg);
                if (!respuestaBloqueo.Success)
                {
                    return respuestaBloqueo;
                }

                //1. Generar código de verificación
                _logger.LogInformation("RegistroPersona - Generar codigo de verificacion");

                var verificationCode = CodigoVerificacionGenerador.GenerateVerificationCode();

                //2. Almacenar codigo de verificacion en la base de datos
                //2.1 Obtener la duracion del codigo desde la configuracion
                var codeDurationTime =
                    await SettingProvider.GetOrNullAsync(PersonRegistrationSettings.CodigoVerificacion.Duracion);

                //2.2 Generar el objeto para persistir
                var actualDate = DateTime.Now;
                var createUpdateVerificationCodeDto = new CrearActualizarCodigoVerificacionDto
                {
                    Codigo = verificationCode,
                    FechaGeneracion = actualDate,
                    FechaExpiracion = actualDate.AddMinutes(Convert.ToInt32(codeDurationTime)),
                    PersonaMdgNumeroRegistro = personMdg.NumeroRegistro,
                    PersonaMdgCorreoElectronico = personMdgInformation.CorreoElectronico
                };

                var verificationCodeDto = await _verificationCodeAppService.GetByRegisterNumber(numeroRegistro);

                //2.3 Guardar codigo en base de datos
                if (verificationCodeDto == null)
                {
                    await _verificationCodeAppService.CreateAsync(createUpdateVerificationCodeDto);
                }
                else
                {
                    createUpdateVerificationCodeDto.Id = verificationCodeDto.Id;
                    await _verificationCodeAppService.UpdateAsync(createUpdateVerificationCodeDto);
                }

                //3. Enviar codigo de verificacion al correo electronico del ciudadano
                _logger.LogInformation("RegistroPersona - Obtener token para envio de correo electronico");


                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


                _notificadorClient.SetAccessToken(token);
                _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

                var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroCodigoVerificacion(_localizer["RegistroPersonaNotificacion:CodigoVerificacion"], personMdgInformation, verificationCode);

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
                _logger.LogError(ex, $"Enviar Codigo Verificacion. Numero Registro: {numeroRegistro}");

                //Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = numeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];

            }

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = numeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_ENVIAR_CODIGO_VERIFICACION,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            return result;
        }

        public async Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input)
        {
            _logger.LogInformation("RegistroPersona - Iniciar validacion de codigo de verificacion");

            var result = new RegistroPersonaOutput();

            try
            {
                var personaMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(input.NumeroRegistro);

                if (personaMdg == null)
                {
                    result.Error = string.Format(_localizer["PersonRegistration:PersonNotExist"]);
                    return result;
                }

                //1. Validar bloqueos
                _logger.LogInformation("RegistroPersona - Verificar si la persona tiene bloqueo por intentos fallidos");
                var respuestaBloqueo = await VerificarBloqueoPersona(personaMdg);
                if (!respuestaBloqueo.Success)
                {
                    return respuestaBloqueo;
                }
                _logger.LogInformation("RegistroPersona - Fin verificar si la persona tiene bloqueo por intentos fallidos");

                _logger.LogInformation("RegistroPersona - Verificar codigo");
                //2. Validar el codigo ingresado por el usuario
                var verificationCode =
                    await _verificationCodeAppService.GetVerificationCodeByRegisterNumber(input.NumeroRegistro);

                if (verificationCode.Codigo != input.CodigoVerificacion)
                {
                    personaMdg = await _personMdgAppService.IncreaseAccessFailedAsync(personaMdg.NumeroRegistro);
                    var resultadoIntentosPermitidos = await ValidarIntentosPermitidos(personaMdg);

                    if (!resultadoIntentosPermitidos.Success)
                    {
                        return resultadoIntentosPermitidos;
                    }

                    result.Error = string.Format(_localizer["PersonRegistration:VerificationCodeNotCorrect"]);
                    //Registrar en tracking
                    var registerTrackingDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = input.NumeroRegistro,
                        Event = RegistroTrackingConsts.EVENTO_INTENTO_FALLIDO_CODIGO_VERIFICACION,
                        Message = result.Error
                    };
                    await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                    return result;
                }
                _logger.LogInformation("RegistroPersona - Fin verificar codigo");


                if (verificationCode.Estado == EstadoCodigoVerificacion.Caducado)
                {
                    result.Error = string.Format(_localizer["PersonRegistration:VerificationCodeExpired"]);

                    //Registrar en tracking
                    var registerTrackingDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = input.NumeroRegistro,
                        Event = RegistroTrackingConsts.EVENTO_CODIGO_VERIFICACION_CADUCADO,
                        Message = result.Error
                    };
                    await _registerTrackingAppService.CreateAsync(registerTrackingDto);

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

                    //Registrar en tracking
                    var registerTrackingDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = input.NumeroRegistro,
                        Event = RegistroTrackingConsts.EVENTO_CODIGO_VERIFICACION_CADUCADO,
                        Message = result.Error
                    };
                    await _registerTrackingAppService.CreateAsync(registerTrackingDto);


                    return result;
                }

                await _verificationCodeAppService.ChangeStateAsync(input.CodigoVerificacion, input.NumeroRegistro,
                    EstadoCodigoVerificacion.Procesado);

                result.Success = true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Validar Codigo Verificacion. Numero Registro: {input.NumeroRegistro}");


                //Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = input.NumeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];

            }

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = input.NumeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_VALIDAR_CODIGO_VERIFICACION,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            _logger.LogInformation("RegistroPersona - Finalizar validacion de codigo de verificacion");

            return result;
        }

        public async Task<RegistroPersonaOutput> ObtenerInformacionPersonaAsync(string numeroRegistro)
        {
            _logger.LogInformation("RegistroPersona - Iniciar obtener informacion de persona");

            var result = new RegistroPersonaOutput();
            var personaDto = new PersonaDto();

            var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
            if (personMdg == null)
            {
                throw new UserFriendlyException(string.Format(_localizer["PersonRegistration:VerificationNumberFail"]));
            }

            _logger.LogInformation("RegistroPersona - Iniciar mapeo personaMdg a persona");
            var personInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

            try
            {
                personaDto = await _personaMapeadorAppService.MapeoPersonaDesdePersonaMdgAsync(personInformation);
            }
            catch (UserFriendlyException ex)
            {
                //Registrar en log
                _logger.LogError(ex, $"Obtener Informacion Persona. Numero Registro: {numeroRegistro}");

                //Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = numeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
                return result;
            }

            _logger.LogInformation("RegistroPersona - Fin mapeo personaMdg a persona");

            result.Success = true;
            result.PersonaDto = personaDto;

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = numeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_OBTENER_INFORMACION_PERSONA,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            _logger.LogInformation("RegistroPersona - Fin obtener informacion de persona");

            return result;
        }

        public async Task<RegistroPersonaOutput> RegistrarPersonaAsync(string numeroRegistro)
        {
            _logger.LogInformation("RegistroPersona - Iniciar registro de persona");

            var result = new RegistroPersonaOutput();
            var personaDto = new PersonaDto();

            var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
            if (personMdg == null)
            {
                throw new UserFriendlyException(string.Format(_localizer["PersonRegistration:VerificationNumberFail"]));
            }

            try
            {
                await _personMdgAppService.ChangeStateAsync(numeroRegistro, EstadoPersonaMdg.Procesado);

                var personInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                var entityCreateDto = await _personaMapeadorAppService.MapeoPersonaAsync(personInformation);

                //Crear persona
                _logger.LogInformation("RegistroPersona - Crear la persona");
                personaDto = await _personService.CreateAsync(entityCreateDto);
                _logger.LogInformation("RegistroPersona - Fin crear la persona");

            }
            catch (Exception ex)
            {
                //Registrar en log
                _logger.LogError(ex, $"Registrar Persona. Numero Registro: {numeroRegistro}");


                //Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = numeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = _localizer["PersonRegistration:ErrorPorDefecto"];
                return result;
            }

            result.Success = true;
            result.PersonaDto = personaDto;

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = numeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_REGISTRAR_PERSONA,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            _logger.LogInformation("RegistroPersona - Finalizar registro de persona");

            return result;
        }

        public async Task<bool> RechazarRegistroAsync(string numeroRegistro)
        {
            _logger.LogInformation("RegistroPersona - Iniciar accion de rechazo de registro");

            var personMdg = await _personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
            var personMdgInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

            //Registrar en tracking
            _logger.LogInformation("RegistroPersona - Registrar evento en tracking");
            var registerTrackingDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = numeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_RECHAZAR_REGISTRO,
                ValidationResult = true.ToString()
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingDto);

            var mensaje = string.Format(_localizer["PersonRegistration:PersonalInformationRejectText"]);

            var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));

            _notificadorClient.SetAccessToken(token);
            _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

            var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroRechazo(personMdgInformation, mensaje);

            _logger.LogInformation("RegistroPersona - Enviar correo electronico de rechazo");
            var notificado = await _notificadorClient.NotificadorAsync(notificacionDto);

            if (notificado)
            {
                _logger.LogInformation("RegistroPersona - Registrar envio de correo de rechazo en el tracking");
                registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = numeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_NOTIFICACION,
                    Message = mensaje
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);
            }

            _logger.LogInformation("RegistroPersona - Finalizar accion de rechazo de registro");

            return notificado;
        }


        #region metodos soporte

        private async Task<RegistroPersonaOutput> ValidateRules(InformacionPersonaDto personMdgInformation)
        {
            _logger.LogInformation("RegistroPersonaReglas - Validar reglas de negocio");

            var resultado = new RegistroPersonaOutput();

            var resultadoValidarPuntoControl = await ValidarPuntoControlAsync(personMdgInformation);
            if (!resultadoValidarPuntoControl.Success)
            {
                return resultadoValidarPuntoControl;
            }

            //Validar nacionalidad
            var resultadoValidarNacionalidad = await ValidarNacionalidadAsync(personMdgInformation);
            if (!resultadoValidarNacionalidad.Success)
            {
                return resultadoValidarNacionalidad;
            }

            //Validar edad
            var resultadoValidarEdad = await ValidarMayoriaEdadAsync(personMdgInformation);
            if (!resultadoValidarEdad.Success)
            {
                return resultadoValidarEdad;
            }

            resultado.Success = true;
            return resultado;
        }

        private async Task<RegistroPersonaOutput> ValidarPuntoControlAsync(InformacionPersonaDto personMdgInformation)
        {
            _logger.LogInformation("RegistroPersonaReglas - Validar punto de control del ciudadano");

            var resultado = new RegistroPersonaOutput();

            var registerNumber = personMdgInformation.CodigoRegistro;
            var mensaje = string.Empty;

            var entryByAccessPoint = await _personRegistrationRules.ValidarPuntoAccesoAsync(registerNumber);
            if (!entryByAccessPoint)
            {
                mensaje = string.Format(_localizer["PersonRegistration:AccessPointValidationFail"]);
            }

            //Registrar resultado en el tracking
            var registerTrackingDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = registerNumber,
                Event = PersonRegistrationSettings.FaseRegulacion.PuntoControl,
                ValidationResult = entryByAccessPoint.ToString(),
                Message = mensaje
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingDto);

            if (!entryByAccessPoint)
            {
                _logger.LogInformation("RegistroPersona - Obtener token para enviar correo notificacion punto de acceso");

                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


                _notificadorClient.SetAccessToken(token);
                _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

                var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroValidacion(_localizer["RegistroPersonaNotificacion:Notificacion"], personMdgInformation, mensaje);

                _logger.LogInformation("RegistroPersona - Enviar correo notificacion punto de acceso");
                var notificado = await _notificadorClient.NotificadorAsync(notificacionDto);

                //Registrar evento de envio de correo en el tracking
                if (notificado)
                {
                    var eventoNotificacionDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = registerNumber,
                        Event = RegistroTrackingConsts.EVENTO_NOTIFICACION,
                        Message = mensaje
                    };
                    await _registerTrackingAppService.CreateAsync(eventoNotificacionDto);
                }

                resultado.Error = mensaje;
                resultado.Success = false;

                _logger.LogInformation("RegistroPersonaReglas - Finalizao validar punto de control del ciudadano");
                return resultado;
            }

            resultado.Success = true;
            _logger.LogInformation("RegistroPersonaReglas - Finalizao validar punto de control del ciudadano");

            return resultado;
        }

        private async Task<RegistroPersonaOutput> ValidarNacionalidadAsync(InformacionPersonaDto personMdgInformation)
        {
            _logger.LogInformation("RegistroPersonaReglas - Validar la nacionalidad del ciudadano");

            var resultado = new RegistroPersonaOutput();

            var registerNumber = personMdgInformation.CodigoRegistro;
            var mensaje = string.Empty;

            var nationalityAllowed = await _personRegistrationRules.ValidarNacionalidadAsync(personMdgInformation.PaisNacimiento);
            if (!nationalityAllowed)
            {
                mensaje = string.Format(_localizer["PersonRegistration:NationalityValidationFail"]);
            }

            var registerTrackingDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = registerNumber,
                Event = PersonRegistrationSettings.FaseRegulacion.Nacionalidad,
                ValidationResult = nationalityAllowed.ToString(),
                Message = mensaje
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingDto);

            if (!nationalityAllowed)
            {
                _logger.LogInformation("RegistroPersona - Obtener token para enviar correo notificacion nacionalidad");

                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


                _notificadorClient.SetAccessToken(token);
                _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

                var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroValidacion(_localizer["RegistroPersonaNotificacion:Notificacion"], personMdgInformation, mensaje);

                _logger.LogInformation("RegistroPersona - Enviar correo notificacion nacionalidad");
                var notificado = await _notificadorClient.NotificadorAsync(notificacionDto);

                //Registrar evento de envio de correo en el tracking
                if (notificado)
                {
                    var eventoNotificacionDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = registerNumber,
                        Event = RegistroTrackingConsts.EVENTO_NOTIFICACION,
                        Message = mensaje
                    };
                    await _registerTrackingAppService.CreateAsync(eventoNotificacionDto);
                }

                resultado.Error = mensaje;
                resultado.Success = false;

                _logger.LogInformation("RegistroPersonaReglas - Finalizado validar la nacionalidad del ciudadano");
                return resultado;
            }

            resultado.Success = true;
            _logger.LogInformation("RegistroPersonaReglas - Finalizado validar la nacionalidad del ciudadano");

            return resultado;
        }

        private async Task<RegistroPersonaOutput> ValidarMayoriaEdadAsync(InformacionPersonaDto personMdgInformation)
        {
            _logger.LogInformation("RegistroPersonaReglas - Validar la edad del ciudadano");

            var resultado = new RegistroPersonaOutput();

            var registerNumber = personMdgInformation.CodigoRegistro;
            var mensaje = string.Empty;

            var isLegalAge = await _personRegistrationRules.ValidarMayoriaEdadAsync(personMdgInformation.FechaNacimiento);
            if (!isLegalAge)
            {
                mensaje = string.Format(_localizer["PersonRegistration:LegalAgeValidationFail"]);
            }

            var registerTrackingDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = registerNumber,
                Event = PersonRegistrationSettings.FaseRegulacion.MayoriaEdad,
                ValidationResult = isLegalAge.ToString(),
                Message = mensaje
            };
            await _registerTrackingAppService.CreateAsync(registerTrackingDto);

            if (!isLegalAge)
            {
                _logger.LogInformation("RegistroPersona - Obtener token para enviar correo notificacion mayor de edad");

                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("NotificacionCliente"));


                _notificadorClient.SetAccessToken(token);
                _notificadorClient.AddHeaders(_abpCorrelationIdOptions.HttpHeaderName, _correlationIdProvider.Get());

                var notificacionDto = NotificacionMapeador.MapeoPersonaRegistroValidacion(_localizer["RegistroPersonaNotificacion:Notificacion"], personMdgInformation, mensaje);

                _logger.LogInformation("RegistroPersona - Enviar correo notificacion mayor de edad");
                var notificado = await _notificadorClient.NotificadorAsync(notificacionDto);

                //Registrar evento de envio de correo en el tracking
                if (notificado)
                {
                    var eventoNotificacionDto = new CreateRegisterTrackingDto
                    {
                        RegisterNumber = registerNumber,
                        Event = RegistroTrackingConsts.EVENTO_NOTIFICACION,
                        Message = mensaje
                    };
                    await _registerTrackingAppService.CreateAsync(eventoNotificacionDto);
                }

                resultado.Error = mensaje;
                resultado.Success = false;
                return resultado;
            }

            resultado.Success = true;

            _logger.LogInformation("Finalizar - Validar la edad del ciudadano");
            return resultado;
        }

        private async Task<RegistroPersonaOutput> VerificarBloqueoPersona(PersonaMdgDto personMdg)
        {
            var result = new RegistroPersonaOutput();
            if (personMdg.BloqueoHabilitado)
            {
                var resultCompare = DateTime.Compare(DateTime.Now, personMdg.FinBloqueo);

                // Desbloquear usuario
                if (resultCompare <= 0)
                {
                    var horaFinBloqueo = personMdg.FinBloqueo.AddMinutes(1);
                    horaFinBloqueo = new DateTime(horaFinBloqueo.Year, horaFinBloqueo.Month, horaFinBloqueo.Day, horaFinBloqueo.Hour, horaFinBloqueo.Minute, 0);
                    result.Error = string.Format(_localizer["PersonRegistration:UserIsLocked"], horaFinBloqueo);
                    result.Success = false;
                    return result;
                }

                await _personMdgAppService.UnlockUserAsync(personMdg.NumeroRegistro);
                personMdg.BloqueoHabilitado = false;
                personMdg.ContadorAccesoFallido = 0;

            }

            result.Success = true;
            return result;
        }

        private async Task<RegistroPersonaOutput> VerificarFechaNacimiento(PersonaMdgDto personMdg, DateTime fechaNacimiento)
        {
            var resultado = new RegistroPersonaOutput();

            var birthDateIsEqual = DateTime.Compare(personMdg.FechaNacimiento.Date, fechaNacimiento.Date);
            if (birthDateIsEqual != 0)
            {
                personMdg = await _personMdgAppService.IncreaseAccessFailedAsync(personMdg.NumeroRegistro);
                var resultadoIntentosPermitidos = await ValidarIntentosPermitidos(personMdg);

                if (!resultadoIntentosPermitidos.Success)
                {
                    return resultadoIntentosPermitidos;
                }

                resultado.Error = string.Format(_localizer["PersonRegistration:BirthDateNotCorrect"]);
                resultado.Success = false;
                return resultado;
            }

            resultado.Success = true;
            return resultado;
        }

        private async Task<RegistroPersonaOutput> ValidarIntentosPermitidos(PersonaMdgDto personaMdg)
        {
            var resultado = new RegistroPersonaOutput();

            var intentosPermitidos =
                    await SettingProvider.GetOrNullAsync(
                        PersonRegistrationSettings.CodigoVerificacion.IntentosPermitiddos);

            if ((personaMdg.ContadorAccesoFallido + 1) > Convert.ToInt32(intentosPermitidos))
            {
                var duracionBloqueo =
                    await SettingProvider.GetOrNullAsync(PersonRegistrationSettings.CodigoVerificacion
                        .DuracionBloqueo);

                // Bloquear usuario
                personaMdg = await _personMdgAppService.LockUserAsyn(personaMdg.NumeroRegistro, double.Parse(duracionBloqueo));

                var horaFinBloqueo = personaMdg.FinBloqueo.AddMinutes(1);
                horaFinBloqueo = new DateTime(horaFinBloqueo.Year, horaFinBloqueo.Month, horaFinBloqueo.Day, horaFinBloqueo.Hour, horaFinBloqueo.Minute, 0);
                resultado.Error = string.Format(_localizer["PersonRegistration:UserIsLocked"], horaFinBloqueo);
                resultado.Success = false;

                //Registrar en tracking
                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = personaMdg.NumeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_USUARIO_BLOQUEADO,
                    Message = resultado.Error
                };
                await _registerTrackingAppService.CreateAsync(registerTrackingDto);

                return resultado;
            }

            resultado.Success = true;
            return resultado;
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

        #endregion metodos soporte
    }
}