using System;
using PaymentGateway.Services.Cryptography.Interface;

namespace PaymentGateway.Services.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        private readonly string _key;
        private readonly string _iv;
        
        public CryptographyService(string key, string iv)
        {
            _key = !string.IsNullOrEmpty(key) ? key : throw new ArgumentNullException(nameof(key));
            _iv = !string.IsNullOrEmpty(iv) ? iv : throw new ArgumentNullException(nameof(iv));
        }

        public string Encyrpt(string plainText)
        {
            return null;
        }

        public string Decrypt(string encyrptedText)
        {
            return null;
        }
    }
}