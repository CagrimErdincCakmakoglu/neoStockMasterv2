using neoStockMasterv2.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace neoStockMasterv2.Data.Services
{
    public static class ActivityLogService
    {
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "activity.json");
        private const int MaxPerUser = 300;

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }   // dosyada "OrderAdded" gibi okunur yazılır
        };

        /// <summary>Giriş yapan kullanıcı adına bir işlem kaydeder.</summary>
        public static void Log(ActivityType type, Dictionary<string, string>? data = null)
        {
            var user = UserService.LoggedInUser;
            if (user == null) return;

            try
            {
                var all = LoadAll();
                all.Add(new ActivityLog
                {
                    UserName = user.Name,
                    Type = type,
                    Data = data ?? new Dictionary<string, string>()
                });

                // Kullanıcı başına en yeni 300 kaydı tut
                var overflow = all
                    .Where(a => a.UserName.Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(a => a.Date)
                    .Skip(MaxPerUser)
                    .ToHashSet();
                if (overflow.Count > 0)
                    all.RemoveAll(overflow.Contains);

                Save(all);
            }
            catch
            {
                // Günlük yazılamazsa asıl işlem (sipariş, ürün...) bozulmasın
            }
        }

        public static List<ActivityLog> GetForUser(string userName) =>
            LoadAll()
                .Where(a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.Date)
                .ToList();

        // ── Biçimlendirme yardımcıları ──
        public static string Change(string oldValue, string newValue) =>
            oldValue == newValue ? newValue : $"{oldValue} → {newValue}";

        public static string FormatAmounts(Dictionary<string, decimal>? amounts) =>
            amounts == null || amounts.Count == 0
                ? "-"
                : string.Join(" | ", amounts.Select(x => $"{x.Value:N2} {x.Key}"));

        public static string FormatItems(IEnumerable<Order.OrderDetail>? items) =>
            items == null
                ? "-"
                : string.Join(", ", items.Select(i => $"{i.ProductName} x{i.Quantity}"));

        // ── Dosya ──
        private static List<ActivityLog> LoadAll()
        {
            if (!File.Exists(_filePath)) return new List<ActivityLog>();
            return JsonConvert.DeserializeObject<List<ActivityLog>>(File.ReadAllText(_filePath), _settings)
                   ?? new List<ActivityLog>();
        }

        private static void Save(List<ActivityLog> logs)
        {
            string tempPath = _filePath + ".tmp";
            File.WriteAllText(tempPath, JsonConvert.SerializeObject(logs, _settings));
            File.Move(tempPath, _filePath, overwrite: true);
        }
    }

    public static class ActivityText
    {
        public static string Title(ActivityType type, bool tr) => type switch
        {
            ActivityType.Login => tr ? "Giriş yapıldı" : "Logged in",
            ActivityType.ProductAdded => tr ? "Ürün eklendi" : "Product added",
            ActivityType.ProductUpdated => tr ? "Ürün güncellendi" : "Product updated",
            ActivityType.ProductDeleted => tr ? "Ürün silindi" : "Product deleted",
            ActivityType.StockAdded => tr ? "Stok eklendi" : "Stock added",
            ActivityType.OrderAdded => tr ? "Sipariş oluşturuldu" : "Order created",
            ActivityType.OrderUpdated => tr ? "Sipariş güncellendi" : "Order updated",
            ActivityType.OrderDeleted => tr ? "Sipariş silindi" : "Order deleted",
            ActivityType.PasswordChanged => tr ? "Şifre değiştirildi" : "Password changed",
            ActivityType.SerialKeyApplied => tr ? "Kullanım süresi eklendi" : "Usage time added",
            ActivityType.CargoUpdated => tr ? "Kargo firması güncellendi" : "Cargo company updated",
            ActivityType.LanguageChanged => tr ? "Dil tercihi değiştirildi" : "Language changed",
            _ => type.ToString()
        };

        /// <summary>Listede görünen kısa satır: "Ürün eklendi: Kolye"</summary>
        public static string Summary(ActivityLog log, bool tr)
        {
            string title = Title(log.Type, tr);
            string? subject =
                log.Data.TryGetValue("Product", out var product) ? product :
                log.Data.TryGetValue("Customer", out var customer) ? customer :
                log.Data.TryGetValue("Cargo", out var cargo) ? cargo : null;

            return string.IsNullOrEmpty(subject) ? title : $"{title}: {subject}";
        }

        public static string FieldLabel(string key, bool tr) => key switch
        {
            "Ip" => "IP",
            "Product" => tr ? "Ürün" : "Product",
            "Name" => tr ? "Ürün Adı" : "Product Name",
            "Cost" => tr ? "Maliyet" : "Cost",
            "Price" => tr ? "Satış Fiyatı" : "Sale Price",
            "Stock" => tr ? "Stok" : "Stock",
            "Quantity" => tr ? "Adet" : "Quantity",
            "Customer" => tr ? "Müşteri" : "Customer",
            "Phone" => tr ? "Telefon" : "Phone",
            "Items" => tr ? "Ürünler" : "Items",
            "Amount" => tr ? "Ödenecek Tutar" : "Payable Amount",
            "Status" => tr ? "Sipariş Durumu" : "Order Status",
            "Payment" => tr ? "Ödeme Durumu" : "Payment Status",
            "Days" => tr ? "Eklenen Gün" : "Days Added",
            "SubscriptionEnd" => tr ? "Abonelik Bitişi" : "Subscription End",
            "Cargo" => tr ? "Kargo" : "Cargo",
            "Language" => tr ? "Dil" : "Language",
            _ => key
        };
    }
}