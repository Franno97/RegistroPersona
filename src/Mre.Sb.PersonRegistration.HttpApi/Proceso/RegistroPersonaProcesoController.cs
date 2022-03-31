using Microsoft.AspNetCore.Mvc;
using Mre.Sb.PersonRegistration.HttpApiClient;
using Mre.Sb.RegistroPersona.Persona;
using System.Threading.Tasks;
using Volo.Abp;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;
using Mre.Sb.PersonRegistration.Localization;
using Microsoft.Extensions.Localization;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Newtonsoft.Json;
using Volo.Abp.IdentityModel;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Mre.Sb.RegistroPersona.Proceso
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/RegistroPersonaProceso")]
    public class RegistroPersonaProcesoController : RegistroPersonaBaseController,
        IRegistroPersonaProcesoAppService
    {
        private readonly IIdentidadClient identidadClient;
        private readonly IIdentityModelAuthenticationService identityModelAuthenticationService;
        
        private readonly IRegistroTrackingAppService registerTrackingAppService;
        private readonly ILogger<RegistroPersonaProcesoController> logger;
        private readonly IStringLocalizer<PersonRegistrationResource> localizer;
        private readonly IPersonaMdgAppService personMdgAppService;
        private readonly IConfiguration config;

        public IRegistroPersonaProcesoAppService RegistroPersonaProcesoAppService { get; }
        public AbpIdentityClientOptions ClientOptions { get; }

        public RegistroPersonaProcesoController(
            IRegistroPersonaProcesoAppService registroPersonaProcesoAppService,
            IConfiguration config,
            IIdentidadClient identidadClient,
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IOptions<AbpIdentityClientOptions> abpIdentityClientOptions,
            IRegistroTrackingAppService registerTrackingAppService,
            ILogger<RegistroPersonaProcesoController> logger,
            IStringLocalizer<PersonRegistrationResource> localizer,
            IPersonaMdgAppService personMdgAppService)
        {
            RegistroPersonaProcesoAppService = registroPersonaProcesoAppService;
            this.identidadClient = identidadClient;
            this.identityModelAuthenticationService = identityModelAuthenticationService;
            
            this.ClientOptions = abpIdentityClientOptions.Value;
            this.registerTrackingAppService = registerTrackingAppService;
            this.logger = logger;
            this.localizer = localizer;
            this.personMdgAppService = personMdgAppService;
            this.config = config;
        }

        [HttpPost]
        [Route("verificacionprevia")]
        public async Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input)
        {
            return await RegistroPersonaProcesoAppService.VerificacionPreviaAsync(input);
        }

        [HttpPost]
        [Route("enviarcodigo")]
        public Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro)
        {
            return RegistroPersonaProcesoAppService.EnviarCodigoVerificacionAsync(numeroRegistro);
        }

        [HttpPost]
        [Route("validarcodigo")]
        public async Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input)
        {
            return await RegistroPersonaProcesoAppService.ValidarCodigoVerificacionAsync(input);
        }

        [HttpGet("obtenerinformacionpersona/{numeroRegistro}")]
        public async Task<RegistroPersonaOutput> ObtenerInformacionPersonaAsync(string numeroRegistro)
        {
            return await RegistroPersonaProcesoAppService.ObtenerInformacionPersonaAsync(numeroRegistro);
        }

        [HttpPost("registrarPersona")]
        public async Task<RegistroPersonaOutput> RegistrarPersonaAsync(string numeroRegistro)
        {
            var result = new RegistroPersonaOutput();
            var resultadoRegistrar = new RegistroPersonaOutput();

            try
            {
                var personMdg = await personMdgAppService.GetPersonByRegisterNumberAsync(numeroRegistro);
                var personInformation = JsonConvert.DeserializeObject<InformacionPersonaDto>(personMdg.Respuesta);

                //Crear el usuario
                var token = await identityModelAuthenticationService.GetAccessTokenAsync(GetClientConfiguration("BaseCliente")); 
                identidadClient.SetAccessToken(token);

                var usuarioCrearDto = UsuarioMapeador.GenerarUsuarioCrearDto(personInformation);
                var user = await identidadClient.UsuarioPostAsync(usuarioCrearDto);

                resultadoRegistrar = await RegistroPersonaProcesoAppService.RegistrarPersonaAsync(numeroRegistro);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $" Registrar Persona. Numero Registro: {numeroRegistro}"); 

                var registerTrackingDto = new CreateRegisterTrackingDto
                {
                    RegisterNumber = numeroRegistro,
                    Event = RegistroTrackingConsts.EVENTO_ERROR,
                    Message = ex.Message
                };
                await registerTrackingAppService.CreateAsync(registerTrackingDto);

                result.Success = false;
                result.Error = localizer["PersonRegistration:ErrorPorDefecto"];
                return result;
            }

            result.Success = true;
            result.PersonaDto = resultadoRegistrar.PersonaDto;

            //Registrar en tracking
            var registerTrackingSucessDto = new CreateRegisterTrackingDto
            {
                RegisterNumber = numeroRegistro,
                Event = RegistroTrackingConsts.EVENTO_CREAR_USUARIO,
                ValidationResult = true.ToString()
            };
            await registerTrackingAppService.CreateAsync(registerTrackingSucessDto);

            return result;
        }

        [HttpPost("rechazarRegistro")]
        public async Task<bool> RechazarRegistroAsync(string numeroRegistro)
        {
            var result = await RegistroPersonaProcesoAppService.RechazarRegistroAsync(numeroRegistro);
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