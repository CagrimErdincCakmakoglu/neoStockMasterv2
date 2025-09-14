using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace neoStockMasterv2.Data.Entities
{
    public class User
    {
        public string ID { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cargo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int UsageRemainingDays { get; set; }
        public int RemainingDays { get; set; }
        public string Language { get; set; }


        [JsonProperty]
        public List<LoginRecord> LoginHistory { get; private set; } = new List<LoginRecord>();

        [JsonProperty]
        public string VerificationCode { get;  private set; }

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
        public string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public void AddLoginIp(string ip)
        {
            ip = ip?.Trim();

            // Aynı gün aynı IP ile giriş yapılmışsa tekrar ekleme
            if (!string.IsNullOrEmpty(ip) && !LoginHistory.Any(record => record.IP.Trim() == ip && record.LoginDate.Date == DateTime.Now.Date))
            {
                // 6'dan fazla kayıt varsa en eskisini sil
                if (LoginHistory.Count >= 6)
                {
                    // En eski giriş tarihli kaydı bul ve sil
                    var oldest = LoginHistory.OrderBy(r => r.LoginDate).FirstOrDefault();
                    if (oldest != null)
                    {
                        LoginHistory.Remove(oldest);
                    }
                }

                // Yeni kaydı ekle
                LoginHistory.Add(new LoginRecord
                {
                    IP = ip,
                    LoginDate = DateTime.Now
                });
            }
        }
    }
}
