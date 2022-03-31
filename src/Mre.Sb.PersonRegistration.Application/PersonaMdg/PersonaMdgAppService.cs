using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.PersonRegistration.Settings;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.Proceso;
using Newtonsoft.Json;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace Mre.Sb.RegistroPersona.PersonaMdg
{
    public class PersonaMdgAppService :
         CrudAppService<
            PersonaMdg,
            PersonaMdgDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CrearActualizarPersonaMdgDto>,
        IPersonaMdgAppService
    {
        private readonly PersonaMdgManager _personMdgManager;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IClientExternalDmzAppService _clientExternalDmzAppService;
        private readonly IStringLocalizer<PersonRegistrationResource> _localizer;
        private readonly ISettingProvider _settingProvider;
        private readonly ILogger<PersonaMdgAppService> _logger;

        public PersonaMdgAppService(
            IRepository<PersonaMdg, Guid> repository, 
            PersonaMdgManager personMdgManager, 
            IUnitOfWorkManager unitOfWorkManager,
            IClientExternalDmzAppService clientExternalDmzAppService,
            IStringLocalizer<PersonRegistrationResource> localizer,
            ISettingProvider settingProvider,
            ILogger<PersonaMdgAppService> logger)
            : base(repository)
        {
            _personMdgManager = personMdgManager;
            _unitOfWorkManager = unitOfWorkManager;
            _clientExternalDmzAppService = clientExternalDmzAppService;
            _localizer = localizer;
            _settingProvider = settingProvider;
            _logger = logger;
        }

        public override async Task<PersonaMdgDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(a => a.Id == id);

            var queryableDto = queryable.Select(entity => new PersonaMdgDto()
            {
                Id = entity.Id,
                NumeroRegistro = entity.NumeroRegistro,
                FechaNacimiento = entity.FechaNacimiento,
                PaisId = entity.PaisId,
                Respuesta = entity.Respuesta,
                Estado = entity.Estado,
                FinBloqueo = entity.FinBloqueo,
                BloqueoHabilitado = entity.BloqueoHabilitado,
                ContadorAccesoFallido = entity.ContadorAccesoFallido
            });

            var entityDto = await AsyncExecuter.FirstOrDefaultAsync(queryableDto);

            return entityDto;
        }

        public override async Task<PersonaMdgDto> CreateAsync(CrearActualizarPersonaMdgDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckCreatePolicyAsync();

                var entity = await _personMdgManager.CrearAsync(
                    GuidGenerator.Create(),
                    registerNumber: input.NumeroRegistro,
                    birthDate: input.FechaNacimiento,
                    nationality: input.PaisId,
                    response: input.Respuesta,
                    state: EstadoPersonaMdg.Registrado
                    );


                TryToSetTenantId(entity);

                await Repository.InsertAsync(entity, true);

                await uow.CompleteAsync();
                return await GetAsync(entity.Id);
            }

        }

        public override async Task<PersonaMdgDto> UpdateAsync(Guid id, CrearActualizarPersonaMdgDto input)
        {

            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            return await GetAsync(entity.Id);
        }

        public async Task<bool> CitizenAlreadyExistsAsync(string registerNumber)
        {
            return await _personMdgManager.VerificarCiudadanoExisteAsync(registerNumber);
        }

        public async Task<bool> DateBirthIsCorrectAsync(string registerNumber, DateTime birthDate)
        {
            return await _personMdgManager.VerificarFechaNacimientoAsync(registerNumber, birthDate);
        }

        public async Task<PersonaMdgDto> GetPersonByRegisterNumberAsync(string registerNumber)
        {
            await CheckGetPolicyAsync();

            var personMdg = await _personMdgManager.ObtenerPersonaMdgPorNumeroRegistroAsync(registerNumber);

            return ObjectMapper.Map<PersonaMdg, PersonaMdgDto>(personMdg);
        }
        
        public async Task<PersonaMdgOutput> ObtenerPorNumeroRegistroAsync(string numeroRegistro)
        {
            await CheckGetPolicyAsync();

            var resultado = new PersonaMdgOutput();

            _logger.LogInformation("Obtener informacion de la persona");
            //Obtener persona desde tabla intermedia
            var personMdg = await _personMdgManager.ObtenerPersonaMdgPorNumeroRegistroAsync(numeroRegistro);

            if(personMdg == null)
            {
                _logger.LogInformation("Obtener informacion de la persona desde servicio Mdg");
                //Consultar servicio de extranjeros
                var personaInfo = await ObtenerDesdeServicio(numeroRegistro);

                if(personaInfo == null)
                {
                    resultado.Success = false;
                    resultado.Error = string.Format(_localizer["PersonRegistration:VerificationNumberFail"]);
                    return resultado;
                }

                //Guardar persona en tabla intermedia
                var respuesta = JsonConvert.SerializeObject(personaInfo);

                var createPersonMdgDto = new CrearActualizarPersonaMdgDto
                {
                    NumeroRegistro = personaInfo.CodigoRegistro,
                    FechaNacimiento = personaInfo.FechaNacimiento,
                    PaisId = personaInfo.Nacionalidad,
                    Respuesta = respuesta,
                };

                await CreateAsync(createPersonMdgDto);

                //Obtener persona desde tabla intermedia
                personMdg = await _personMdgManager.ObtenerPersonaMdgPorNumeroRegistroAsync(numeroRegistro);
            }
           

            var personaMdgDto= ObjectMapper.Map<PersonaMdg, PersonaMdgDto>(personMdg);
            resultado.PersonaMdgDto = personaMdgDto;
            resultado.Success = true;

            return resultado;
        }

        public async Task<bool> StateCitizenIsRegisteredAsync(string registerNumber)
        {
            await CheckGetPolicyAsync();

            return await _personMdgManager.VerificarEstadoCiudadanoAsync(registerNumber);
        }

        public async Task<string> GetNationalityAsync(string registerNumber)
        {
            await CheckGetPolicyAsync();

            return await _personMdgManager.ObtenerNacionalidadAsync(registerNumber);
        }

        public async Task<DateTime> GetBirthDateAsync(string registerNumber)
        {
            await CheckGetPolicyAsync();

            return await _personMdgManager.ObtenerFechaNacimientoAsync(registerNumber);
        }

        public async Task<bool> PersonIsLockedAsync(string registerNumber)
        {
            await CheckGetPolicyAsync();

            return await _personMdgManager.VerificarBloqueoPersonaAsync(registerNumber);
        }

        public async Task<PersonaMdgDto> IncreaseAccessFailedAsync(string registerNumber)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckUpdatePolicyAsync();

                var entity = await _personMdgManager.IncrementarAccesoFallidoAsync(registerNumber);

                await Repository.UpdateAsync(entity, autoSave: true);

                await uow.CompleteAsync();
                return await GetAsync(entity.Id);
            }

        }

        public async Task UnlockUserAsync(string registerNumber)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckUpdatePolicyAsync();

                var entity = await _personMdgManager.DesbloquearUsuarioAsync(registerNumber);

                await Repository.UpdateAsync(entity, autoSave: true);

                await uow.CompleteAsync();
            }

        }

        public async Task<PersonaMdgDto> LockUserAsyn(string registerNumber, double lockDuration)
        {
            PersonaMdgDto personaMdg;
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await CheckUpdatePolicyAsync();

                var entity = await _personMdgManager.BloquearUsuarioAsync(registerNumber, lockDuration);

                await Repository.UpdateAsync(entity, autoSave: true);

                await uow.CompleteAsync();

                personaMdg = await GetAsync(entity.Id);
            }

            return personaMdg;
        }

        public async Task ChangeStateAsync(string registerNumber, EstadoPersonaMdg state)
        {
            await CheckUpdatePolicyAsync();

            var entity = await _personMdgManager.CambiarEstadoAsync(registerNumber, state);

            await Repository.UpdateAsync(entity, autoSave: true);
        }

        private async Task<InformacionPersonaDto> ObtenerDesdeServicio(string numeroRegistro)
        {
            // 2.Obtener información desde servicio de persona
            _logger.LogInformation("RegistroPersona - Inicia obtener informacion de persona desde servicio Mdg");

            InformacionPersonaDto personInformation = null;

            //Llamar al servicio con autenticacion
            _logger.LogInformation("RegistroPersona - Inicia consulta a servicio ServiceRegulacionDmz con usuario y clave configurados");
            personInformation =
            await _clientExternalDmzAppService.ObtenerInformacionPersona(numeroRegistro);

            _logger.LogInformation("RegistroPersona - Finalizado obtener informacion de persona desde servicio Mdg");

            return personInformation;
        }
    }
}
