using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public interface IRegistroTrackingAppService : ICrudAppService<RegistroTrackingDto, int, PagedAndSortedResultRequestDto, CreateRegisterTrackingDto>
    {
    }
}
