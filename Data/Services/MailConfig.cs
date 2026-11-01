using Newtonsoft.Json;

namespace neoStockMasterv2.Data.Services
{
    public class MailConfig
    {
        public string SenderEmail { get; set; } = "";
        public string AppPassword { get; set; } = "";
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;

        private static MailConfig? _current;
        public static MailConfig Current => _current ??= Load();

        private static MailConfig Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "mailsettings.json");
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    "mailsettings.json bulunamadı. mailsettings.example.json dosyasını kopyalayıp doldurun.", path);

            return JsonConvert.DeserializeObject<MailConfig>(File.ReadAllText(path)) ?? new MailConfig();
        }
    }
}