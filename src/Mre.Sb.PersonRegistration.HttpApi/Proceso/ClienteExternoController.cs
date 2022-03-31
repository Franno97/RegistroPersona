using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace Mre.Sb.RegistroPersona.Proceso
{
    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/ClienteExterno")]
    public class ClienteExternoController : RegistroPersonaBaseController, IClientExternalDmzAppService
    {
        public IClientExternalDmzAppService ClientExternalAppService { get; }


        public ClienteExternoController(
            IClientExternalDmzAppService clientExternalExtranjerosAppService)
        {
            ClientExternalAppService = clientExternalExtranjerosAppService;
        }

        [HttpGet("puntoControl/{numeroRegistro}")]
        public Task<ConsultarPuntoControlOutput> ConsultarPuntoControlAsync(string numeroRegistro)
        {
            throw new NotImplementedException();
        }

        [HttpGet("informacionPersona/{numeroRegistro}")]
        public Task<InformacionPersonaDto> ObtenerInformacionPersona(string numeroRegistro)
        {
            throw new NotImplementedException();
        }

        [HttpGet("multas/{numeroRegistro}")]
        public async Task<ConsultarMultasRespuesta> ConsultarMultasAsync(string numeroRegistro)
        {
            return await ClientExternalAppService.ConsultarMultasAsync(numeroRegistro);
        }

        [HttpGet("flujoMigratorio/{numeroRegistro}")]
        public async Task<ConsultarFlujoMigratorioRespuesta> ConsultarFlujoMigratorioAsync(string numeroRegistro)
        {
            return await ClientExternalAppService.ConsultarFlujoMigratorioAsync(numeroRegistro);
        }

        [HttpGet("pago/{numeroComprobante}")]
        public async Task<ConsultarPagoRespuesta> ConsultarPagoAsync(string numeroComprobante)
        {
            return await ClientExternalAppService.ConsultarPagoAsync(numeroComprobante);
        }

        [HttpGet("discapacidad/{numeroCedula}")]
        public async Task<ConsultarDiscapacidadRespuesta> ConsultarDiscapacidadAsync(string numeroCedula)
        {
            return await ClientExternalAppService.ConsultarDiscapacidadAsync(numeroCedula);
        }
    }
}
