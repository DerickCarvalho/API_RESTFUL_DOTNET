using ApiReservas.Models;
using System.Security.Cryptography;
using System.Text;

namespace ApiReservas.Services
{
    public class EncPassword
    {
        public string EncriptPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var encPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

            return encPassword;
        }
    }
}
