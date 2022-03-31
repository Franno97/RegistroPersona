using System;
using System.Threading.Tasks;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class RegistroTrackingAppService :
        CrudAppService<
            RegistroTracking,
            RegistroTrackingDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateRegisterTrackingDto>,
            IRegistroTrackingAppService
    {

        private readonly RegistroTrackingManager _registerTrackingManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RegistroTrackingAppService(IRepository<RegistroTracking, int> repository, RegistroTrackingManager registerTrackingManager, IUnitOfWorkManager unitOfWorkManager)
            : base(repository)
        {
            _registerTrackingManager = registerTrackingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task<RegistroTrackingDto> CreateAsync(CreateRegisterTrackingDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckCreatePolicyAsync();

                var entity = await _registerTrackingManager.CreateAsync(
                    registerNumber: input.RegisterNumber,
                    event_: input.Event,
                    validationresult: input.ValidationResult,
                    message: input.Message,
                    eventDate: input.EventDate
                    );


                TryToSetTenantId(entity);

                await Repository.InsertAsync(entity, true);


                await uow.CompleteAsync();
                return await GetAsync(entity.Id);
            }
        }
    }
}
