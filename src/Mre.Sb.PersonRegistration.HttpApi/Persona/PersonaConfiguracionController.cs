using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;

namespace Mre.Sb.RegistroPersona.Persona
{

    [RemoteService(Name = RegistroPersonaRemoteServiceConsts.RemoteServiceName)]
    [Area("RegistroPersona")]
    [Route("api/RegistroPersona/personaConfiguracion")]
    public class PersonaConfiguracionController : RegistroPersonaBaseController, IPersonaConfiguracionAppService
    {
        private readonly IPersonaConfiguracionAppService _personaConfiguracionAppService;

        public PersonaConfiguracionController(IPersonaConfiguracionAppService personaConfiguracionAppService)
        {
            _personaConfiguracionAppService = personaConfiguracionAppService;
        }

        [HttpGet]
        public async Task<PersonaConfiguracionDto> ObtenerAsync()
        {
            return await _personaConfiguracionAppService.ObtenerAsync();
        }

        [HttpPut]
        public async Task ActualizarAsync(ActualizarPersonaConfiguracionDto input)
        {
            await _personaConfiguracionAppService.ActualizarAsync(input);
        }

    }
}
