using neoStockMasterv2.Data.Entities;
using Newtonsoft.Json;

namespace neoStockMasterv2.Data.Services
{
    class ProductService
    {
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "products.json");

        public static User LoggedInUser { get; set; }
        public static event Action OnProductsChanged;

        // ── Dosya ──
        private List<Product> GetProductsFromFile()
        {
            if (!File.Exists(_filePath)) return new List<Product>();
            return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(_filePath)) ?? new List<Product>();
        }

        private void SaveProductsToFile(List<Product> products)
        {
            string tempPath = _filePath + ".tmp";
            File.WriteAllText(tempPath, JsonConvert.SerializeObject(products, Formatting.Indented));
            File.Move(tempPath, _filePath, overwrite: true);
        }

        private static bool IsMine(Product p) =>
            LoggedInUser != null &&
            string.Equals(p.AddedBy, LoggedInUser.Name, StringComparison.OrdinalIgnoreCase);

        private static Product FindMine(List<Product> products, string name) =>
            products.FirstOrDefault(p => IsMine(p) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        private void NotifyUI() => OnProductsChanged?.Invoke();

        // ── Ekle / Güncelle / Sil ──
        public bool AddProduct(Product product)
        {
            var products = GetProductsFromFile();

            if (FindMine(products, product.Name) != null)
            {
                MessageBox.Show(LanguageService.T($"'{product.Name}' adlı ürün zaten mevcut.", $"A product named '{product.Name}' already exists."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            product.AddedBy = LoggedInUser.Name;
            product.DateAdded = DateTime.Now;

            products.Add(product);
            SaveProductsToFile(products);
            ActivityLogService.Log(ActivityType.ProductAdded, new Dictionary<string, string>
            {
                ["Product"] = product.Name,
                ["Cost"] = $"{product.Cost:N2} {product.CostCurrency}",
                ["Price"] = $"{product.Price:N2} {product.PriceCurrency}",
                ["Stock"] = product.Stock.ToString()
            });
            MessageBox.Show(LanguageService.T($"Ürün {product.Name} başarıyla eklendi.", $"Product {product.Name} has been added successfully."),
                   LanguageService.T("Başarılı", "Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            NotifyUI();
            return true;
        }

        public bool UpdateProduct(string oldProductName, Product updatedProduct)
        {
            var products = GetProductsFromFile();
            var productToUpdate = FindMine(products, oldProductName);

            if (productToUpdate == null)
            {
                MessageBox.Show(LanguageService.T("Güncellenecek ürün bulunamadı.", "The product to update could not be found."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool nameChanged = !oldProductName.Equals(updatedProduct.Name, StringComparison.OrdinalIgnoreCase);

            if (nameChanged && FindMine(products, updatedProduct.Name) != null)
            {
                MessageBox.Show(LanguageService.T($"'{updatedProduct.Name}' adlı bir ürününüz zaten var.", $"You already have a product named '{updatedProduct.Name}'."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var logData = new Dictionary<string, string>
            {
                ["Product"] = oldProductName,
                ["Name"] = ActivityLogService.Change(productToUpdate.Name, updatedProduct.Name),
                ["Cost"] = ActivityLogService.Change($"{productToUpdate.Cost:N2} {productToUpdate.CostCurrency}",
                                            $"{updatedProduct.Cost:N2} {updatedProduct.CostCurrency}"),
                ["Price"] = ActivityLogService.Change($"{productToUpdate.Price:N2} {productToUpdate.PriceCurrency}",
                                            $"{updatedProduct.Price:N2} {updatedProduct.PriceCurrency}"),
                ["Stock"] = ActivityLogService.Change(productToUpdate.Stock.ToString(), updatedProduct.Stock.ToString())
            };

            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.Cost = updatedProduct.Cost;
            productToUpdate.Price = updatedProduct.Price;
            productToUpdate.Stock = updatedProduct.Stock;
            productToUpdate.DateAdded = updatedProduct.DateAdded;
            productToUpdate.CostCurrency = updatedProduct.CostCurrency;
            productToUpdate.PriceCurrency = updatedProduct.PriceCurrency;

            SaveProductsToFile(products);

            ActivityLogService.Log(ActivityType.ProductUpdated, logData);

            // Siparişlerdeki eski ürün adını da güncelle (stok iadesi bozulmasın)
            if (nameChanged)
                new OrderService().RenameProductInOrders(oldProductName, updatedProduct.Name, LoggedInUser.Name);

            NotifyUI();
            return true;
        }

        public void DeleteProduct(string id)
        {
            var products = GetProductsFromFile();
            var productToRemove = products.FirstOrDefault(p => IsMine(p) && string.Equals(p.ID, id, StringComparison.OrdinalIgnoreCase));
            if (productToRemove == null) return;

            products.Remove(productToRemove);
            SaveProductsToFile(products);
            ActivityLogService.Log(ActivityType.ProductDeleted, new Dictionary<string, string>
            {
                ["Product"] = productToRemove.Name,
                ["Stock"] = productToRemove.Stock.ToString()
            });
            NotifyUI();
        }

        // ── Stok ──
        public bool AddStockToProduct(string productName, int amountToAdd)
        {
            var products = GetProductsFromFile();
            var product = FindMine(products, productName);

            if (product == null)
            {
                MessageBox.Show(LanguageService.T($"'{productName}' adlı ürün bulunamadı, stok güncellenemedi.", $"Product '{productName}' was not found, stock was not updated."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            product.Stock += amountToAdd;
            SaveProductsToFile(products);
            NotifyUI();
            return true;
        }

        /// <summary>Sipariş kaydedilmeden önce çağrılır. Sorun yoksa null, varsa hata mesajı döner.</summary>
        public string? CheckStock(IEnumerable<(string ProductName, int Quantity)> items)
        {
            var products = GetProductsFromFile();

            foreach (var group in items.GroupBy(i => i.ProductName, StringComparer.OrdinalIgnoreCase))
            {
                int requested = group.Sum(i => i.Quantity);
                var product = FindMine(products, group.Key);

                if (product == null)
                    return LanguageService.T($"'{group.Key}' adlı ürün bulunamadı.", $"Product '{group.Key}' was not found.");
                if (product.Stock < requested)
                    return LanguageService.T(
                        $"'{group.Key}' için yeterli stok yok. Mevcut: {product.Stock}, istenen: {requested}",
                        $"Not enough stock for '{group.Key}'. Available: {product.Stock}, requested: {requested}");
            }
            return null;
        }

        /// <summary>Tüm sipariş satırlarının stoklarını tek seferde düşer.</summary>
        public void DecreaseStockForOrder(IEnumerable<(string ProductName, int Quantity)> items)
        {
            var products = GetProductsFromFile();

            foreach (var (productName, quantity) in items)
            {
                var product = FindMine(products, productName);
                if (product != null && quantity > 0)
                    product.Stock -= quantity;
            }

            SaveProductsToFile(products);
            NotifyUI();
        }

        // ── Sorgular ──
        public List<Product> GetAllProducts() => GetProductsFromFile();

        public List<Product> GetProductsByLoggedInUser()
        {
            if (LoggedInUser == null)
            {
                MessageBox.Show(LanguageService.T("Lütfen giriş yapınız.", "Please log in."),
                    LanguageService.T("Uyarı", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<Product>();
            }
            return GetProductsFromFile().Where(IsMine).ToList();
        }

        public int GetProductStockQuantity(string productName) =>
            FindMine(GetProductsFromFile(), productName)?.Stock ?? 0;
    }
}