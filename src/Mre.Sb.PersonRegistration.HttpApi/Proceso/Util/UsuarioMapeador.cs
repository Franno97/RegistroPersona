using Mre.Sb.PersonRegistration.HttpApiClient;
using Mre.Sb.RegistroPersona.Persona;
using Mre.Sb.RegistroPersona.PersonaMdg;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class UsuarioMapeador
    {
        public static UsuarioCrearDto GenerarUsuarioCrearDto(InformacionPersonaDto personaInfo)
        {
            var usuarioCrearDto = new UsuarioCrearDto
            {
                UserName = personaInfo.CorreoElectronico,
                Name = personaInfo.Nombres,
                Surname = string.IsNullOrEmpty(personaInfo.SegundoApellido) ? personaInfo.PrimerApellido : $"{personaInfo.PrimerApellido} {personaInfo.SegundoApellido}",
                Email = personaInfo.CorreoElectronico,
                PhoneNumber = personaInfo.Telefono,
                IsActive = true,
                //Activar mecanismo bloqueo al usuario, por X accesos incorrectos continuos.
                LockoutEnabled = true,
                UserType = UserType._2
            };

            return usuarioCrearDto;
        }

    }
}
