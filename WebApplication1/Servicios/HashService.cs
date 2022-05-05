using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApplication1.DTOs;

namespace WebApplication1.Servicios
{
    public class HashService
    {


        public ResultadoHashDTOO Hash(string textoplano)
        {
            var sal = new byte[16];

            using(var random= RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }
            return Hash(textoplano, sal);
        }


        public ResultadoHashDTOO Hash(string textoplano, byte[] sal)
        {
            var llaveDeribada = KeyDerivation.Pbkdf2(password: textoplano,
                salt: sal, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);
            var hash= Convert.ToBase64String(llaveDeribada);

            return new ResultadoHashDTOO()
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
