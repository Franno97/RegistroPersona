using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.PersonRegistration.Parameterization
{
    public interface IParameterizationAppService : ICrudAppService<
        ParameterizationDto, 
        int, 
        PagedAndSortedResultRequestDto,
        CreateUpdateParameterizationDto>
    {

    }
}
