using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public class PersonaMdgManager : DomainService
    {
        private readonly IPersonaMdgRepository _repository;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;

        public PersonaMdgManager(IPersonaMdgRepository repository, IStringLocalizer<PersonRegistrationResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public virtual async Task<PersonaMdg> CrearAsync(Guid id, string registerNumber, DateTime birthDate, string nationality, string response, EstadoPersonaMdg state)
        {

            // Check if exist register number
            var existing = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existing != null)
            {
                throw new AbpException($"register number exist {registerNumber}");
            }

            var entity = new PersonaMdg(id: id, numeroRegistro: registerNumber, fechaNacimiento: birthDate, nacionalidad: nationality, respuesta: response, estado: state);

            return entity;
        }

        public async Task<bool> VerificarCiudadanoExisteAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            bool existingCitizen = false;
            var existingPerson = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existingPerson == null)
            {
                return existingCitizen;
            }

            if (existingPerson.Estado == EstadoPersonaMdg.Procesado)
            {
                return !existingCitizen;
            }

            return existingCitizen;
        }

        public async Task<bool> VerificarEstadoCiudadanoAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            bool citizenRegistered = false;
            var existingPerson = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existingPerson == null)
            {
                return citizenRegistered;
            }

            if (existingPerson.Estado == EstadoPersonaMdg.Registrado)
            {
                return !citizenRegistered;
            }

            return citizenRegistered;
        }

        public async Task<bool> VerificarFechaNacimientoAsync(string registerNumber, DateTime birthDate)
        {
            Check.NotNull(birthDate, nameof(birthDate));
            Check.NotNull(registerNumber, nameof(registerNumber));

            bool isCorrectDateBirth = false;
            var existingPerson = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existingPerson == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            var isEqual = DateTime.Compare(existingPerson.FechaNacimiento, birthDate);

            if (isEqual != 0)
            {
                existingPerson.IncrementarAccesoFallido();

                await _repository.UpdateAsync(existingPerson, autoSave: true);
                return isCorrectDateBirth;
            }

            return !isCorrectDateBirth;
        }

        public async Task<PersonaMdg> ObtenerPersonaMdgPorNumeroRegistroAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var personMdg = await _repository.FindByRegisterNumberAsync(registerNumber);

            return personMdg;
        }

        public async Task<string> ObtenerNacionalidadAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var existingPerson = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existingPerson == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            var nationality = existingPerson.PaisId;

            return nationality;
        }

        public async Task<DateTime> ObtenerFechaNacimientoAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var existingPerson = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (existingPerson == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            var birthate = existingPerson.FechaNacimiento;

            return birthate;
        }

        public async Task<bool> VerificarBloqueoPersonaAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var personMdg = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (personMdg == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            var isLocked = personMdg.BloqueoHabilitado;

            return isLocked;
        }

        public async Task<PersonaMdg> IncrementarAccesoFallidoAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var personMdg = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (personMdg == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            personMdg.IncrementarAccesoFallido();

            return personMdg;
        }

        public async Task<PersonaMdg> BloquearUsuarioAsync(string registerNumber, double lockDuration)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var personMdg = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (personMdg == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            personMdg.BloquearUsuario(lockDuration);

            return personMdg;
        }

        public async Task<PersonaMdg> DesbloquearUsuarioAsync(string registerNumber)
        {
            Check.NotNull(registerNumber, nameof(registerNumber));

            var personMdg = await _repository.FindByRegisterNumberAsync(registerNumber);

            if (personMdg == null)
            {
                throw new AbpException(string.Format(_localizer["PersonRegistration:PersonNotExist"]));
            }

            personMdg.DesbloquearUsuario();

            return personMdg;
        }

        public async Task<PersonaMdg> CambiarEstadoAsync(string registerNumber, EstadoPersonaMdg state)
        {
            Check.NotNullOrWhiteSpace(registerNumber, nameof(registerNumber));

            var queryable = await _repository.GetQueryableAsync();
            queryable = queryable.Where(x => x.NumeroRegistro.Equals(registerNumber));

            var entity = await AsyncExecuter.FirstOrDefaultAsync(queryable);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(CodigoVerificacion), registerNumber);
            }

            entity.Estado = state;

            return entity;
        }

    }
}
