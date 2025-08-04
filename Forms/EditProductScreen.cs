using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neoStockMasterv2.Forms
{
    public partial class EditProductScreen : Form
    {
        public static User LoggedInUser { get; set; }
        private readonly Point groupBoxLocation = new Point(12, 160);
        private ProductService _productService = new ProductService();
        public event EventHandler ProductUpdated;


        public EditProductScreen(string selectedLanguage)
        {
            InitializeComponent();
            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
        }

        private void EditProductScreen_Load(object sender, EventArgs e)
        {
            grbEdit.Visible = false;
            grbStock.Visible = false;

            rdbEdit.Checked = false;
            rdbUpdStock.Checked = false;

            grbDelete.Visible = false;
            grbDelete.Location = groupBoxLocation;

            rdbDelete.Checked = false;

            grbEdit.Location = groupBoxLocation;
            grbStock.Location = groupBoxLocation;

            SetGroupBoxesEnabledState();
            LoadProductsToComboBox();
            AdjustFormHeight();
            FillCurrencyComboBoxes();
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            SetRdbUpdStockLocation();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbProductList.Text = LanguageService.GetString("Ürünler");
            grbMethod.Text = LanguageService.GetString("Yöntem");
            rdbEdit.Text = LanguageService.GetString("Düzenle");
            rdbUpdStock.Text = LanguageService.GetString("Stok Ekle");
            grbEdit.Text = LanguageService.GetString("Ürün Bilgilerini Düzenle");
            lblName.Text = LanguageService.GetString("İsim");
            lblCost.Text = LanguageService.GetString("Maliyet");
            lblPrice.Text = LanguageService.GetString("Fiyat");
            lblStock.Text = LanguageService.GetString("Stok");
            lblPercent.Text = LanguageService.GetString("Kâr Yüzdesi");
            btnClear.Text = LanguageService.GetString("Temizle");
            btnOrijinal.Text = LanguageService.GetString("Orijinal");
            btnUpdate.Text = LanguageService.GetString("Güncelle");
            grbStock.Text = LanguageService.GetString("Stok Ekle");
            lblStName.Text = LanguageService.GetString("İsim");
            lblStStok.Text = LanguageService.GetString("Stok");
            lblStAdd.Text = LanguageService.GetString("Eklenecek");
            btnStClear.Text = LanguageService.GetString("Temizle");
            btnStAdd.Text = LanguageService.GetString("Ekle");
            rdbDelete.Text = LanguageService.GetString("Sil");
            grbDelete.Text = LanguageService.GetString("Ürünü Sil");
            lblDelName.Text = LanguageService.GetString("İsim");
            lblDelCost.Text = LanguageService.GetString("Maliyet");
            lblDelPrice.Text = LanguageService.GetString("Fiyat");
            lblDelStock.Text = LanguageService.GetString("Stok");
            btnDel.Text = LanguageService.GetString("Sil");

            this.Text = LanguageService.GetString("Ürün Düzenle / Sil");

            UpdateSelectedLanguage();
        }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
        }

        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("Türkçe");
            UpdateSelectedLanguage();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("English");
            UpdateSelectedLanguage();
        }

        private void AdjustFormHeight()
        {
            if (rdbEdit.Checked)
            {
                this.Height = grbMethod.Bottom + 280;
            }
            else if (rdbUpdStock.Checked)
            {
                this.Height = grbMethod.Bottom + 210;
            }
            else if (rdbDelete.Checked)
            {
                this.Height = grbMethod.Bottom + 230; // Silme için sade bir görünüm
            }
            else
            {
                this.Height = grbMethod.Bottom + 50;
            }
        }

        private void rdbEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEdit.Checked)
            {
                grbEdit.Visible = rdbEdit.Checked;
                grbStock.Visible = !rdbEdit.Checked;

                LoadSelectedProductInfo();
                AdjustFormHeight();

            }
        }

        private void rdbUpdStock_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUpdStock.Checked)
            {
                grbEdit.Visible = !rdbUpdStock.Checked;
                grbStock.Visible = rdbUpdStock.Checked;

                LoadSelectedProductInfo();
                AdjustFormHeight();
            }
        }

        private void LoadProductsToComboBox() // ÜRÜNLERİ YÜKLEME KISMI
        {
            cmbProducts.Items.Clear();

            var products = _productService.GetProductsByLoggedInUser()
                                          .OrderBy(p => p.Name)
                                          .ToList();

            foreach (var product in products)
            {
                cmbProducts.Items.Add(product.Name);
            }

            if (cmbProducts.Items.Count > 0)
            {
                cmbProducts.SelectedIndex = -1; // Hiçbiri seçili olmasın
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGroupBoxesEnabledState();
            LoadSelectedProductInfo();
        }

        private void SetGroupBoxesEnabledState()
        {
            bool isProductSelected = cmbProducts.SelectedIndex != -1;

            grbMethod.Enabled = isProductSelected;
            grbEdit.Enabled = isProductSelected;
            grbStock.Enabled = isProductSelected;
        }

        private void LoadSelectedProductInfo()
        {
            // Seçilen ürünün ismini al
            string selectedProductName = cmbProducts.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(selectedProductName))
                return;

            // Ürünü ProductService üzerinden bul
            ProductService service = new ProductService();
            var selectedProduct = service.GetProductsByLoggedInUser()
                                         .FirstOrDefault(p => p.Name.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase));

            if (selectedProduct == null)
            {
                string errorMessage = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Seçilen ürün bulunamadı."
                    : "Selected product could not be found.";

                string errorCaption = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Hata"
                    : "Error";

                MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ID'yi değişkende tut (gerekirse global değişken olabilir)
            string selectedProductId = selectedProduct.ID;

            // rdbEdit seçiliyse bilgileri doldur
            if (rdbEdit.Checked)
            {
                txtName.Text = selectedProduct.Name;
                nmrCost.Value = selectedProduct.Cost;
                nmrPrice.Value = selectedProduct.Price;
                nmrStock.Value = selectedProduct.Stock;

                // Para birimlerini ayarla
                cmbCost.SelectedItem = selectedProduct.CostCurrency;
                cmbPrice.SelectedItem = selectedProduct.PriceCurrency;

                // Alanlar aktif olsun
                txtName.Enabled = true;
                nmrCost.Enabled = true;
                nmrPrice.Enabled = true;
                nmrStock.Enabled = true;
                cmbCost.Enabled = true;
                cmbPrice.Enabled = true;
            }
            // rdbUpdStock seçiliyse sadece ad ve stok gösterilsin
            else if (rdbUpdStock.Checked)
            {
                txtUpgName.Text = selectedProduct.Name;
                nmrUpgStock.Value = selectedProduct.Stock;

                // Bu alanlar kapalı olsun
                txtUpgName.Enabled = false;
                nmrUpgStock.Enabled = false;
            }
            // rdbDelete seçiliyse sadece ad ve stok gösterilsin
            else if (rdbDelete.Checked)
            {
                txtDelName.Text = selectedProduct.Name;
                nmrDelCost.Value = selectedProduct.Cost;
                nmrDelPrice.Value = selectedProduct.Price;
                nmrDelStock.Value = selectedProduct.Stock;

                cmbDelCost.SelectedItem = selectedProduct.CostCurrency;
                cmbDelPrice.SelectedItem = selectedProduct.PriceCurrency;

                // Bu alanlar kapalı olsun
                txtDelName.Enabled = false;
                nmrDelCost.Enabled = false;
                nmrDelPrice.Enabled = false;
                nmrDelStock.Enabled = false;
                cmbDelCost.Enabled = false;
                cmbDelPrice.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            nmrCost.Value = 0;
            nmrPrice.Value = 0;
            nmrStock.Value = 0;
        }

        private void btnOrijinal_Click(object sender, EventArgs e)
        {
            ProductService service = new ProductService();
            string selectedProductName = cmbProducts.SelectedItem?.ToString();

            var selectedProduct = service.GetProductsByLoggedInUser()
                                         .FirstOrDefault(p => p.Name.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase));
            string selectedProductId = selectedProduct.ID;

            if (selectedProduct != null)
            {
                txtName.Text = selectedProduct.Name;
                nmrCost.Value = selectedProduct.Cost;
                nmrPrice.Value = selectedProduct.Price;
                nmrStock.Value = selectedProduct.Stock;
                // Para birimlerini ayarla
                cmbCost.SelectedItem = selectedProduct.CostCurrency;
                cmbPrice.SelectedItem = selectedProduct.PriceCurrency;
                // Alanlar aktif olsun
                txtName.Enabled = true;
                nmrCost.Enabled = true;
                nmrPrice.Enabled = true;
                nmrStock.Enabled = true;

                string message = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Orijinal ürün bilgileri yüklendi."
                    : "Original product information has been loaded.";

                string caption = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Bilgi"
                    : "Information";

                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string errorMessage = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Seçilen ürün bulunamadı."
                    : "Selected product could not be found.";

                string errorCaption = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Hata"
                    : "Error";

                MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillCurrencyComboBoxes()
        {
            List<string> currencies = new List<string>
    {
        "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları",
        "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "100 Japon Yeni", "Kuveyt Dinarı", "Güney Afrika Randı", "Arnavutluk Leki",
        "Arjantin Pesosu", "Azerbaycan Manatı", "Bosna-Hersek Markı", "Bulgar Levası", "Bahreyn Dinarı", "Brezilya Reali", "Belarus Rublesi",
        "Şili Pesosu", "Çin Yuanı", "Kolombiya Pesosu", "Kosta Rika Kolonu", "Çek Korunası", "Sepet Kur", "Cezayir Dinarı", "Mısır Lirası",
        "Gürcistan Larisi", "Hong Kong Doları", "Hırvat Kunası", "Macar Forinti", "Endonezya Rupiahi", "İsrail Şekeli", "Hindistan Rupisi",
        "Irak Dinarı", "İran Riyali", "İzlanda Kronası", "Ürdün Dinarı", "Güney Kore Wonu", "Kazan Tengesi", "Lübnan Lirası", "Sri Lanka Rupisi",
        "Litvanya Litası", "Letonya Latsı", "Libya Dinarı", "Fas Dirhemi", "Moldovya Leusu", "Makedon Dinarı", "Meksika Pesosu", "Malezya Ringgiti",
        "Yeni Zelanda Doları", "Umman Riyali", "Peru İnti", "Filipinler Pesosu", "Pakistan Rupisi", "Polonya Zlotisi", "Katar Riyali",
        "Romanya Leyi", "Sırbistan Dinarı", "Suudi Arabistan Riyali", "Singapur Doları", "Suriye Lirası", "Tayland Bahtı", "Tunus Dinarı",
        "Yeni Tayvan Doları", "Ukrayna Grivnası", "Uruguay Pesosu"
    };

            // Türk Lirası'nı en başa al, geri kalanları alfabetik sırala
            string baseCurrency = "Türk Lirası";
            List<string> sortedCurrencies = currencies.OrderBy(c => c).ToList();
            sortedCurrencies.Insert(0, baseCurrency);

            // ComboBox'ları temizle ve tekrar doldur
            cmbCost.Items.Clear();
            cmbDelCost.Items.Clear();
            cmbPrice.Items.Clear();
            cmbDelPrice.Items.Clear();

            foreach (string currency in sortedCurrencies)
            {
                cmbCost.Items.Add(currency);
                cmbDelCost.Items.Add(currency);
                cmbPrice.Items.Add(currency);
                cmbDelPrice.Items.Add(currency);
            }

            cmbCost.SelectedIndex = 0;
            cmbPrice.SelectedIndex = 0;

            int maxWidth = 0;
            using (Graphics g = cmbCost.CreateGraphics())
            {
                foreach (string currency in sortedCurrencies)
                {
                    cmbCost.Items.Add(currency);
                    cmbDelCost.Items.Add(currency);
                    cmbPrice.Items.Add(currency);
                    cmbDelPrice.Items.Add(currency);

                    // En uzun string'in piksel genişliğini ölç
                    int itemWidth = (int)g.MeasureString(currency, cmbCost.Font).Width;
                    if (itemWidth > maxWidth)
                        maxWidth = itemWidth;
                }
            }

            // Sağdan kaydırma çubuğu için biraz ekstra ekle (isteğe göre artırılabilir)
            maxWidth += 30;

            cmbCost.DropDownWidth = maxWidth;
            cmbPrice.DropDownWidth = maxWidth;

            cmbCost.SelectedIndex = 0;
            cmbPrice.SelectedIndex = 0;
        }

        private void CalculateProfitAndPrice()
        {
            // Eğer para birimleri farklıysa kar hesabı yapılmasın
            if (cmbCost.SelectedItem?.ToString() != cmbPrice.SelectedItem?.ToString())
            {
                nmrPercent.Value = 0;
                nmrPercent.Enabled = false;
                return;
            }

            // Eşitse, kar hesabı açılır
            nmrPercent.Enabled = true;

            decimal cost = nmrCost.Value;
            decimal price = nmrPrice.Value;
            decimal percent = nmrPercent.Value;

            // Hangi alanın odakta olduğuna göre karar ver
            if (nmrPercent.Focused)
            {
                // Kar yüzdesinden fiyatı hesapla
                price = cost + (cost * percent / 100);
                nmrPrice.Value = Math.Round(price, 2);
            }
            else
            {
                // Cost veya Price değiştiyse ya da para birimleri yeni eşitlendiğinde bu hesap çalışır
                if (cost > 0)
                {
                    percent = ((price - cost) / cost) * 100;
                    nmrPercent.Value = Math.Round(percent, 2);
                }
                else
                {
                    nmrPercent.Value = 0;
                }
            }
        }

        private void nmrCost_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void nmrPrice_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void nmrCost_ValueChanged(object sender, EventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void nmrPrice_ValueChanged(object sender, EventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void cmbCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void cmbPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void nmrPercent_ValueChanged(object sender, EventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void nmrPercent_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateProfitAndPrice();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                MessageBox.Show("Lütfen güncellenecek ürünü seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string oldProductName = cmbProducts.SelectedItem.ToString();

            var oldProduct = _productService.GetProductsByLoggedInUser()
                                            .FirstOrDefault(p => p.Name.Equals(oldProductName, StringComparison.OrdinalIgnoreCase));

            if (oldProduct == null)
            {
                string notFoundMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Güncellenecek ürün bulunamadı."
                    : "Product to be updated could not be found.";

                string notFoundCaption = LanguageService.CurrentLanguage == "Türkçe" ? "Hata" : "Error";

                MessageBox.Show(notFoundMsg, notFoundCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Yeni bilgileri al
            string newName = txtName.Text.Trim();
            decimal newCost = nmrCost.Value;
            decimal newPrice = nmrPrice.Value;
            int newStock = (int)nmrStock.Value;
            string newCostCurrency = cmbCost.SelectedItem?.ToString();
            string newPriceCurrency = cmbPrice.SelectedItem?.ToString();

            // Ürün adı boşsa engelle
            if (string.IsNullOrWhiteSpace(newName))
            {
                string nameEmptyMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Ürün ismi boş olamaz!"
                    : "Product name cannot be empty!";

                string nameEmptyCaption = LanguageService.CurrentLanguage == "Türkçe" ? "Uyarı" : "Warning";

                MessageBox.Show(nameEmptyMsg, nameEmptyCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kar yüzdesi hesapla
            decimal profitPercent = newCost > 0 ? ((newPrice - newCost) / newCost) * 100 : 0;

            string message;
            string caption;

            if (LanguageService.CurrentLanguage == "Türkçe")
            {
                message = $"Aşağıdaki değişiklikleri yapmak istediğinizden emin misiniz?\n\n" +
                          $"İsim: {oldProduct.Name} → {newName}\n" +
                          $"Maliyet: {oldProduct.Cost} {oldProduct.CostCurrency} → {newCost} {newCostCurrency}\n" +
                          $"Fiyat: {oldProduct.Price} {oldProduct.PriceCurrency} → {newPrice} {newPriceCurrency}\n" +
                          $"Stok: {oldProduct.Stock} → {newStock}";

                if (profitPercent < 0)
                {
                    message += "\n\n⚠️ Ürün zararına satılıyor! (Fiyat < Maliyet)";
                }

                caption = "Ürün Güncelleme Onayı";
            }
            else
            {
                message = $"Are you sure you want to make the following changes?\n\n" +
                          $"Name: {oldProduct.Name} → {newName}\n" +
                          $"Cost: {oldProduct.Cost} {oldProduct.CostCurrency} → {newCost} {newCostCurrency}\n" +
                          $"Price: {oldProduct.Price} {oldProduct.PriceCurrency} → {newPrice} {newPriceCurrency}\n" +
                          $"Stock: {oldProduct.Stock} → {newStock}";

                if (profitPercent < 0)
                {
                    message += "\n\n⚠️ Product is being sold at a loss! (Price < Cost)";
                }

                caption = "Product Update Confirmation";
            }

            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                string cancelMessage = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Ürün güncelleme işlemi iptal edildi."
                    : "Product update operation was cancelled.";

                string cancelCaption = LanguageService.CurrentLanguage == "Türkçe" ? "İptal" : "Cancelled";

                MessageBox.Show(cancelMessage, cancelCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Yeni ürün nesnesini oluştur
            Product updatedProduct = new Product
            {
                Name = newName,
                Cost = newCost,
                Price = newPrice,
                Stock = newStock,
                CostCurrency = newCostCurrency,
                PriceCurrency = newPriceCurrency,
                DateAdded = DateTime.Now,
            };

            _productService.UpdateProduct(oldProductName, updatedProduct);

            // Güncelleme sonrası başarı mesajı göster
            string successMsg = LanguageService.CurrentLanguage == "Türkçe"
                ? $"Ürün '{updatedProduct.Name}' başarıyla güncellendi."
                : $"Product '{updatedProduct.Name}' has been successfully updated.";

            string successCaption = LanguageService.CurrentLanguage == "Türkçe"
                ? "Başarılı"
                : "Success";

            MessageBox.Show(successMsg, successCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Event'i tetikle – diğer formlara haber ver
            ProductUpdated?.Invoke(this, EventArgs.Empty);

            LoadProductsToComboBox();
            cmbProducts.SelectedItem = updatedProduct.Name;
        }

        private void btnStClear_Click(object sender, EventArgs e)
        {
            nmrUpgAddStock.Value = 0;
        }

        private void btnStAdd_Click(object sender, EventArgs e)
        {
            // Seçili ürün kontrolü
            if (cmbProducts.SelectedItem == null)
            {
                string noProductMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Lütfen bir ürün seçin."
                    : "Please select a product.";
                MessageBox.Show(noProductMsg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedProductName = cmbProducts.SelectedItem.ToString();
            // Ürün ismine göre ilgili Product nesnesini bul
            var selectedProduct = _productService.GetProductsByLoggedInUser()
                                                .FirstOrDefault(p => p.Name.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase));


            if (selectedProduct == null)
            {
                string invalidProductMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Geçersiz ürün seçildi."
                    : "Invalid product selected.";
                MessageBox.Show(invalidProductMsg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Eklenecek miktar kontrolü
            int amountToAdd = (int)nmrUpgAddStock.Value;
            if (amountToAdd <= 0)
            {
                string invalidAmountMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Eklenecek miktar 0 veya daha az olamaz."
                    : "Amount to add cannot be 0 or less.";
                MessageBox.Show(invalidAmountMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string confirmMessage = LanguageService.CurrentLanguage == "Türkçe"
                ? $"Seçili ürünün stokuna {amountToAdd} adet eklensin mi?"
                : $"Add {amountToAdd} units to the selected product's stock?";

            DialogResult result = MessageBox.Show(confirmMessage, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            bool success = _productService.AddStockToProduct(selectedProduct.Name, amountToAdd);

            if (success)
            {
                string successMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Stok başarıyla eklendi."
                    : "Stock added successfully.";
                MessageBox.Show(successMsg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProductUpdated?.Invoke(this, EventArgs.Empty); // güncel liste
                nmrUpgAddStock.Value = 0;

                // Güncel stok değerini al
                var updatedProduct = _productService.GetProductsByLoggedInUser()
                                                    .FirstOrDefault(p => p.Name.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase));
                if (updatedProduct != null)
                {
                    nmrUpgStock.Value = updatedProduct.Stock;
                }
            }
            else
            {
                string errorMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Stok eklenemedi."
                    : "Failed to add stock.";
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rdbDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDelete.Checked)
            {
                grbEdit.Visible = false;
                grbStock.Visible = false;
                grbDelete.Visible = true;

                LoadSelectedProductInfo(); // Sadece seçili ürün varsa ad/fiyat gösterimi
            }
            else
            {
                grbDelete.Visible = false;
            }

            AdjustFormHeight();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                string noProductMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Lütfen bir ürün seçin."
                    : "Please select a product.";
                MessageBox.Show(noProductMsg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedProductName = cmbProducts.SelectedItem.ToString();

            // Ürün ismine göre ilgili Product nesnesini bul
            var selectedProduct = _productService.GetProductsByLoggedInUser()
                                                 .FirstOrDefault(p => p.Name.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase));

            if (selectedProduct == null)
            {
                string notFoundMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Ürün bulunamadı."
                    : "Product not found.";
                MessageBox.Show(notFoundMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Onay mesajı
            string confirmMsg = LanguageService.CurrentLanguage == "Türkçe"
                ? $"'{selectedProduct.Name}' adlı ürünü silmek istediğinize emin misiniz?"
                : $"Are you sure you want to delete the product '{selectedProduct.Name}'?";

            DialogResult result = MessageBox.Show(confirmMsg,
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _productService.DeleteProduct(selectedProduct.ID);

                string successMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Ürün başarıyla silindi."
                    : "Product deleted successfully.";
                MessageBox.Show(successMsg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ComboBox'ı güncelle
                cmbProducts.Items.Clear();
                var updatedProducts = _productService.GetProductsByLoggedInUser();
                foreach (var product in updatedProducts)
                {
                    cmbProducts.Items.Add(product.Name);
                }
                cmbProducts.SelectedItem = null; // Hiçbir ürün seçili olmasın

                // GroupBox içindeki RadioButton'ların seçimini kaldır
                foreach (var control in grbMethod.Controls)
                {
                    if (control is RadioButton rb)
                    {
                        rb.Checked = false;
                    }
                }

                // TextBox temizle
                txtDelName.Clear();

                // NumericUpDown sıfırla
                nmrDelCost.Value = 0;
                nmrDelPrice.Value = 0;
                nmrDelStock.Value = 0;

                // ComboBox'ları sıfırla
                cmbDelCost.SelectedItem = null;
                cmbDelPrice.SelectedItem = null;

                grbMethod.Enabled = false;

                // ComboBox'ı ve varsa DataGridView'i güncelle
                ProductUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                string cancelMsg = LanguageService.CurrentLanguage == "Türkçe"
                    ? "Silme işlemi iptal edildi."
                    : "Deletion cancelled.";
                MessageBox.Show(cancelMsg, "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetRdbUpdStockLocation()
        {
            if (LanguageService.CurrentLanguage == "Türkçe")
            {
                rdbUpdStock.Location = new Point(106, 22);
            }
            else if (LanguageService.CurrentLanguage == "English")
            {
                rdbUpdStock.Location = new Point(90, 22);
            }
        }
    }

}
