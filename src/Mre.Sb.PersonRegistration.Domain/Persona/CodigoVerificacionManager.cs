using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class CodigoVerificacionManager : DomainService
    {
        private readonly IRepository<CodigoVerificacion, int> _repository;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public CodigoVerificacionManager(IRepository<CodigoVerificacion, int> repository, IStringLocalizer<PersonRegistrationResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<CodigoVerificacion> CreateAsync(string code, DateTime generationDate, DateTime expirationDate, string personMdgRegisterNumber, string personMdgEmail, EstadoCodigoVerificacion state)
        {
            var entity = new CodigoVerificacion(codigo: code, fechaGeneracion: generationDate, fechaExpiracion: expirationDate, personaMdgNumeroRegistro: personMdgRegisterNumber, personaMdgCorreoElectronico: personMdgEmail, estado: state);

            return await Task.FromResult(entity);
        }

        public async Task<CodigoVerificacion> UpdateAsync(int id, string code, string personMdgEmail, EstadoCodigoVerificacion state, DateTime expirationDate)
        {
            var verificationCode = await _repository.FirstAsync(x => x.Id == id);

            verificationCode.Codigo = code;
            verificationCode.Estado = state;
            verificationCode.PersonaMdgCorreoElectronico = personMdgEmail;
            verificationCode.FechaExpiracion = expirationDate;

            return verificationCode;
        }

        public async Task<CodigoVerificacion> GetVerificationCodeByRegisterNumber(string registerNumber)
        {
            Check.NotNullOrWhiteSpace(registerNumber, nameof(registerNumber));

            var queryable = await _repository.GetQueryableAsync();
            queryable = queryable.Where(e => e.PersonaMdgNumeroRegistro == registerNumber);

            var verificationCodes = queryable.OrderByDescending(e => e.FechaGeneracion);

            var verificationCode = await AsyncExecuter.FirstOrDefaultAsync(verificationCodes);
            if (verificationCode == null)
            {
                throw new EntityNotFoundException(typeof(CodigoVerificacion), registerNumber);
            }


            return verificationCode;

        }

        public async Task<CodigoVerificacion> ChangeStateAsync(string verificationCode, string registerNumber, EstadoCodigoVerificacion state)
        {
            Check.NotNullOrWhiteSpace(verificationCode, nameof(verificationCode));
            Check.NotNullOrWhiteSpace(registerNumber, nameof(registerNumber));
            Check.NotNull(state, nameof(state));

            var queryable = await _repository.GetQueryableAsync();
            queryable = queryable.Where(e => e.Codigo == verificationCode);
            queryable = queryable.Where(e => e.PersonaMdgNumeroRegistro == registerNumber);

            var code = await AsyncExecuter.FirstOrDefaultAsync(queryable);
            if (code == null)
            {
                throw new EntityNotFoundException(typeof(CodigoVerificacion), registerNumber);
            }
            code.CambiarEstado(state);

            return code;
        }

        public async Task<CodigoVerificacion> GetByRegisterNumber(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var entity = await _repository.FirstOrDefaultAsync(x => x.PersonaMdgNumeroRegistro == registerNumber);

            return entity;
        }
    }
}
