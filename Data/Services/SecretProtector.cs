using System.Security.Cryptography;
using System.Text;

namespace neoStockMasterv2.Data.Services
{
    public static class SecretProtector
    {
        public static string Protect(string plain)
        {
            if (string.IsNullOrEmpty(plain)) return "";
            byte[] data = ProtectedData.Protect(Encoding.UTF8.GetBytes(plain), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(data);
        }

        public static string Unprotect(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText)) return "";
            try
            {
                byte[] data = ProtectedData.Unprotect(Convert.FromBase64String(protectedText), null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return ""; // eski düz metin kayıt veya başka kullanıcı: şifreyi tekrar girsin
            }
        }
    }
}