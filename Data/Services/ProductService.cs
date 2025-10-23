using neoStockMasterv2.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace neoStockMasterv2.Data.Services
{
    class ProductService
    {
        private string _filePath = "products.json"; // JSON dosya yolu

        public static User LoggedInUser { get; set; }



        // JSON dosyasından ürünleri okuma
        private List<Product> GetProductsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Product>(); // Dosya yoksa boş liste döndür
            }

            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        // JSON dosyasına ürünleri yazma
        private void SaveProductsToFile(List<Product> products)
        {
            string json = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        // Ürün ekleme
        public void AddProduct(Product product)
        {
            var products = GetProductsFromFile();

            // Kullanıcı farklılığında aynı isimli ürünü eklemeyi sağladım. 
            var existingProduct = products.FirstOrDefault(p =>
                p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase) &&
                p.AddedBy.Equals(LoggedInUser.Name, StringComparison.OrdinalIgnoreCase));

            if (existingProduct != null)
            {
                MessageBox.Show($"'{product.Name}' adlı ürün zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Aynı isimli ürün varsa ekleme işlemini iptal et
            }

            product.AddedBy = LoggedInUser.Name;
            product.DateAdded = DateTime.Now;

            // Ürün eklemeden önce onay al
            var result = MessageBox.Show($"'{product.Name}' adlı ürünü eklemek istediğinizden emin misiniz?",
                                          "Ürün Ekleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Ürünü ekle
                products.Add(product);
                SaveProductsToFile(products);
                MessageBox.Show($"Ürün {product.Name} başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NotifyUI();
            }
            else
            {
                MessageBox.Show("Ürün ekleme işlemi iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        // Ürün güncelleme
        public void UpdateProduct(string oldProductName, Product updatedProduct)
        {
            var products = GetProductsFromFile();
            var productToUpdate = products.FirstOrDefault(p => p.Name.Equals(oldProductName, StringComparison.OrdinalIgnoreCase));

            if (productToUpdate == null)
            {
                MessageBox.Show("Güncellenecek ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ürünü doğrudan güncelle
            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.Cost = updatedProduct.Cost;
            productToUpdate.Price = updatedProduct.Price;
            productToUpdate.Stock = updatedProduct.Stock;
            productToUpdate.DateAdded = updatedProduct.DateAdded;
            productToUpdate.CostCurrency = updatedProduct.CostCurrency;
            productToUpdate.PriceCurrency = updatedProduct.PriceCurrency;

            SaveProductsToFile(products);

            //MessageBox.Show($"Ürün {updatedProduct.Name} başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NotifyUI();
        }

        // Ürüne stok ekleme
        public bool AddStockToProduct(string productName, int amountToAdd)
        {
            var products = GetProductsFromFile();

            var product = products.FirstOrDefault(p =>
                p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase) &&
                p.AddedBy.Equals(LoggedInUser?.Name, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                MessageBox.Show("Stok eklenecek ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            product.Stock += amountToAdd;
            SaveProductsToFile(products);
            NotifyUI();

            return true;
        }


        // Ürün silme
        public void DeleteProduct(string ID)
        {
            var products = GetProductsFromFile();
            var productToRemove = products.FirstOrDefault(p => p.ID != null && p.ID.Equals(ID, StringComparison.OrdinalIgnoreCase));

            if (productToRemove != null)
            {
                products.Remove(productToRemove);
                SaveProductsToFile(products);
                NotifyUI();
            }
        }

        public List<Product> GetAllProducts()
        {
            return GetProductsFromFile();
        }


        public List<Product> GetProductsByLoggedInUser()
        {
            if (LoggedInUser == null)
            {
                MessageBox.Show("Lütfen giriş yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<Product>(); // Kullanıcı giriş yapmamışsa boş liste döndür
            }

            var products = GetProductsFromFile();

            var userProducts = products.Where(p => p.AddedBy.Equals(LoggedInUser.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userProducts.Count == 0)
            {
                //MessageBox.Show("Bu kullanıcı tarafından eklenmiş ürün bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return userProducts;
        }

        // UI güncelleme bildirimi
        public static event Action OnProductsChanged;

        private void NotifyUI()
        {
            OnProductsChanged?.Invoke();
        }

        public void UpdateStockAfterOrder(string productName, int orderQuantity)
        {
            var products = GetProductsFromFile();
            var product = products.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                MessageBox.Show($"'{productName}' adlı ürün bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (product.Stock < orderQuantity)
            {
                MessageBox.Show($"'{productName}' adlı ürün için yeterli stok yok. Mevcut stok: {product.Stock}",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Stok miktarını güncelle
            product.Stock -= orderQuantity;

            SaveProductsToFile(products); // Güncellenen stokları kaydet
            NotifyUI(); // UI'yi bilgilendir
        }

        public int GetProductStockQuantity(string productName)
        {
            try
            {
                // Veritabanında ürünün stok miktarını sorgula
                var product = GetAllProducts()
                    .FirstOrDefault(p => p.Name == productName); // Ürün ismi ile arama yap

                if (product != null)
                {
                    return product.Stock; // Ürün bulunursa, stok miktarını döndür
                }
                else
                {
                    // Ürün bulunamazsa 0 döndür
                    return 0;
                }

            }
            catch (Exception ex)
            {
                // Hata durumunda, uygun bir hata mesajı döndür
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
    }
}
