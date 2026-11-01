using System.Net;
using System.Security.Cryptography;
using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Properties;
using Newtonsoft.Json;

namespace neoStockMasterv2.Data.Services
{
    public class UserService
    {
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "users.json");

        private static readonly Dictionary<string, int> SerialKeys = new()
        {
            ["TRIAL7"] = 7,
            ["FULL30"] = 30,
            ["VIP90"] = 90
        };

        public static User LoggedInUser { get; set; }

        public UserService()
        {
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        // ── Kayıt ──
        public async Task<string> RegisterUserAsync(User newUser)
        {
            var users = GetAllUsers();

            if (users.Any(u => u.Name.Equals(newUser.Name, StringComparison.OrdinalIgnoreCase)))
                return LanguageService.T("Bu kullanıcı adı zaten kullanılıyor!", "This username is already taken!");

            if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
                return LanguageService.T("Bu e-posta adresi zaten kayıtlı!", "This e-mail address is already registered!");

            newUser.Password = PasswordHasher.Hash(newUser.Password);

            try
            {
                await new EmailService().SendWelcomeEmailAsync(newUser.Email, newUser.Name, newUser.VerificationCode);
            }
            catch (Exception ex)
            {
                return LanguageService.T("Kayıt başarısız! Doğrulama e-postası gönderilemedi: ", "Registration failed! The verification e-mail could not be sent: ") + ex.Message;
            }

            users.Add(newUser);
            SaveAllUsers(users);
            return LanguageService.T("Kayıt başarılı! Doğrulama kodu e-posta adresinize gönderildi.", "Registration successful! The verification code has been sent to your e-mail.");
        }

        // ── Giriş / Çıkış ──
        public (bool Success, string Message) Login(string username, string password, bool rememberUsername, bool rememberPassword)
        {
            var users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null || !PasswordHasher.Verify(password, user.Password))
                return (false, LanguageService.T("Geçersiz kullanıcı adı veya şifre!", "Invalid username or password!"));

            if (!user.IsVerified)
                return (false, LanguageService.T("Üyeliğiniz henüz onaylanmamış!\nLütfen hesabınızı onaylayınız.", "Your account has not been verified yet!\nPlease verify your account."));

            // Eski düz metin şifreyi hash'e çevir
            if (!PasswordHasher.IsHashed(user.Password))
                user.Password = PasswordHasher.Hash(password);

            user.MigrateLegacySubscription();
            string ip = GetIpAddress();
            user.AddLoginIp(ip);
            SaveAllUsers(users);

            LoggedInUser = user;
            ActivityLogService.Log(ActivityType.Login, new Dictionary<string, string> { ["Ip"] = ip });

            Settings.Default.Username = rememberUsername ? username : string.Empty;
            Settings.Default.Password = rememberPassword ? SecretProtector.Protect(password) : string.Empty;
            Settings.Default.Save();

            return (true, $"Giriş başarılı! Hoş geldiniz, {user.Name}.");
        }

        public void LogOut()
        {
            LoggedInUser = null;
        }

        // ── Şifre ──
        public (bool Success, string Message) ChangePassword(string username, string newPassword)
        {
            var users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return (false, LanguageService.T("Kullanıcı bulunamadı!", "User not found!"));

            if (newPassword.Length < 6)
                return (false, LanguageService.T("Yeni şifre en az 6 karakter olmalıdır!", "The new password must be at least 6 characters!"));

            if (PasswordHasher.Verify(newPassword, user.Password))
                return (false, LanguageService.T("Yeni şifre, mevcut şifreyle aynı olamaz!", "The new password cannot be the same as the current one!"));

            user.Password = PasswordHasher.Hash(newPassword);
            SaveAllUsers(users);

            ActivityLogService.Log(ActivityType.PasswordChanged);

            if (LoggedInUser?.ID == user.ID)
                LoggedInUser.Password = user.Password;

            Settings.Default.Password = string.Empty;
            Settings.Default.Save();

            return (true, LanguageService.T("Şifreniz başarıyla değiştirildi!", "Your password has been changed successfully!"));
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string email)
        {
            var users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return (false, LanguageService.T("Bu e-posta adresine ait kullanıcı bulunamadı.", "No user was found for this e-mail address."));

            string tempPassword = GenerateTemporaryPassword();

            // Önce mail; gönderilemezse eski şifre geçerli kalır
            await new EmailService().SendTemporaryPasswordEmailAsync(user.Email, tempPassword);

            user.Password = PasswordHasher.Hash(tempPassword);
            SaveAllUsers(users);
            return (true, LanguageService.T("Geçici şifreniz e-posta adresinize gönderildi.", "Your temporary password has been sent to your e-mail address."));
        }

        private static string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            return new string(Enumerable.Range(0, 10)
                .Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)])
                .ToArray());
        }

        // ── Abonelik ──
        public static int GetSerialKeyDays(string serialKey) =>
            SerialKeys.TryGetValue(serialKey.Trim().ToUpperInvariant(), out int days) ? days : 0;

        public (bool Success, string Message) ApplySerialKey(string serialKey)
        {
            serialKey = serialKey.Trim().ToUpperInvariant();
            int days = GetSerialKeyDays(serialKey);

            if (LoggedInUser == null) return (false, LanguageService.T("Aktif kullanıcı bulunamadı!", "No active user found!"));
            if (days == 0) return (false, LanguageService.T("Geçersiz serial key!", "Invalid serial key!"));

            var users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.ID == LoggedInUser.ID);
            if (user == null) return (false, LanguageService.T("Kullanıcı güncellenemedi.", "The user could not be updated."));

            if (user.UsedSerialKeys.Contains(serialKey))
                return (false, LanguageService.T("Bu serial key daha önce kullanılmış!", "This serial key has already been used!"));

            DateTime start = user.SubscriptionEnd.HasValue && user.SubscriptionEnd.Value > DateTime.Today
                ? user.SubscriptionEnd.Value
                : DateTime.Today;

            user.SubscriptionEnd = start.AddDays(days);
            user.UsedSerialKeys.Add(serialKey);
            SaveAllUsers(users);

            LoggedInUser.SubscriptionEnd = user.SubscriptionEnd;
            LoggedInUser.UsedSerialKeys = user.UsedSerialKeys;

            ActivityLogService.Log(ActivityType.SerialKeyApplied, new Dictionary<string, string>
            {
                ["Days"] = days.ToString(),
                ["SubscriptionEnd"] = user.SubscriptionEnd!.Value.ToString("dd.MM.yyyy")
            });

            return (true, LanguageService.T(
                $"{days} gün başarıyla eklendi!\nYeni kalan süreniz: {user.DaysLeft} gün",
                $"{days} days added successfully!\nDays remaining: {user.DaysLeft}"));
        }

        // ── Diğer ──
        public bool UpdateCargoCompany(User user)
        {
            var users = GetAllUsers();
            var existingUser = users.FirstOrDefault(u => u.ID == user.ID);
            if (existingUser == null) return false;

            existingUser.Cargo = user.Cargo;
            SaveAllUsers(users);
            ActivityLogService.Log(ActivityType.CargoUpdated, new Dictionary<string, string> { ["Cargo"] = user.Cargo ?? "-" });
            return true;
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
            string tempPath = _filePath + ".tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, _filePath, overwrite: true);   // yarım yazılmış dosya kalmasın
        }

        public User FindUserByEmail(string email) =>
            GetAllUsers().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}