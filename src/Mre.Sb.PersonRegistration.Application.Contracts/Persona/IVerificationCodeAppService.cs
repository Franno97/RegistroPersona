using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IVerificationCodeAppService : ICrudAppService<CodigoVerificacionDto, int, PagedAndSortedResultRequestDto, CrearActualizarCodigoVerificacionDto>
    {
        public Task<CodigoVerificacionDto> GetByRegisterNumber(string registerNumber);

        public Task<CodigoVerificacionDto> UpdateAsync(CrearActualizarCodigoVerificacionDto input);

        public Task<CodigoVerificacionDto> GetVerificationCodeByRegisterNumber(string registerNumber);

        public Task ChangeStateAsync(string verificationCode, string registerNumber, EstadoCodigoVerificacion state);
    }
}
