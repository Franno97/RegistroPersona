using Microsoft.Extensions.Localization;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.RegistroPersona.Proceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonaAppService :
        CrudAppService<
            Persona,
            PersonaDto,
            Guid,
            ObtenerPersonaInputDto,
            CrearActualizarPersonaDto>,
        IPersonaAppService
    {
        public PersonaManager PersonaManager { get; }

        //private readonly IDocumentoIdentidadAppService _identityDocumentService;
        //private readonly IVisaAppService _visaAppService;
        private readonly IPersonaMapeadorAppService _personaMapeadorAppService;
        private readonly IRepository<Visa> _visaRepository;
        private readonly IRepository<DocumentoIdentidad> _documentoRepository;
        private readonly ICurrentUser _currentUser;

        public PersonaAppService(IRepository<Persona, Guid> repository,
            PersonaManager personaManager,
            //IDocumentoIdentidadAppService identityDocumentService,
            //IVisaAppService visaAppService,
            IPersonaMapeadorAppService personaMapeadorAppService,
            IRepository<DocumentoIdentidad> documentoRepository,
            IRepository<Visa> visaRepository,
            ICurrentUser currentUser)
            : base(repository)
        {
            PersonaManager = personaManager;
            //_identityDocumentService = identityDocumentService;
            //_visaAppService = visaAppService;
            _personaMapeadorAppService = personaMapeadorAppService;
            _visaRepository = visaRepository;
            _documentoRepository = documentoRepository;
            _currentUser = currentUser;
        }

        public override async Task<PersonaDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(a => a.Id == id);

            var queryableDto = queryable.Select(entity => new PersonaDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                PrimerApellido = entity.PrimerApellido,
                SegundoApellido = entity.SegundoApellido,
                FechaNacimiento = entity.FechaNacimiento,
                PaisNacimientoId = entity.PaisNacimientoId,
                CorreoElectronico = entity.CorreoElectronico,
                PoseeDocumentoIdentidad = entity.TieneDocumentoIdentidad,
                Genero = entity.Genero,
                EstadoCivilId = entity.EstadoCivilId,
                EstadoCivil = entity.EstadoCivil.Nombre,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,
                RegionId = entity.RegionId,
                Ciudad = entity.Ciudad,
                NivelEducativoId = entity.NivelEducativoId,
                NivelEducativo = entity.NivelEducativo.Nombre,
                ProfesionId = entity.ProfesionId,
                Profesion = entity.Profesion.Nombre,
                OcupacionId = entity.OcupacionId,
                Ocupacion = entity.Ocupacion.Nombre,
                Fotografia = entity.Fotografia,
                FechaIngresoPais = entity.FechaIngresoPais,
                NumeroRegistroPermanencia = entity.NumeroRegistroPermanencia,
                IngresoPuntoRegular = entity.PuntoAccesoRegular,
                PaisResidenciaPrevia = entity.PaisResidenciaPrevia,
                HuellasDactilares = entity.HuellaDactilar,
                NombreUsuario = entity.NombreUsuario,
                Origen = entity.Origen,
                OrigenId = entity.OrigenId
            });

            var entityDto = await AsyncExecuter.SingleOrDefaultAsync(queryableDto);

            if(entityDto != null)
            {
                //Obtener documento identidad
                var queryDocumentoIdentidad = await _documentoRepository.GetQueryableAsync();
                queryDocumentoIdentidad = queryDocumentoIdentidad.Where(x => x.OrigenId == entityDto.OrigenId);

                var queryDocumentoDto = queryDocumentoIdentidad.Select( entity => new DocumentoIdentidadDto()
                {
                    NumeroDocumento = entity.NumeroDocumento,
                    FechaEmision = entity.FechaEmision,
                    FechaExpiracion = entity.FechaExpiracion,
                    PaisEmisionId = entity.PaisEmisionId,
                    TipoDocumentoIdentidadId = entity.TipoDocumentoIdentidadId,
                    TipoDocumentoIdentidad = entity.TipoDocumentoIdentidad.Nombre,
                    OrigenId = entity.OrigenId
                });

                var documentoDto = await AsyncExecuter.SingleOrDefaultAsync(queryDocumentoDto);

                if(documentoDto != null)
                {
                    entityDto.NumeroDocumentoIdentidad = documentoDto.NumeroDocumento;
                    entityDto.DocumentoIdentidadFechaEmision = documentoDto.FechaEmision;
                    entityDto.DocumentoIdentidadFechaExpiracion = documentoDto.FechaExpiracion;
                    entityDto.DocumentoIdentidadPaisEmision = documentoDto.PaisEmisionId;
                    entityDto.TipoDocumentoIdentidadId = documentoDto.TipoDocumentoIdentidadId;
                    entityDto.TipoDocumentoIdentidad = documentoDto.TipoDocumentoIdentidad;
                }

                //Obtener visa
                var queryVisa = await _visaRepository.GetQueryableAsync();
                queryVisa = queryVisa.Where(x => x.OrigenId == entityDto.OrigenId);

                var queryVisaDto = queryVisa.Select(entity => new VisaDto()
                {
                    Numero = entity.Numero,
                    FechaEmision = entity.FechaEmision,
                    FechaExpiracion = entity.FechaExpiracion,
                    TipoVisaId = entity.TipoVisaId,
                    TipoVisa = entity.TipoVisa.Nombre,
                    OrigenId = entity.OrigenId
                });

                var visaDto = await AsyncExecuter.SingleOrDefaultAsync(queryVisaDto);

                if(visaDto != null)
                {
                    entityDto.NumeroVisa = visaDto.Numero;
                    entityDto.TipoVisaId = visaDto.TipoVisaId;
                    entityDto.TipoVisa = visaDto.TipoVisa;
                    entityDto.VisaFechaEmision = visaDto.FechaEmision;
                    entityDto.VisaFechaExpiracion = visaDto.FechaExpiracion;
                }
                

                //Obtener listados
                var consultableNacionalidades = await Repository.GetQueryableAsync();
                consultableNacionalidades = consultableNacionalidades.Where(p => p.Id == entityDto.Id);

                var consultableNacionalidadesDto = consultableNacionalidades.SelectMany(entity => entity.Nacionalidades)
                        .Select(n => n.PaisId);

                entityDto.NacionalidadesId = await AsyncExecuter.ToListAsync(consultableNacionalidadesDto); 
                
            }

            return await _personaMapeadorAppService.MapeoPersonaAsync(entityDto);
        }

        public override async Task<PagedResultDto<PersonaDto>> GetListAsync(ObtenerPersonaInputDto input)
        {
            await CheckGetListPolicyAsync();

            var queryable = await CreateFilteredQueryAsync(input);

            queryable = queryable.WhereIf(
                !input.Filtro.IsNullOrWhiteSpace(),
                u =>
                    u.Nombre.Contains(input.Filtro)
            );

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = ApplySorting(queryable, input);
            queryable = ApplyPaging(queryable, input);


            var queryableDto = queryable.Select(entity => new PersonaDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                PrimerApellido = entity.PrimerApellido,
                SegundoApellido = entity.SegundoApellido,
                FechaNacimiento = entity.FechaNacimiento,
                PaisNacimientoId = entity.PaisNacimientoId,
                CorreoElectronico = entity.CorreoElectronico,
                PoseeDocumentoIdentidad = entity.TieneDocumentoIdentidad,
                Genero = entity.Genero,
                EstadoCivilId = entity.EstadoCivilId,
                EstadoCivil = entity.EstadoCivil.Nombre,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,
                RegionId = entity.RegionId,
                Ciudad = entity.Ciudad,
                NivelEducativoId = entity.NivelEducativoId,
                NivelEducativo = entity.NivelEducativo.Nombre,
                ProfesionId = entity.ProfesionId,
                Profesion = entity.Profesion.Nombre,
                OcupacionId = entity.OcupacionId,
                Ocupacion = entity.Ocupacion.Nombre,
                Fotografia = entity.Fotografia,
                FechaIngresoPais = entity.FechaIngresoPais,
                NumeroRegistroPermanencia = entity.NumeroRegistroPermanencia,
                IngresoPuntoRegular = entity.PuntoAccesoRegular,
                PaisResidenciaPrevia = entity.PaisResidenciaPrevia,
                HuellasDactilares = entity.HuellaDactilar,
                NombreUsuario = entity.NombreUsuario,
                Origen = entity.Origen,
                OrigenId = entity.OrigenId
            });

            var entityDtos = await AsyncExecuter.ToListAsync(queryableDto);


            if (entityDtos.Any())
            {
                foreach (var entityDto in entityDtos)
                {
                    //Obtener documento identidad
                    var queryDocumentoIdentidad = await _documentoRepository.GetQueryableAsync();
                    queryDocumentoIdentidad = queryDocumentoIdentidad.Where(x => x.OrigenId == entityDto.OrigenId);

                    var queryDocumentoDto = queryDocumentoIdentidad.Select(entity => new DocumentoIdentidadDto()
                    {
                        NumeroDocumento = entity.NumeroDocumento,
                        FechaEmision = entity.FechaEmision,
                        FechaExpiracion = entity.FechaExpiracion,
                        PaisEmisionId = entity.PaisEmisionId,
                        TipoDocumentoIdentidadId = entity.TipoDocumentoIdentidadId,
                        TipoDocumentoIdentidad = entity.TipoDocumentoIdentidad.Nombre,
                        OrigenId = entity.OrigenId
                    });

                    var documentoDto = await AsyncExecuter.SingleOrDefaultAsync(queryDocumentoDto);

                    if (documentoDto != null)
                    {
                        entityDto.NumeroDocumentoIdentidad = documentoDto.NumeroDocumento;
                        entityDto.DocumentoIdentidadFechaEmision = documentoDto.FechaEmision;
                        entityDto.DocumentoIdentidadFechaExpiracion = documentoDto.FechaExpiracion;
                        entityDto.DocumentoIdentidadPaisEmision = documentoDto.PaisEmisionId;
                        entityDto.TipoDocumentoIdentidadId = documentoDto.TipoDocumentoIdentidadId;
                        entityDto.TipoDocumentoIdentidad = documentoDto.TipoDocumentoIdentidad;
                    }

                    //Obtener visa
                    var queryVisa = await _visaRepository.GetQueryableAsync();
                    queryVisa = queryVisa.Where(x => x.OrigenId == entityDto.OrigenId);

                    var queryVisaDto = queryVisa.Select(entity => new VisaDto()
                    {
                        Numero = entity.Numero,
                        FechaEmision = entity.FechaEmision,
                        FechaExpiracion = entity.FechaExpiracion,
                        TipoVisaId = entity.TipoVisaId,
                        TipoVisa = entity.TipoVisa.Nombre,
                        OrigenId = entity.OrigenId
                    });

                    var visaDto = await AsyncExecuter.SingleOrDefaultAsync(queryVisaDto);

                    if (visaDto != null)
                    {
                        entityDto.NumeroVisa = visaDto.Numero;
                        entityDto.TipoVisaId = visaDto.TipoVisaId;
                        entityDto.TipoVisa = visaDto.TipoVisa;
                        entityDto.VisaFechaEmision = visaDto.FechaEmision;
                        entityDto.VisaFechaExpiracion = visaDto.FechaExpiracion;
                    }

                    //Obtener listados
                    var consultableNacionalidades = await Repository.GetQueryableAsync();
                    consultableNacionalidades = consultableNacionalidades.Where(p => p.Id == entityDto.Id);

                    var consultableNacionalidadesDto = consultableNacionalidades.SelectMany(entity => entity.Nacionalidades)
                            .Select(n => n.PaisId);

                    entityDto.NacionalidadesId = await AsyncExecuter.ToListAsync(consultableNacionalidadesDto);
                }
            }

            return new PagedResultDto<PersonaDto>(
                totalCount,
                entityDtos
            );
        }

        public override async Task<PersonaDto> CreateAsync(CrearActualizarPersonaDto input)
        {
            await CheckCreatePolicyAsync();

            Visa visa = null;

            if (!string.IsNullOrEmpty(input.NumeroVisa) && !string.IsNullOrEmpty(input.TipoVisaId))
            {
                visa = await PersonaManager.CrearVisaAsync(input.NumeroVisa,
                                            input.TipoVisaId,
                                            input.FechaEmisionVisa,
                                            input.FechaExpiracionVisa,
                                            input.OrigenId);

                await _visaRepository.InsertAsync(visa);
            }


            DocumentoIdentidad identityDocument = null;
            if (input.TieneDocumentoIdentidad)
            {
                identityDocument = await PersonaManager.CrearDocumentoIdentidadAsync(input.TipoDocumentoIdentidadId,
                                            input.NumeroDocumentoIdentidad,
                                            input.PaisEmisionDocumentoIdentidad,
                                            input.FechaEmisionDocumentoIdentidad,
                                            input.FechaExpiracionDocumentoIdentidad,
                                            input.OrigenId);

                await _documentoRepository.InsertAsync(identityDocument);
            }

            var entity = await PersonaManager.CreateAsync(
                name: input.Nombre, firtsSurname: input.PrimerApellido, secondSurname: input.SegundoApellido,
                birthDate: input.FechaNacimiento, birthCountryId: input.PaisNacimientoId, 
                emailAddress: input.CorreoElectronico,
                hasIdentityDocument: input.TieneDocumentoIdentidad,
                identityDocumentId: identityDocument != null ? identityDocument.OrigenId : "",
                gender: input.Genero,
                maritalStatusId: input.EstadoCivilId, 
                phoneNumber: input.Telefono, 
                address: input.Direccion,
                regionId: input.RegionId, 
                city: input.Ciudad, 
                visaId: visa != null ? visa.OrigenId : "",
                educationLevelId: input.NivelEducativoId, 
                professionId: input.ProfesionId,
                occupationId: input.OcupacionId,
                photograph: input.Fotografia, 
                permanenceRecordNumber: input.NumeroRegistroPermanencia,
                fingerPrint: input.HuellaDactilar,
                countryEntryDate: input.FechaIngresoPais, 
                regularPointAccess: input.PuntoAccesoRegular,
                lastResidenceCountry: input.PaisResidenciaPrevia,
                originId: input.OrigenId, origin: input.Origen, 
                userName: input.NombreUsuario,
                nationalities: input.Nacionalidades);


            TryToSetTenantId(entity);


            await Repository.InsertAsync(entity, autoSave: true);


            return await GetAsync(entity.Id);
        }

        public override async Task<PersonaDto> UpdateAsync(Guid id, CrearActualizarPersonaDto input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            return await GetAsync(entity.Id);
        }

        public Task CitizenAlreadyExists(string registrationCode)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonaDto> ObtenerPersonaPorNombreUsuarioAsync(string nombreUsuario)
        {
            await CheckGetPolicyAsync();

            return await ObtenerInformacionPersona(nombreUsuario);
        }


        public async Task<PersonaDto> ObtenerPersonaActualAsync()
        {
            await CheckGetPolicyAsync();

            //Obtener el usuario actual
            var nombreUsuario = _currentUser.Email;
            if(string.IsNullOrEmpty(nombreUsuario))
            {
                return null;
            }
            return await ObtenerInformacionPersona(nombreUsuario);
        }

        #region metodos soporte

        private async Task<PersonaDto> ObtenerInformacionPersona(String nombreUsuario)
        {
            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.Where(u => u.NombreUsuario == nombreUsuario);

            var queryableDto = queryable.Select(entity => new PersonaDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                PrimerApellido = entity.PrimerApellido,
                SegundoApellido = entity.SegundoApellido,
                FechaNacimiento = entity.FechaNacimiento,
                PaisNacimientoId = entity.PaisNacimientoId,
                CorreoElectronico = entity.CorreoElectronico,
                PoseeDocumentoIdentidad = entity.TieneDocumentoIdentidad,
                Genero = entity.Genero,
                EstadoCivilId = entity.EstadoCivilId,
                EstadoCivil = entity.EstadoCivil.Nombre,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,
                RegionId = entity.RegionId,
                Ciudad = entity.Ciudad,
                NivelEducativoId = entity.NivelEducativoId,
                NivelEducativo = entity.NivelEducativo.Nombre,
                ProfesionId = entity.ProfesionId,
                Profesion = entity.Profesion.Nombre,
                OcupacionId = entity.OcupacionId,
                Ocupacion = entity.Ocupacion.Nombre,
                Fotografia = entity.Fotografia,
                FechaIngresoPais = entity.FechaIngresoPais,
                NumeroRegistroPermanencia = entity.NumeroRegistroPermanencia,
                IngresoPuntoRegular = entity.PuntoAccesoRegular,
                PaisResidenciaPrevia = entity.PaisResidenciaPrevia,
                HuellasDactilares = entity.HuellaDactilar,
                NombreUsuario = entity.NombreUsuario,
                Origen = entity.Origen,
                OrigenId = entity.OrigenId
        });

            var entityDto = await AsyncExecuter.FirstOrDefaultAsync(queryableDto);

            if (entityDto != null)
            {
                //Obtener documento identidad
                var queryDocumentoIdentidad = await _documentoRepository.GetQueryableAsync();
                queryDocumentoIdentidad = queryDocumentoIdentidad.Where(x => x.OrigenId == entityDto.OrigenId);

                var queryDocumentoDto = queryDocumentoIdentidad.Select(entity => new DocumentoIdentidadDto()
                {
                    NumeroDocumento = entity.NumeroDocumento,
                    FechaEmision = entity.FechaEmision,
                    FechaExpiracion = entity.FechaExpiracion,
                    PaisEmisionId = entity.PaisEmisionId,
                    TipoDocumentoIdentidadId = entity.TipoDocumentoIdentidadId,
                    TipoDocumentoIdentidad = entity.TipoDocumentoIdentidad.Nombre,
                    OrigenId = entity.OrigenId
                });

                var documentoDto = await AsyncExecuter.SingleOrDefaultAsync(queryDocumentoDto);

                if (documentoDto != null)
                {
                    entityDto.NumeroDocumentoIdentidad = documentoDto.NumeroDocumento;
                    entityDto.DocumentoIdentidadFechaEmision = documentoDto.FechaEmision;
                    entityDto.DocumentoIdentidadFechaExpiracion = documentoDto.FechaExpiracion;
                    entityDto.DocumentoIdentidadPaisEmision = documentoDto.PaisEmisionId;
                    entityDto.TipoDocumentoIdentidadId = documentoDto.TipoDocumentoIdentidadId;
                    entityDto.TipoDocumentoIdentidad = documentoDto.TipoDocumentoIdentidad;
                }

                //Obtener visa
                var queryVisa = await _visaRepository.GetQueryableAsync();
                queryVisa = queryVisa.Where(x => x.OrigenId == entityDto.OrigenId);

                var queryVisaDto = queryVisa.Select(entity => new VisaDto()
                {
                    Numero = entity.Numero,
                    FechaEmision = entity.FechaEmision,
                    FechaExpiracion = entity.FechaExpiracion,
                    TipoVisaId = entity.TipoVisaId,
                    TipoVisa = entity.TipoVisa.Nombre,
                    OrigenId = entity.OrigenId
                });

                var visaDto = await AsyncExecuter.SingleOrDefaultAsync(queryVisaDto);

                if (visaDto != null)
                {
                    entityDto.NumeroVisa = visaDto.Numero;
                    entityDto.TipoVisaId = visaDto.TipoVisaId;
                    entityDto.TipoVisa = visaDto.TipoVisa;
                    entityDto.VisaFechaEmision = visaDto.FechaEmision;
                    entityDto.VisaFechaExpiracion = visaDto.FechaExpiracion;
                }

                //Obtener listados
                var consultableNacionalidades = await Repository.GetQueryableAsync();
                consultableNacionalidades = consultableNacionalidades.Where(p => p.Id == entityDto.Id);

                var consultableNacionalidadesDto = consultableNacionalidades.SelectMany(entity => entity.Nacionalidades)
                        .Select(n => n.PaisId);

                entityDto.NacionalidadesId = await AsyncExecuter.ToListAsync(consultableNacionalidadesDto);

            }

            return await _personaMapeadorAppService.MapeoPersonaAsync(entityDto);
        }

        #endregion metodos soporte

    }
}