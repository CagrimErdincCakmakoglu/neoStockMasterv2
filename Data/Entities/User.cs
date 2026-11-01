using System.Security.Cryptography;
using Newtonsoft.Json;

namespace neoStockMasterv2.Data.Entities
{
    public class User
    {
        [JsonProperty]
        public string ID { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }          // artık PBKDF2 hash tutar
        public string Cargo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Language { get; set; }

        // Abonelik
        public DateTime? SubscriptionEnd { get; set; }
        public List<string> UsedSerialKeys { get; set; } = new List<string>();

        [Obsolete("Sadece eski users.json kayıtlarını taşımak için. DaysLeft kullanın.")]
        public int RemainingDays { get; set; }

        [JsonIgnore]
        public int DaysLeft => SubscriptionEnd.HasValue
            ? Math.Max(0, (SubscriptionEnd.Value.Date - DateTime.Today).Days)
            : 0;

        [JsonProperty]
        public List<LoginRecord> LoginHistory { get; private set; } = new List<LoginRecord>();

        [JsonProperty]
        public string VerificationCode { get; private set; }

        [JsonProperty]
        public bool IsVerified { get; set; } = false;

        public User(string name, string email, string password, string language, string verificationCode = null)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Email = email;
            Password = password;
            RegistrationDate = DateTime.Now;
            Language = language;
            VerificationCode = verificationCode ?? GenerateVerificationCode();
        }

        public static string GenerateVerificationCode() =>
            RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

#pragma warning disable CS0618
        // Eski "RemainingDays" değerini bitiş tarihine çevirir (girişte bir kez çalışır)
        public bool MigrateLegacySubscription()
        {
            if (SubscriptionEnd == null && RemainingDays > 0)
            {
                SubscriptionEnd = DateTime.Today.AddDays(RemainingDays);
                RemainingDays = 0;
                return true;
            }
            return false;
        }
#pragma warning restore CS0618

        public void AddLoginIp(string ip)
        {
            ip = ip?.Trim();

            if (string.IsNullOrEmpty(ip)) return;
            if (LoginHistory.Any(r => r.IP.Trim() == ip && r.LoginDate.Date == DateTime.Today)) return;

            if (LoginHistory.Count >= 6)
            {
                var oldest = LoginHistory.OrderBy(r => r.LoginDate).First();
                LoginHistory.Remove(oldest);
            }

            LoginHistory.Add(new LoginRecord { IP = ip, LoginDate = DateTime.Now });
        }
    }
}