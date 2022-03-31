using AutoMapper;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using Mre.Sb.RegistroPersona.Proceso;
using Volo.Abp.AutoMapper;

namespace Mre.Sb.PersonRegistration
{
    public class PersonRegistrationApplicationAutoMapperProfile : Profile
    {
        public PersonRegistrationApplicationAutoMapperProfile()
        {
            CreateMap<Visa, VisaDto>();

            CreateMap<VisaDto, Visa>()
                .ForMember(x => x.TipoVisa, map => map.Ignore());


            CreateMap<TipoVisa, TipoVisaDto>();

            CreateMap<TipoVisaDto, TipoVisa>();


            CreateMap<Profesion, ProfesionDto>();

            CreateMap<ProfesionDto, Profesion>();


            CreateMap<Ocupacion, OcupacionDto>();

            CreateMap<OcupacionDto, Ocupacion>();


            CreateMap<EstadoCivil, EstadoCivilDto>();

            CreateMap<EstadoCivilDto, EstadoCivil>();


            CreateMap<TipoDocumentoIdentidad, TipoDocumentoIdentidadDto>();

            CreateMap<TipoDocumentoIdentidadDto, TipoDocumentoIdentidad>();


            CreateMap<NivelEducativo, NivelEducativoDto>();

            CreateMap<NivelEducativoDto, NivelEducativo>();


            CreateMap<DocumentoIdentidad, DocumentoIdentidadDto>();

            CreateMap<DocumentoIdentidadDto, DocumentoIdentidad>()
                .ForMember(x => x.TipoDocumentoIdentidad, map => map.Ignore());

            //CreateMap<CrearActualizarPersonaDto, Persona.Persona>()
            //    .ForMember(x => x.Id, map => map.Ignore())
            //    .ForMember(x => x.DocumentoIdentidad, map => map.Ignore())
            //    .ForMember(x => x.EstadoCivil, map => map.Ignore())
            //    .ForMember(x => x.Visa, map => map.Ignore())
            //    .ForMember(x => x.NivelEducativo, map => map.Ignore())
            //    .ForMember(x => x.Profesion, map => map.Ignore())
            //    .ForMember(x => x.Ocupacion, map => map.Ignore())
            //    .ForMember(x => x.Origen, map => map.Ignore())
            //    .ForMember(x => x.OrigenId, map => map.Ignore())
            //    .ForMember(x => x.NombreUsuario, map => map.Ignore())
            //    .ForMember(x => x.Nacionalidades, map => map.Ignore())
            //    .ForMember(x => x.IsDeleted, map => map.Ignore())
            //    .ForMember(x => x.DeleterId, map => map.Ignore())
            //    .ForMember(x => x.DeletionTime, map => map.Ignore())
            //    .IgnoreAuditedObjectProperties();

            CreateMap<PersonaMdg, PersonaMdgDto>();
            CreateMap<CrearActualizarPersonaMdgDto, PersonaMdg>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.Estado, map => map.Ignore())
                .ForMember(x => x.BloqueoHabilitado, map => map.Ignore())
                .ForMember(x => x.FinBloqueo, map => map.Ignore())
                .ForMember(x => x.ContadorAccesoFallido, map => map.Ignore())
                .ForMember(x => x.FinBloqueo, map => map.Ignore())
                .IgnoreAuditedObjectProperties();

            CreateMap<CodigoVerificacion, CodigoVerificacionDto>();
            CreateMap<CrearActualizarCodigoVerificacionDto, CodigoVerificacion>()
                .ForMember(x => x.Id, map => map.Ignore())
                .ForMember(x => x.Estado, map => map.Ignore())
                .IgnoreAuditedObjectProperties();

            CreateMap<RegistroTracking, RegistroTrackingDto>();
        }
    }
}