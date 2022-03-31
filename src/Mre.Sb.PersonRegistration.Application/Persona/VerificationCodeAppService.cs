using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class VerificationCodeAppService : CrudAppService<
            CodigoVerificacion,
            CodigoVerificacionDto,
            int,
            PagedAndSortedResultRequestDto,
            CrearActualizarCodigoVerificacionDto>,
        IVerificationCodeAppService
    {

        private readonly CodigoVerificacionManager _verificationCodeManager;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public VerificationCodeAppService(
            IRepository<CodigoVerificacion, int> repository,
            CodigoVerificacionManager verificationCodeManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(repository)
        {
            _verificationCodeManager = verificationCodeManager;
            _unitOfWorkManager = unitOfWorkManager;
        }


        public async Task<CodigoVerificacionDto> GetByRegisterNumber(string registerNumber)
        {
            await CheckGetPolicyAsync();
            var verificationCode = await _verificationCodeManager.GetByRegisterNumber(registerNumber);

            return ObjectMapper.Map<CodigoVerificacion, CodigoVerificacionDto>(verificationCode);
        }

        public override async Task<CodigoVerificacionDto> CreateAsync(CrearActualizarCodigoVerificacionDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckCreatePolicyAsync();

                var entity = await _verificationCodeManager.CreateAsync(
                    code: input.Codigo,
                    generationDate: input.FechaGeneracion,
                    expirationDate: input.FechaExpiracion,
                    personMdgRegisterNumber: input.PersonaMdgNumeroRegistro,
                    personMdgEmail: input.PersonaMdgCorreoElectronico,
                    state: EstadoCodigoVerificacion.Registrado
                    );


                TryToSetTenantId(entity);

                await Repository.InsertAsync(entity, autoSave: true);

                await uow.CompleteAsync();
                return await GetAsync(entity.Id);
            }

        }

        public async Task<CodigoVerificacionDto> UpdateAsync(CrearActualizarCodigoVerificacionDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckUpdatePolicyAsync();

                var entity = await _verificationCodeManager.UpdateAsync(
                    id: input.Id,
                    code: input.Codigo,
                                    personMdgEmail: input.PersonaMdgCorreoElectronico,
                                    state: EstadoCodigoVerificacion.Registrado,
                                    expirationDate: input.FechaExpiracion
                                    );


                await Repository.UpdateAsync(entity, autoSave: true);

                await uow.CompleteAsync();
                return await GetAsync(entity.Id);
            }
        }

        public async Task<CodigoVerificacionDto> GetVerificationCodeByRegisterNumber(string registerNumber)
        {
            await CheckGetPolicyAsync();

            var verificationCode = await _verificationCodeManager.GetVerificationCodeByRegisterNumber(registerNumber);

            return ObjectMapper.Map<CodigoVerificacion, CodigoVerificacionDto>(verificationCode);
        }

        public async Task ChangeStateAsync(string verificationCode, string registerNumber, EstadoCodigoVerificacion state)
        {
            await CheckUpdatePolicyAsync();

            var entity = await _verificationCodeManager.ChangeStateAsync(verificationCode, registerNumber, state);

            await Repository.UpdateAsync(entity, autoSave: true);
        }
    }
}
