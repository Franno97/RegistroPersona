using Volo.Abp.Application.Dtos;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class ObtenerPersonaInputDto : PagedAndSortedResultRequestDto
    {
        public string Filtro { get; set; }
    }
}
