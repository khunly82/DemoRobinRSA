using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Security.Rsa
{
    public class EncryptionService(RSACryptoServiceProvider provider)
    {
        private readonly string _privateKey = provider.ExportRSAPrivateKeyPem(); 
        private readonly string _publicKey = provider.ExportRSAPublicKeyPem();
        public string PublicKey => _publicKey;

        public string Decrypt(string encodedValue)
        {
            provider.ImportFromPem(_privateKey);
            return Encoding.UTF8.GetString(provider.Decrypt(Convert.FromBase64String(encodedValue), true));
        }

        public string Encrypt(string rawValue, string publicKey)
        {
            provider.ImportFromPem(publicKey);
            return Convert.ToBase64String(provider.Encrypt(Encoding.UTF8.GetBytes(rawValue), true));
        }
    }
}
