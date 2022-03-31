using Microsoft.Extensions.Localization;
using Mre.Sb.Geographical;
using Mre.Sb.PersonRegistration.Localization;
using Mre.Sb.RegistroPersona.Persona;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class PersonaMapeadorAppService : ApplicationService, IPersonaMapeadorAppService
    {
        private readonly CountryData countryData;
        private readonly IStringLocalizer<PersonRegistrationResource> localizer;

        public IEstadoCivilAppService MaritalStatusAppService { get; }
        public INivelEducativoAppService EducationLevelAppService { get; }
        public IProfesionAppService ProfessionAppService { get; }
        public IOcupacionAppService OccupationAppService { get; }
        public ITipoDocumentoIdentidadAppService IdentityDocumentTypeAppService { get; }
        public ITipoVisaAppService VisaTypeAppService { get; }
        

        public PersonaMapeadorAppService(
            IEstadoCivilAppService maritalStatusAppService,
            INivelEducativoAppService educationLevelAppService,
            IProfesionAppService professionAppService,
            IOcupacionAppService occupationAppService,
            ITipoDocumentoIdentidadAppService identityDocumentTypeAppService,
            ITipoVisaAppService visaTypeAppService,
            CountryData countryData,
            IStringLocalizer<PersonRegistrationResource> localizer)
        {
            MaritalStatusAppService = maritalStatusAppService;
            EducationLevelAppService = educationLevelAppService;
            ProfessionAppService = professionAppService;
            OccupationAppService = occupationAppService;
            IdentityDocumentTypeAppService = identityDocumentTypeAppService;
            VisaTypeAppService = visaTypeAppService;
            this.countryData = countryData;
            this.localizer = localizer;
        }

        public async Task<PersonaDto> MapeoPersonaDesdePersonaMdgAsync(InformacionPersonaDto personaInformacionDto) 
        {
            string base64String = Convert.ToBase64String(personaInformacionDto.Fotografia, 0, personaInformacionDto.Fotografia.Length);
            var photo = "data:image/png;base64," + base64String;

            base64String = Convert.ToBase64String(personaInformacionDto.HuellasDactilares, 0, personaInformacionDto.HuellasDactilares.Length);
            var fingerPrint = "data:image/png;base64," + base64String;

            var personaDto = new PersonaDto
            {
                Nombre = personaInformacionDto.Nombres,
                PrimerApellido = personaInformacionDto.PrimerApellido,
                SegundoApellido = personaInformacionDto.SegundoApellido,
                FechaNacimiento = personaInformacionDto.FechaNacimiento,
                CorreoElectronico = personaInformacionDto.CorreoElectronico,
                PoseeDocumentoIdentidad = personaInformacionDto.PoseeDocumentoIdentidad.ToUpper().Equals(RespuestaMdgConsts.SI),
                PoseeDocumentoIdentidadTexto = personaInformacionDto.PoseeDocumentoIdentidad.ToUpper().Equals(RespuestaMdgConsts.SI)
                    ? string.Format(localizer["PersonRegistration:Yes"])
                    : string.Format(localizer["PersonRegistration:No"]),
                NumeroDocumentoIdentidad = personaInformacionDto.NumeroDocumentoViaje,
                DocumentoIdentidadFechaEmision = personaInformacionDto.FechaEmisionDocumentoIdentidad,
                DocumentoIdentidadFechaExpiracion = personaInformacionDto.FechaExpiracionDocumentoIdentidad,
                DocumentoIdentidadPaisEmision = personaInformacionDto.PaisEmisionDocumentoIdentidad,
                Genero = personaInformacionDto.Genero,
                Telefono = personaInformacionDto.Telefono,
                Direccion = personaInformacionDto.Direccion,
                Ciudad = personaInformacionDto.Ciudad,
                FotografiaBase64 = photo,
                NumeroRegistroPermanencia = personaInformacionDto.NumeroRegistroPermanencia,
                HuellasDactilaresBase64 = fingerPrint,
                FechaIngresoPais = personaInformacionDto.FechaIngresoPais,
                IngresoPuntoRegular = personaInformacionDto.IngresoPorPuntoRegular.ToUpper().Equals(RespuestaMdgConsts.SI),
                IngresoPuntoRegularTexto = personaInformacionDto.IngresoPorPuntoRegular.ToUpper().Equals(RespuestaMdgConsts.SI)
                    ? string.Format(localizer["PersonRegistration:Yes"])
                    : string.Format(localizer["PersonRegistration:No"]),
                PaisResidenciaPrevia = personaInformacionDto.PaisResidenciaPrevia,
                NumeroVisa = personaInformacionDto.NumeroVisa,
                VisaFechaEmision = personaInformacionDto.FechaEmisionVisa,
                VisaFechaExpiracion = personaInformacionDto.FechaExpiracionVisa,
                Origen = PersonMdgConsts.SERVICIO_MDG,
                OrigenId = personaInformacionDto.CodigoRegistro
            };

            //Mapear datos de ubicación
            var paisNacimiento = MapeoPais(personaInformacionDto.PaisNacimiento);
            if (paisNacimiento == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "PaisNacimiento", personaInformacionDto.PaisNacimiento));
            }
            personaDto.PaisNacimientoId = paisNacimiento.Code;
            personaDto.PaisNacimiento = paisNacimiento.Name;

            var paisResidenciaPrevia = MapeoPais(personaInformacionDto.PaisResidenciaPrevia);
            if (paisResidenciaPrevia == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "PaisResidenciaPrevia", personaInformacionDto.PaisResidenciaPrevia));
            }
            personaDto.PaisResidenciaPreviaTexto = paisResidenciaPrevia.Name;

            var region = MapeoRegionPorCodigoMapeo(personaInformacionDto.Provincia);
            if (region == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "Region", personaInformacionDto.Provincia));
            }
            personaDto.RegionId = region.Code;
            personaDto.Region = region.Name;

            var paisEmisionDocumento= MapeoPais(personaInformacionDto.PaisEmisionDocumentoIdentidad);
            if (paisEmisionDocumento == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "PaisEmisionDocumento", personaInformacionDto.PaisEmisionDocumentoIdentidad));
            }
            personaDto.DocumentoIdentidadPaisEmisionNombre = paisEmisionDocumento.Name;

            //Agregar nacionalidades
            if (!string.IsNullOrEmpty(personaInformacionDto.Nacionalidad))
            {
                var pais = MapeoPais(personaInformacionDto.Nacionalidad);
                if (pais == null)
                {
                    throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "Nacionalidad", personaInformacionDto.Nacionalidad));
                }
                personaDto.NacionalidadesId.Add(pais.Code);
                personaDto.NacionalidadesNombre.Add(pais.Name);
            }

            if (!string.IsNullOrEmpty(personaInformacionDto.OtraNacionalidad))
            {
                var pais = MapeoPais(personaInformacionDto.OtraNacionalidad);
                if (pais == null)
                {
                    throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], "Nacionalidad", personaInformacionDto.OtraNacionalidad));
                }
                personaDto.NacionalidadesId.Add(pais.Code);
                personaDto.NacionalidadesNombre.Add(pais.Name);
            }

            var estadoCivil = await MapeoEstadoCivil(personaInformacionDto);
            if(estadoCivil == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(EstadoCivil), personaInformacionDto.EstadoCivil));
            }

            personaDto.EstadoCivilId = estadoCivil.Id;
            personaDto.EstadoCivil = estadoCivil.Nombre;

            var nivelEducativo = await MapeoNivelEducativo(personaInformacionDto);
            if (nivelEducativo == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(NivelEducativo), personaInformacionDto.NivelEducativo));
            }

            personaDto.NivelEducativoId = nivelEducativo.Id;
            personaDto.NivelEducativo = nivelEducativo.Nombre;

            var profesion = await MapeoProfesion(personaInformacionDto);
            if (profesion == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(Profesion), personaInformacionDto.Profesion));
            }

            personaDto.ProfesionId = profesion.Id;
            personaDto.Profesion = profesion.Nombre;

            var ocupacion = await MapeoOcupacion(personaInformacionDto);
            if (ocupacion == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(Ocupacion), personaInformacionDto.Ocupacion));
            }

            personaDto.OcupacionId = ocupacion.Id;
            personaDto.Ocupacion = ocupacion.Nombre;

            var tipoDocumentoIdentidad = await MapeoTipoDocumentoIdentidad(personaInformacionDto);
            if (tipoDocumentoIdentidad == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(TipoDocumentoIdentidad), personaInformacionDto.TipoDocumentoIdentidad));
            }

            personaDto.TipoDocumentoIdentidadId = tipoDocumentoIdentidad.Id;
            personaDto.TipoDocumentoIdentidad = tipoDocumentoIdentidad.Nombre;

            var tipoVisa = await MapeoTipoVisa(personaInformacionDto);
            if (tipoVisa == null)
            {
                throw new UserFriendlyException(string.Format(localizer["PersonRegistration:ErrorMapeo"], typeof(TipoVisa), personaInformacionDto.TipoVisa));
            }

            personaDto.TipoVisaId = tipoVisa.Id;
            personaDto.TipoVisa = tipoVisa.Nombre;

            return personaDto;
        }

        public async Task<CrearActualizarPersonaDto> MapeoPersonaAsync(InformacionPersonaDto personInformationDto)
        {
            var entityCreateUpdateDto = new CrearActualizarPersonaDto
            {
                Nombre = personInformationDto.Nombres,
                PrimerApellido = personInformationDto.PrimerApellido,
                SegundoApellido = personInformationDto.SegundoApellido,
                FechaNacimiento = personInformationDto.FechaNacimiento,
                CorreoElectronico = personInformationDto.CorreoElectronico,
                TieneDocumentoIdentidad = personInformationDto.PoseeDocumentoIdentidad.ToUpper().Equals(RespuestaMdgConsts.SI),
                NumeroDocumentoIdentidad = personInformationDto.NumeroDocumentoViaje,
                Genero = personInformationDto.Genero,
                Telefono = personInformationDto.Telefono,
                Direccion = personInformationDto.Direccion,
                Ciudad = personInformationDto.Ciudad,
                Fotografia = personInformationDto.Fotografia,
                NumeroRegistroPermanencia = personInformationDto.NumeroRegistroPermanencia,
                HuellaDactilar = personInformationDto.HuellasDactilares,
                FechaIngresoPais = personInformationDto.FechaIngresoPais,
                PuntoAccesoRegular = personInformationDto.IngresoPorPuntoRegular.ToUpper().Equals(RespuestaMdgConsts.SI),
                PaisResidenciaPrevia = personInformationDto.PaisResidenciaPrevia,
                FechaEmisionVisa = personInformationDto.FechaEmisionVisa,
                FechaExpiracionVisa = personInformationDto.FechaExpiracionVisa,
                NumeroVisa = personInformationDto.NumeroVisa,
                PaisEmisionDocumentoIdentidad = personInformationDto.PaisEmisionDocumentoIdentidad,
                FechaEmisionDocumentoIdentidad = personInformationDto.FechaEmisionDocumentoIdentidad,
                FechaExpiracionDocumentoIdentidad = personInformationDto.FechaExpiracionDocumentoIdentidad,
                NombreUsuario = personInformationDto.CorreoElectronico,
                Origen = PersonMdgConsts.SERVICIO_MDG,
                OrigenId = personInformationDto.CodigoRegistro.ToString()
            };

            //Mapear datos de ubicación
            entityCreateUpdateDto.PaisNacimientoId = MapeoPais(personInformationDto.PaisNacimiento).Code;
            entityCreateUpdateDto.RegionId = MapeoRegionPorCodigoMapeo(personInformationDto.Provincia).Code;

            //Agregar nacionalidades
            if (!string.IsNullOrEmpty(personInformationDto.Nacionalidad))
            {
                entityCreateUpdateDto.Nacionalidades.Add(MapeoPais(personInformationDto.Nacionalidad).Code);
            }

            if (!string.IsNullOrEmpty(personInformationDto.OtraNacionalidad))
            {
                entityCreateUpdateDto.Nacionalidades.Add(MapeoPais(personInformationDto.OtraNacionalidad).Code);
            }

            var estadoCivil = await MapeoEstadoCivil(personInformationDto);
            entityCreateUpdateDto.EstadoCivilId = estadoCivil.Id;

            var nivelEducativo = await MapeoNivelEducativo(personInformationDto);
            entityCreateUpdateDto.NivelEducativoId = nivelEducativo.Id;

            var profesion = await MapeoProfesion(personInformationDto);
            entityCreateUpdateDto.ProfesionId = profesion.Id;

            var ocupacion = await MapeoOcupacion(personInformationDto);
            entityCreateUpdateDto.OcupacionId = ocupacion.Id;

            var tipoDocumentoIdentidad = await MapeoTipoDocumentoIdentidad(personInformationDto);
            entityCreateUpdateDto.TipoDocumentoIdentidadId = tipoDocumentoIdentidad.Id;

            var tipoVisa = await MapeoTipoVisa(personInformationDto);
            entityCreateUpdateDto.TipoVisaId = tipoVisa.Id;

            return entityCreateUpdateDto;
        }

        public async Task<PersonaDto> MapeoPersonaAsync(PersonaDto personaDto) 
        {
            string base64String = Convert.ToBase64String(personaDto.Fotografia, 0, personaDto.Fotografia.Length);
            var photo = "data:image/png;base64," + base64String;

            base64String = Convert.ToBase64String(personaDto.HuellasDactilares, 0, personaDto.HuellasDactilares.Length);
            var fingerPrint = "data:image/png;base64," + base64String;

            personaDto.FotografiaBase64 = photo;
            personaDto.HuellasDactilaresBase64 = fingerPrint;


            personaDto.IngresoPuntoRegularTexto = personaDto.IngresoPuntoRegular
                ? string.Format(localizer["PersonRegistration:Yes"])
                : string.Format(localizer["PersonRegistration:No"]);

            personaDto.PoseeDocumentoIdentidadTexto = personaDto.PoseeDocumentoIdentidad
                ? string.Format(localizer["PersonRegistration:Yes"])
                : string.Format(localizer["PersonRegistration:No"]);

            personaDto.DocumentoIdentidadPaisEmisionNombre = MapeoPais(personaDto.DocumentoIdentidadPaisEmision).Name;

            personaDto.PaisNacimiento = MapeoPais(personaDto.PaisNacimientoId).Name;

            foreach (var item in personaDto.NacionalidadesId)
            {
                var nacionalidad = MapeoPais(item);
                personaDto.NacionalidadesNombre.Add(nacionalidad.Name);
            }

            personaDto.Region = MapeoRegionPorCodigo(personaDto.RegionId).Name;

            return personaDto;
        }

        
        #region metodos soporte

        private async Task<EstadoCivilDto> MapeoEstadoCivil(InformacionPersonaDto personInformationDto)
        {
            var estadoCivil = await MaritalStatusAppService.BuscarPorCodigoMapeoAsync(personInformationDto.EstadoCivil);
            
            return estadoCivil;
        }

        private async Task<NivelEducativoDto> MapeoNivelEducativo(InformacionPersonaDto personInformationDto)
        {
            var nivelEducativo = await EducationLevelAppService.BuscarPorCodigoMapeoAsync(personInformationDto.NivelEducativo);

            return nivelEducativo;
        }

        private async Task<ProfesionDto> MapeoProfesion(InformacionPersonaDto personInformationDto)
        {
            var profesion = await ProfessionAppService.BuscarPorCodigoMapeoAsync(personInformationDto.Profesion);

            return profesion;
        }

        private async Task<OcupacionDto> MapeoOcupacion(InformacionPersonaDto personInformationDto)
        {
            var ocupacion = await OccupationAppService.BuscarPorCodigoMapeoAsync(personInformationDto.Ocupacion);

            return ocupacion;
        }

        private async Task<TipoDocumentoIdentidadDto> MapeoTipoDocumentoIdentidad(InformacionPersonaDto personInformationDto)
        {
            var tipoDocumentoIdentidad = await IdentityDocumentTypeAppService.BuscarPorCodigoMapeoAsync(personInformationDto.TipoDocumentoIdentidad);

            return tipoDocumentoIdentidad;
        }

        private async Task<TipoVisaDto> MapeoTipoVisa(InformacionPersonaDto personInformationDto)
        {
            var tipoVisa = await VisaTypeAppService.BuscarPorCodigoMapeoAsync(personInformationDto.TipoVisa);

            return tipoVisa;
        }

        private Region MapeoRegionPorCodigo(string codigo)
        {
            var region = countryData.GetRegion("ECU", codigo.ToUpper());

            return region;
        }

        private Region MapeoRegionPorCodigoMapeo(string codigo)
        {
            var region = countryData.GetRegionByMappingCode("ECU", codigo.ToUpper());

            return region;
        }

        private Country MapeoPais(string codigo)
        {
            var pais = countryData.GetCountry(codigo);

            return pais;
        }

        #endregion metodos soporte

    }
}
