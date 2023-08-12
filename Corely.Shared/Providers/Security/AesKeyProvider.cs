using System.Security.Cryptography;

namespace Corely.Shared.Providers.Security
{
    public class AesKeyProvider : IKeyProvider
    {
        public string CreateKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return Convert.ToBase64String(aes.Key);
            }
        }

        public bool IsKeyValid(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            using (Aes aes = Aes.Create())
            {
                try
                {
                    aes.Key = Convert.FromBase64String(key);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
