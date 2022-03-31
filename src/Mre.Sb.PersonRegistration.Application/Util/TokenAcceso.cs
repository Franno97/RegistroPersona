using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.PersonRegistration.Util
{
    public interface ITokenAcceso
    {

        Task<string> ObtenerAsync();

    }


    public class TokenAcceso : ITokenAcceso, ITransientDependency
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }

        private readonly string BearerPrefix = "Bearer ";

        private readonly string CabeceraAutentificacion = "Authorization";

        public TokenAcceso(
            IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<string> ObtenerAsync()
        {
            if (HttpContextAccessor.HttpContext?.Request?.Headers == null)
            {
                return string.Empty;
            }

            var tokenAcceso = string.Empty;

            string autentificacionCabecera = HttpContextAccessor.HttpContext.Request.Headers[CabeceraAutentificacion];

            if (autentificacionCabecera != null && autentificacionCabecera.StartsWith(BearerPrefix)
                && !string.IsNullOrEmpty(autentificacionCabecera))
            {
                tokenAcceso = autentificacionCabecera.Substring(BearerPrefix.Length);
            }

            return await Task.FromResult(tokenAcceso);
        }
    }
}
