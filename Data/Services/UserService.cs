using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace neoStockMasterv2.Data.Services
{
    public class UserService
    {
        private readonly string _filePath = "users.json";
        public static User LoggedInUser { get; set; }
        private List<User> users;
        private User _currentUser;

        public UserService()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
            users = GetAllUsers();
        }

        public string RegisterUser(User newUser)
        {
            users = GetAllUsers();

            if (users.Any(u => u.Name.Equals(newUser.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return "Bu kullanıcı adı zaten kullanılıyor!";
            }

            if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return "Bu e-posta adresi zaten kayıtlı!";
            }

            try
            {
                EmailService emailService = new EmailService("stockmasterapp@gmail.com", "bfbi cpom gikz azjx");
                emailService.SendWelcomeEmail(newUser.Email, newUser.Name, newUser.VerificationCode);

                users.Add(newUser);
                SaveAllUsers(users);
            }
            catch (Exception ex)
            {
                return $"Kayıt başarısız! Hoş geldiniz e-postası gönderilemedi: {ex.Message}";
            }

            return "Kayıt başarılı!";
        }

        public string Login(string username, string password, bool rememberUsername, bool rememberPassword)
        {
            users = GetAllUsers();

            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null || user.Password != password)
            {
                return "Geçersiz kullanıcı adı veya şifre!";
            }

            if (!user.IsVerified) // Kullanıcının onaylanıp onaylanmadığını kontrol et
            {
                return "Üyeliğiniz henüz onaylanmamış!\nLütfen hesabınızı onaylayınız.";
            }

            _currentUser = user;
            LoggedInUser = user;
            string currentIp = GetIpAddress();
            user.AddLoginIp(currentIp);
            SaveAllUsers(users);

            if (rememberUsername)
                Settings.Default.Username = username;
            else
                Settings.Default.Username = string.Empty;

            if (rememberPassword)
                Settings.Default.Password = password;
            else
                Settings.Default.Password = string.Empty;

            Settings.Default.Save();

            return $"Giriş başarılı! Hoş geldiniz, {user.Name}.";
        }

        public string ChangePassword(string username, string currentPassword, string newPassword)
        {
            users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return "Geçersiz mevcut şifre!";
            }

            if (newPassword.Length < 6)
            {
                return "Yeni şifre en az 6 karakter olmalıdır!";
            }

            user.Password = newPassword;
            SaveAllUsers(users);

            return "Şifreniz başarıyla değiştirildi!";
        }

        public bool UpdateCargoCompany(User user)
        {
            try
            {
                // Kullanıcı verisini veri kaynağında güncelle (örneğin JSON dosyası ya da DB)
                // Örnek bir JSON update için:
                User existingUser = users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    existingUser.Cargo = user.Cargo;
                    SaveAllUsers(users);// ya da SaveChanges gibi bir metot
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
            }

            return false;
        }

        public string LogOut()
        {
            if (_currentUser == null)
                return "Zaten giriş yapılmamış!";

            _currentUser = null;
            Settings.Default.Save();
            return "Başarıyla çıkış yaptınız!";
        }

        public string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ipAddress?.ToString() ?? "IP adresi alınamadı";
        }

        public List<User> GetAllUsers()
        {
            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        public void SaveAllUsers(List<User> users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public User FindUserByEmail(string email)
        {
            users = GetAllUsers();
            return users?.FirstOrDefault(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserByName(string userName)
        {
            users = GetAllUsers();
            return users?.FirstOrDefault(user => user.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }


        public User GetCurrentUser()
        {
            return _currentUser;
        }


    }
}
