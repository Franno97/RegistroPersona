using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public class RegistroTrackingManager : DomainService
    {
        private readonly IRepository<RegistroTracking> _repository;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public RegistroTrackingManager(IRepository<RegistroTracking> repository, IStringLocalizer<PersonRegistrationResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public virtual async Task<RegistroTracking> CreateAsync(string registerNumber, string event_, string validationresult, string message, DateTime eventDate)
        {
            var entity = new RegistroTracking(registerNumber: registerNumber, eventParameter: event_, validationResult: validationresult, message: message, eventDate: eventDate);


            return await Task.FromResult(entity);
        }

    }
}
