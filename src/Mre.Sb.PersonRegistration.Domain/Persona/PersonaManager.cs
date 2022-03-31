using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Mre.Sb.RegistroPersona.Persona
{
    public class PersonaManager : DomainService
    {
        private readonly IRepository<Persona, Guid> _repository;

        public PersonaManager(IRepository<Persona, Guid> repository)
        {
            _repository = repository;
        }

        public virtual async Task<Persona> CreateAsync(
            string name, string firtsSurname, string secondSurname,
            DateTime birthDate, string birthCountryId, string emailAddress,
            bool hasIdentityDocument, string identityDocumentId, string gender,
            string maritalStatusId, string phoneNumber, string address,
            string regionId, string city, string visaId,
            string educationLevelId, string professionId, string occupationId,
            byte[] photograph, string permanenceRecordNumber, byte[] fingerPrint,
            DateTime countryEntryDate, bool regularPointAccess, string lastResidenceCountry,
            string origin, string originId, string userName,
            List<string> nationalities)
        {
            //1. Check if exist correoElectronico
            //var queryable = await repository.GetQueryableAsync();
            //var existing = queryable.AsEnumerable().Any(e => e.EmailAddress.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            //if (existing)
            //{
            //    throw new AbpException($"correoElectronico exist {nombre}");
            //}

            var entity = new Persona(GuidGenerator.Create(), nombre: name, primerApellido: firtsSurname,
                segundoApellido: secondSurname,
                fechaNacimiento: birthDate, paisNacimientoId: birthCountryId, correoElectronico: emailAddress,
                tieneDocumentoIdentidad: hasIdentityDocument, documentoIdentidadId: identityDocumentId, genero: gender,
                estadoCivilId: maritalStatusId, telefono: phoneNumber, direccion: address,
                regionId: regionId, ciudad: city, visaId: visaId,
                nivelEducativoId: educationLevelId, profesionId: professionId, ocupacionId: occupationId,
                fotografia: photograph, numeroRegistroPermanencia: permanenceRecordNumber, huellaDactilar: fingerPrint,
                fechaIngresoPais: countryEntryDate, puntoAccesoRegular: regularPointAccess,
                paisResidenciaPrevia: lastResidenceCountry,
                origen: origin, origenId: originId, nombreUsuario: userName,
                nacionalidades: nationalities);

            return await Task.FromResult(entity);
        }

        public virtual async Task<DocumentoIdentidad> CrearDocumentoIdentidadAsync(string tipoDocumentoIdentidadId,
            string numeroDocumento,
            string paisEmisionId,
            DateTime? fechaEmision,
            DateTime? fechaExpiracion,
            string origenId)
        {
            var entity = new DocumentoIdentidad(tipoDocumentoIdentidadId: tipoDocumentoIdentidadId,
                numeroDocumento: numeroDocumento,
                paisEmisionId: paisEmisionId,
                fechaEmision: fechaEmision,
                fechaExpiracion: fechaExpiracion,
                origenId: origenId);

            return await Task.FromResult(entity);
        }

        public virtual async Task<Visa> CrearVisaAsync(string numero,
            string tipoVisaId,
           DateTime? fechaEmision,
           DateTime? fechaExpiracion,
           string origenId)
        {
            var entity = new Visa(numero: numero,
                tipoVisaId: tipoVisaId,
                fechaEmision: fechaEmision,
                fechaExpiracion: fechaExpiracion,
                origenId: origenId);

            return await Task.FromResult(entity);
        }

    }
}