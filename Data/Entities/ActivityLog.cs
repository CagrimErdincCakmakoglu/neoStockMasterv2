namespace neoStockMasterv2.Data.Entities
{
    public enum ActivityType
    {
        Login,
        ProductAdded,
        ProductUpdated,
        ProductDeleted,
        StockAdded,
        OrderAdded,
        OrderUpdated,
        OrderDeleted,
        PasswordChanged,
        SerialKeyApplied,
        CargoUpdated,
        LanguageChanged
    }

    public class ActivityLog
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public ActivityType Type { get; set; }

        // Dilden bağımsız detaylar: anahtar (ör. "Product") → değer
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}