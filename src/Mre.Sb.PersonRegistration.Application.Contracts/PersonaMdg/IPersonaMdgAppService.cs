using System;
using System.Threading.Tasks;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.Proceso;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public interface IPersonaMdgAppService : ICrudAppService<PersonaMdgDto, Guid, PagedAndSortedResultRequestDto, CrearActualizarPersonaMdgDto>
    {
        public Task<bool> CitizenAlreadyExistsAsync(string registerNumber);
        public Task<bool> StateCitizenIsRegisteredAsync(string registerNumber);
        public Task<bool> DateBirthIsCorrectAsync(string registerNumber, DateTime birthDate);
        public Task<PersonaMdgDto> GetPersonByRegisterNumberAsync(string registerNumber);

        /// <summary>
        /// Obtener personaMdg, se consulta en tabla intermedia, si no existe se obtiene desde el servicio de extranjeros
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        public Task<PersonaMdgOutput> ObtenerPorNumeroRegistroAsync(string numeroRegistro);

        public Task<string> GetNationalityAsync(string registerNumber);
        public Task<bool> PersonIsLockedAsync(string registerNumber);
        public Task<PersonaMdgDto> IncreaseAccessFailedAsync(string registerNumber);
        public Task UnlockUserAsync(string registerNumber);
        public Task<PersonaMdgDto> LockUserAsyn(string registerNumber, double lockDuration);
        public Task ChangeStateAsync(string registerNumber, EstadoPersonaMdg state);
    }
}
