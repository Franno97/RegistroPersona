using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Mre.Sb.RegistroPersona.Proceso
{

    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/FamiliarProceso")]
    //[Authorize]
    public class FamiliarProcesoController : RegistroPersonaBaseController, IFamiliarProcesoAppService
    {
        public IFamiliarProcesoAppService FamiliarProcesoAppService { get; }


        public FamiliarProcesoController(IFamiliarProcesoAppService familiarProcesoAppService)
        {
            FamiliarProcesoAppService = familiarProcesoAppService;
        }

        [HttpPost]
        [Route("verificacionprevia")]
        public async Task<RegistroPersonaOutput> VerificacionPreviaAsync(ChequeoPrevioInput input)
        {
            return await FamiliarProcesoAppService.VerificacionPreviaAsync(input);
        }

        [HttpPost]
        [Route("enviarcodigo")]
        public async Task<RegistroPersonaOutput> EnviarCodigoVerificacionAsync(string numeroRegistro)
        {
            return await FamiliarProcesoAppService.EnviarCodigoVerificacionAsync(numeroRegistro);
        }

        [HttpPost]
        [Route("validarcodigo")]
        public async Task<RegistroPersonaOutput> ValidarCodigoVerificacionAsync(ValidarCodigoVerificacionInput input)
        {
            return await FamiliarProcesoAppService.ValidarCodigoVerificacionAsync(input);
        }

        [HttpGet("obtenerinformacionpersona/{numeroRegistro}")]
        public async Task<PersonaDto> ObtenerInformacionPersonaAsync(string numeroRegistro)
        {
            return await FamiliarProcesoAppService.ObtenerInformacionPersonaAsync(numeroRegistro);
        }

        [HttpPost("guardarpersona")]
        public async Task<PersonaDto> GuardarPersonaAsync(string numeroRegistro)
        {
            return await FamiliarProcesoAppService.GuardarPersonaAsync(numeroRegistro);
        }
    }

}
