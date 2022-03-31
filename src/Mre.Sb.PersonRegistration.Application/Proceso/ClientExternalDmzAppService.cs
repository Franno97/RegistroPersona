using System;
using System.Collections.Generic;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.PersonRegistration.Settings;
using MreSecuredServiceReference;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class ClientExternalDmzAppService : ApplicationService, IClientExternalDmzAppService, ITransientDependency
    {

        private readonly ServiceRegulacionDmzClient _serviceRegulacionDmz;
        private readonly IConfiguration _config;

        public ClientExternalDmzAppService(
            IConfiguration config)
        {
            _config = config;

            _serviceRegulacionDmz = new ServiceRegulacionDmzClient();

            _serviceRegulacionDmz.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            _serviceRegulacionDmz.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;
            _serviceRegulacionDmz.ClientCredentials.UserName.UserName = _config.GetValue<string>("MdgService:Username");
            _serviceRegulacionDmz.ClientCredentials.UserName.Password = _config.GetValue<string>("MdgService:Password");
            _serviceRegulacionDmz.Endpoint.Address = new System.ServiceModel.EndpointAddress(config.GetValue<string>("MdgService:HttpsEndpoint"));
        }

        public async Task<InformacionPersonaDto> ObtenerInformacionPersona(string numeroRegistro)
        {
            var regulacionExtranjerosModel = await _serviceRegulacionDmz.ConsultaRegulacionExtranjerosAsync(codigoRegistro: Convert.ToInt32(numeroRegistro));

            if (regulacionExtranjerosModel == null || regulacionExtranjerosModel.CodigoRegistro == 0)
            {
                return null;
            }

            var nationalities = new List<string> {
                regulacionExtranjerosModel.Nacionalidad
            };

            if (!string.IsNullOrEmpty(regulacionExtranjerosModel.SeleccioneOtraNacionalidad))
            {
                nationalities.Add(regulacionExtranjerosModel.SeleccioneOtraNacionalidad);
            }

            var personInformationDto = MapearRespuesta(regulacionExtranjerosModel);

            return personInformationDto;
        }

        public async Task<ConsultarPuntoControlOutput> ConsultarPuntoControlAsync(string numeroRegistro)
        {
            //1. Leer configuracion
            var fechaInicial = DateTime.Parse(await SettingProvider.GetOrNullAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlInicial));
            var fechaFinal = DateTime.Parse(await SettingProvider.GetOrNullAsync(PersonRegistrationSettings.FaseRegulacion.FechaControlFinal));


            var resultado = await _serviceRegulacionDmz.ConsultaPuntoControlAsync(
                codigoRegistro: Convert.ToInt32(numeroRegistro),
                fechaControlInicial: fechaInicial,
                fechaControlFinal: fechaFinal
                );

            return ObtenerRespuestaPuntoControl(resultado);
        }

        public async Task<ConsultarMultasRespuesta> ConsultarMultasAsync(string numeroRegistro)
        {
            var resultado = await _serviceRegulacionDmz.Consulta_Multas_RegulacionExtranjerosAsync(
                    codigoRegistro: Convert.ToInt32(numeroRegistro)
                );
            return MapearRespuestaMultas(resultado);
        }


        public async Task<ConsultarFlujoMigratorioRespuesta> ConsultarFlujoMigratorioAsync(string numeroRegistro)
        {
            var resultado = await _serviceRegulacionDmz.Consulta_F_M_RegulacionExtranjerosAsync(
                    codigoRegistro: Convert.ToInt32(numeroRegistro)
                );
            return MapearRespuestaFlujoMigratorio(resultado);
        }


        public async Task<ConsultarPagoRespuesta> ConsultarPagoAsync(string numeroComprobante)
        {
            var resultado = await _serviceRegulacionDmz.ConsultaPagoAsync(numeroComprobante);

            return MapearRespuestaPago(resultado);
        }


        public async Task<ConsultarDiscapacidadRespuesta> ConsultarDiscapacidadAsync(string numeroCedula)
        {
            var resultado = await _serviceRegulacionDmz.ConsultaDiscapacidadmspAsync(numeroCedula);

            return MapearRespuestaDiscapacidad(resultado);
        }



        #region metodos soporte

        private InformacionPersonaDto MapearRespuesta(RegulacionExtranjerosModel respuesta)
        {
            var personInformationDto = new InformacionPersonaDto
            {
                Nombres = respuesta.Nombres,
                PrimerApellido = respuesta.PrimerApellido,
                SegundoApellido = respuesta.SegundoApellido,
                FechaNacimiento = respuesta.FechaNacimiento,
                PaisNacimiento = respuesta.PaisNacimiento,
                Nacionalidad = respuesta.Nacionalidad,
                OtraNacionalidad = respuesta.SeleccioneOtraNacionalidad,
                CorreoElectronico = respuesta.CorreoElectronico,
                PoseeDocumentoIdentidad = respuesta.DocumentoIdentidadSiNo,
                TipoDocumentoIdentidad = respuesta.TipoDocumento,
                NumeroDocumentoViaje = respuesta.NumeroDocumento,
                PaisEmisionDocumentoIdentidad = respuesta.PaisEmision,
                FechaEmisionDocumentoIdentidad = respuesta.FechaEmisionDocumento,
                FechaExpiracionDocumentoIdentidad = respuesta.FechaExpiracionDocumento,
                Genero = respuesta.Genero,
                EstadoCivil = respuesta.EstadoCivil,
                Telefono = respuesta.Telefono,
                Direccion = respuesta.DireccionEcuador,
                Provincia = respuesta.Provincia,
                Ciudad = respuesta.Ciudad,
                NumeroVisa = respuesta.NumeroVisa,
                TipoVisa = respuesta.TipoVisa,
                FechaEmisionVisa = respuesta.FechaEmisionVisa,
                FechaExpiracionVisa = respuesta.FechaExpiracionVisa,
                NivelEducativo = respuesta.NivelEducativo,
                Profesion = respuesta.Profesion,
                Ocupacion = respuesta.Ocupacion,
                NumeroRegistroPermanencia = respuesta.NumRegistroPermanencia,
                Fotografia = respuesta.FotoGrafia,
                CodigoRegistro = respuesta.CodigoRegistro.ToString(),
                HuellasDactilares = respuesta.HuellasDactilares,
                FechaIngresoPais = respuesta.FechaIngresoPais,
                IngresoPorPuntoRegular = respuesta.PuntoIngresoRegularSiNo,
                PaisResidenciaPrevia = respuesta.PaisResidenciaPrevia,
            };

            return personInformationDto;
        }

        private ConsultarPuntoControlOutput ObtenerRespuestaPuntoControl(PuntoEntradaModel respuesta)
        {
            var resultado = new ConsultarPuntoControlOutput();
            if (respuesta.Codigo != Convert.ToInt32(CodigoRespuestaRegulacionExtranjeros.DatoEncontrado))
            {
                resultado.Correcto = false;
                return resultado;
            }

            resultado.PuntoIngresoRegular = respuesta.PuntoIngresoRegularSiNo.ToUpper().Equals(RespuestaMdgConsts.SI);
            resultado.Correcto = true;
            return resultado;
        }

        private ConsultarMultasRespuesta MapearRespuestaMultas(MultasModel respuesta)
        {
            var resultado = new ConsultarMultasRespuesta();
            if (respuesta.Codigo != Convert.ToInt32(CodigoRespuestaRegulacionExtranjeros.DatoEncontrado))
            {
                resultado.Correcto = false;
                resultado.Codigo = respuesta.Codigo;
                resultado.Detalle = respuesta.Detalle;
                return resultado;
            }

            var multaDto = new MultaDto
            {
                ApellidosNombres = respuesta.DetalleMulasNotificaciones.ApellidosNombres,
                FechaNacimiento = respuesta.DetalleMulasNotificaciones.FechaNacimiento,
                PaisNacimiento = respuesta.DetalleMulasNotificaciones.PaisNacimiento,
                PaisResidencia = respuesta.DetalleMulasNotificaciones.PaisResidencia,
                Genero = respuesta.DetalleMulasNotificaciones.Genero,
                Multas = new List<MultaFlujoMigratorio>(),
                NotificacionesSalidas = new List<NotificacionSalidaVoluntaria>()
            };

            //Mapear multas
            if (respuesta.DetalleMulasNotificaciones.ListaMultas != null)
            {
                foreach (var detalle in respuesta.DetalleMulasNotificaciones.ListaMultas)
                {
                    var detalleMulta = new MultaFlujoMigratorio
                    {
                        CodigoError = detalle.CodigoError,
                        Estado = detalle.Estado,
                        FechaRegistro = detalle.FechaRegistro,
                        TipoMulta = detalle.TipoMulta
                    };
                    multaDto.Multas.Add(detalleMulta);
                }
            }

            //Mapear notificaciones de salida voluntaria
            if (respuesta.DetalleMulasNotificaciones.ListaNotificacionesSalidas != null)
            {
                foreach (var notificacion in respuesta.DetalleMulasNotificaciones.ListaNotificacionesSalidas)
                {
                    var notificacionSalida = new NotificacionSalidaVoluntaria
                    {
                        CodigoError = notificacion.CodigoError,
                        Estado = notificacion.Estado,
                        FechaRegistro = notificacion.FechaRegistro
                    };
                    multaDto.NotificacionesSalidas.Add(notificacionSalida);
                }
            }

            resultado.MultaDto = multaDto;
            resultado.Correcto = true;
            resultado.Codigo = respuesta.Codigo;
            resultado.Detalle = respuesta.Detalle;
            return resultado;
        }

        private ConsultarFlujoMigratorioRespuesta MapearRespuestaFlujoMigratorio(FlujoMigratorioDatos respuesta)
        {
            var resultado = new ConsultarFlujoMigratorioRespuesta();
            if (respuesta.Codigo != Convert.ToInt32(CodigoRespuestaRegulacionExtranjeros.DatoEncontrado))
            {
                resultado.Correcto = false;
                resultado.Codigo = respuesta.Codigo;
                resultado.Detalle = respuesta.Detalle;
                return resultado;
            }

            resultado.FlujosMigratoriosMovimientosDto = new List<FlujoMigratorioMovimientoDto>();

            //Mapear FlujosMigratorios
            if (respuesta.ListaMovimientoMigratorio != null)
            {
                foreach (var flujo in respuesta.ListaMovimientoMigratorio)
                {
                    var flujoMigratorioMovimientoDto = new FlujoMigratorioMovimientoDto
                    {
                        ApellidosNombres = flujo.ApellidosNombres,
                        CategoriaMigratoria = flujo.CategoriaMigratoria,
                        CodigoError = flujo.CodigoError,
                        FechaHoraMovimiento = flujo.FechaHoraMovimiento,
                        FechaNacimiento = flujo.FechaNacimiento,
                        Genero = flujo.Genero,
                        Medio = flujo.Medio,
                        MotivoViaje = flujo.MotivoViaje,
                        NacionalidadDocumentoMovimientoMigratorio = flujo.NacionalidadDocumentoMovMigra,
                        NumeroDocumentoMovimientoMigratorio = flujo.NumeroDocumentoMovMigra,
                        PaisDestino = flujo.PaisDestino,
                        PaisNacimiento = flujo.PaisNacimiento,
                        PaisOrigen = flujo.PaisOrigen,
                        PaisResidencia = flujo.PaisResidencia,
                        PuertoRegistro = flujo.PuertoRegistro,
                        TarjetaAndina = flujo.TarjetaAndina,
                        TiempoDeclarado = flujo.TiempoDeclarado,
                        TipoDocumentoMovimientoMigratorio = flujo.TipoDocumentoMovMigra,
                        TipoMovimiento = flujo.TipoMovimiento
                    };
                    resultado.FlujosMigratoriosMovimientosDto.Add(flujoMigratorioMovimientoDto);
                }
            }

            resultado.Correcto = true;
            resultado.Codigo = respuesta.Codigo;
            resultado.Detalle = respuesta.Detalle;
            return resultado;
        }

        private ConsultarPagoRespuesta MapearRespuestaPago(ComprobanteBanEcuadorModel respuesta)
        {
            var resultado = new ConsultarPagoRespuesta();
            resultado.CodigoBanEcuador = respuesta.CodigoBanEcuador;
            resultado.Cuenta = respuesta.Cuenta;
            resultado.Fecha = respuesta.Fecha;
            resultado.MensajeBanEcuador = respuesta.MensajeBanEcuador;
            resultado.NumeroComprobante = respuesta.NumeroComprobante;
            resultado.Oficina = respuesta.Oficina;
            resultado.Valor = respuesta.Valor;

            return resultado;
        }

        private ConsultarDiscapacidadRespuesta MapearRespuestaDiscapacidad(DiscapacidadModel respuesta)
        {
            var resultado = new ConsultarDiscapacidadRespuesta();

            resultado.Codigo = respuesta.Codigo;
            resultado.Cedula = respuesta.Cedula;
            resultado.Apellidos = respuesta.Apellidos;
            resultado.Nombres = respuesta.Nombres;
            resultado.CodigoConadis = respuesta.CodigoConadis;
            resultado.GradoDiscapacidad = respuesta.GradoDiscapacidad;
            resultado.PorcentajeDiscapacidad = respuesta.PorcentajeDiscapacidad;
            resultado.TipoDiscapacidadPredomina = respuesta.TipoDiscapacidadPredomina;
            resultado.Mensaje = respuesta.Mensaje;

            return resultado;
        }

        #endregion metodos soporte
    }
}