using System;
using System.Collections.Generic;
using System.Text;

namespace Mre.Sb.RegistroPersona.Proceso
{
    public class CodigoVerificacionGenerador
    {
        public static string GenerateVerificationCode()
        {
            //TODO: Mejorar la forma de generacion
            const int MinValue = 100000;
            const int MaxValue = 999999;
            var verificationCode = new Random().Next(MinValue, MaxValue);

            return verificationCode.ToString();
        }
    }
}
