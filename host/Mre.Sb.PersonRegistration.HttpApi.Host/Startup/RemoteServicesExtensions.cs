using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mre.Sb.Notificacion.HttpApi;
using Mre.Sb.PersonRegistration.HttpApiClient;
using System;
using Volo.Abp.Modularity;

namespace Mre.Sb.RegistroPersona
{
    public static class RemoteServicesExtensions
    {

        public static void ConfigureHttpClient(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {

            //1. Opcion, con proxy dinamicos creados con Abp
            //context.Services.AddHttpClientProxy(
            //  typeof(IIdentidadClient),
            //  "IdentidadClient"
            //);


            //2. Cliente creados con nswag
            var urlBase = configuration["RemoteServices:Base:BaseUrl"];

            context.Services.AddHttpClient<IIdentidadClient, IdentidadClient>(
                c =>
                {
                    c.BaseAddress = new Uri(urlBase);
                })
              //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              //.AddDevspacesSupport()
              ;

            //Cliente para envio de notificaciones
            var urlServicioNotificacion = configuration["RemoteServices:Notificacion:BaseUrl"];

            context.Services.AddHttpClient<INotificadorClient, NotificadorClient>(
                c =>
                {
                    c.BaseAddress = new Uri(urlServicioNotificacion);
                })
              ;
        }

    }
}
