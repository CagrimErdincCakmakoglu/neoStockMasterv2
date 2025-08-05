using Microsoft.VisualBasic;
using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neoStockMasterv2.Forms
{
    public partial class CompareScreen : Form
    {
        private List<DataGridViewRow> savedRows = new List<DataGridViewRow>();
        int previousValue = 1;

        public CompareScreen(string selectedLanguage)
        {
            InitializeComponent();

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();

            CreateComparisonTable((int)nmrRow.Value);

            // Tüm satırlardaki tarihleri güncel dil formatına göre güncelle
            UpdateAllDateFormats();

            // Önce mevcut satırları kaydet
            SaveCurrentRows();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbRow.Text = LanguageService.GetString("Satır Sayısı Belirle");
            lblRow.Text = LanguageService.GetString("Satır Sayısı");
            grbMethod.Text = LanguageService.GetString("Eylem");
            btnApply.Text = LanguageService.GetString("Uygula");
            btnClear.Text = LanguageService.GetString("Temizle");
            grbTable.Text = LanguageService.GetString("Tablo");

            this.Text = LanguageService.GetString("Karşılaştır");

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

        private void btnApply_Click(object sender, EventArgs e)
        {
            CreateComparisonTable((int)nmrRow.Value);
        }

        private void CreateComparisonTable(int newRowCount)
        {
            // 1. Mevcut seçili satırları kaydet
            SaveCurrentRows();

            // 1.1. Mevcut seçili ürün ID'lerini al
            //var selectedProductIds = new List<int>();
            //foreach (DataGridViewRow row in dgwTable.Rows)
            //{
            //    if (!row.IsNewRow && row.Cells["ProductColumn"]?.Value != null)
            //    {
            //        if (int.TryParse(row.Cells["ProductColumn"].Value.ToString(), out int productId))
            //        {
            //            selectedProductIds.Add(productId);
            //        }
            //    }
            //}
            var selectedProductIds = new List<string>();
            foreach (DataGridViewRow row in dgwTable.Rows)
            {
                if (!row.IsNewRow && row.Cells["ProductColumn"]?.Value != null)
                {
                    selectedProductIds.Add(row.Cells["ProductColumn"].Value.ToString());
                }
            }

            // 2. Tabloyu temizle
            dgwTable.Columns.Clear();
            dgwTable.Rows.Clear();
            dgwTable.DataSource = null;

            // 3. Ürünleri çek ve seçili olanları filtrele
            var productService = new ProductService();
            var products = productService.GetProductsByLoggedInUser() ?? new List<Product>();

            if (products.Count == 0)
            {
                MessageBox.Show("Ürün bulunamadı!");
                return;
            }

            // 4. Dil ayarı
            bool isEnglish = LanguageService.CurrentLanguage == "English";
            string dateFormat = isEnglish ? "MM/dd/yyyy hh:mm tt" : "dd.MM.yyyy HH:mm";

            // 5. ComboBox Sütununu Oluştur
            DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = isEnglish ? "Select Product" : "Ürün Seç",
                Name = "ProductColumn",
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                DataPropertyName = "ProductName"
            };

            comboColumn.DataSource = new BindingSource(products, null);
            comboColumn.DisplayMember = "Name";
            comboColumn.ValueMember = "ID";

            // 6. Diğer sütunlar
            var columns = new[]
            {
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Cost" : "Maliyet", Width = 80, Name = "Cost", ReadOnly = true },
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Currency" : "Para Birimi", Width = 80, Name = "Currency", ReadOnly = true },
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Price" : "Fiyat", Width = 80, Name = "Price", ReadOnly = true },
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Price Currency" : "Fiyat Para Birimi", Width = 80, Name = "PriceCurrency", ReadOnly = true },
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Stock Quantity" : "Stok Adeti", Width = 80, Name = "Stock", ReadOnly = true },
                new DataGridViewTextBoxColumn { HeaderText = isEnglish ? "Added Date" : "Eklenme Tarihi", Width = 120, Name = "AddedDate", ReadOnly = true }
            };

            // 7. Sütunları ekle
            dgwTable.Columns.Add(comboColumn);
            foreach (var col in columns) dgwTable.Columns.Add(col);

            // 8. Kayıtlı satırları geri yükle (yeni satır sayısına kadar)
            int rowsToRestore = Math.Min(savedRows.Count, newRowCount);
            for (int i = 0; i < rowsToRestore; i++)
            {
                //dgwTable.Rows.Add(savedRows[i]);
                var newRow = dgwTable.Rows.Add();
                for (int j = 0; j < savedRows[i].Cells.Count; j++)
                {
                    dgwTable.Rows[newRow].Cells[j].Value = savedRows[i].Cells[j].Value;
                }
            }

            // 9. Eksik kalan satırları ekle
            for (int i = rowsToRestore; i < newRowCount; i++)
            {
                dgwTable.Rows.Add();
            }


            // 10. Event'leri ayarla
            dgwTable.CurrentCellDirtyStateChanged += (sender, e) =>
            {
                if (dgwTable.CurrentCell is DataGridViewComboBoxCell)
                {
                    dgwTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            dgwTable.CellValueChanged += (sender, e) =>
            {
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    var selectedValue = dgwTable.Rows[e.RowIndex].Cells[0].Value;
                    if (selectedValue != null)
                    {
                        var selectedProduct = products.FirstOrDefault(p => p.ID.ToString() == selectedValue.ToString());
                        if (selectedProduct != null)
                        {
                            dgwTable.Rows[e.RowIndex].Cells["Cost"].Value = selectedProduct.Cost;
                            dgwTable.Rows[e.RowIndex].Cells["Currency"].Value = selectedProduct.CostCurrency;
                            dgwTable.Rows[e.RowIndex].Cells["Price"].Value = selectedProduct.Price;
                            dgwTable.Rows[e.RowIndex].Cells["PriceCurrency"].Value = selectedProduct.PriceCurrency;
                            dgwTable.Rows[e.RowIndex].Cells["Stock"].Value = selectedProduct.Stock;
                            dgwTable.Rows[e.RowIndex].Cells["AddedDate"].Value = isEnglish
                                 ? selectedProduct.DateAdded.ToString("MM/dd/yyyy hh:mm tt")
                                 : selectedProduct.DateAdded.ToString("dd.MM.yyyy HH:mm");

                            // ⬇⬇⬇ Bu satır çok önemli
                            UpdateComboBoxes(products);
                        }
                    }
                }

            };

            // 11. Görsel ayarlar
            dgwTable.RowHeadersVisible = false;
            dgwTable.EnableHeadersVisualStyles = false;
            dgwTable.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;

            UpdateComboBoxes(products);

        }




        private void UpdateAllDateFormats()
        {
            bool isEnglish = LanguageService.CurrentLanguage == "English";

            // Şu anki hedef dilin formatı ve kültürü
            string targetFormat = isEnglish ? "MM/dd/yyyy hh:mm tt" : "dd.MM.yyyy HH:mm";
            CultureInfo targetCulture = isEnglish ? new CultureInfo("en-US") : new CultureInfo("tr-TR");

            // Her iki dilin formatı ve kültürü
            string[] allFormats = { "MM/dd/yyyy hh:mm tt", "dd.MM.yyyy HH:mm" };
            CultureInfo[] allCultures = { new CultureInfo("en-US"), new CultureInfo("tr-TR") };

            foreach (DataGridViewRow row in dgwTable.Rows)
            {
                if (!row.IsNewRow && row.Cells["ProductColumn"]?.Value != null && row.Cells["AddedDate"] != null)
                {
                    var cellValue = row.Cells["AddedDate"].Value;

                    DateTime? parsedDate = null;

                    // Eğer zaten DateTime ise direkt al
                    if (cellValue is DateTime dt)
                    {
                        parsedDate = dt;
                    }
                    else if (cellValue is string dateStr && !string.IsNullOrWhiteSpace(dateStr))
                    {
                        // Hem İngilizce hem Türkçe kültürle ve formatla sırayla dene
                        foreach (var culture in allCultures)
                        {
                            foreach (var format in allFormats)
                            {
                                if (DateTime.TryParseExact(dateStr, format, culture, DateTimeStyles.None, out DateTime temp))
                                {
                                    parsedDate = temp;
                                    break;
                                }
                            }
                            if (parsedDate.HasValue)
                                break;
                        }

                        // Son çare genel parse
                        if (!parsedDate.HasValue && DateTime.TryParse(dateStr, out DateTime fallbackDate))
                        {
                            parsedDate = fallbackDate;
                        }
                    }

                    if (parsedDate.HasValue)
                    {
                        row.Cells["AddedDate"].Value = parsedDate.Value.ToString(targetFormat, targetCulture);
                    }
                }
            }
        }


        private void SaveCurrentRows() //SATIRLAR SİLİNMESİN DİYE
        {
            savedRows.Clear();
            foreach (DataGridViewRow row in dgwTable.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dgwTable);
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        newRow.Cells[i].Value = row.Cells[i].Value;
                    }
                    savedRows.Add(newRow);
                }
            }
        }

        private void UpdateComboBoxes(List<Product> allProducts)
        {
            var selectedIds = new HashSet<string>();

            foreach (DataGridViewRow row in dgwTable.Rows)
            {
                if (!row.IsNewRow)
                {
                    var selectedValue = row.Cells["ProductColumn"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(selectedValue))
                        selectedIds.Add(selectedValue);
                }
            }

            foreach (DataGridViewRow row in dgwTable.Rows)
            {
                if (!row.IsNewRow)
                {
                    var comboCell = row.Cells["ProductColumn"] as DataGridViewComboBoxCell;
                    if (comboCell == null)
                    {
                        comboCell = new DataGridViewComboBoxCell();
                        row.Cells["ProductColumn"] = comboCell;
                    }

                    var currentSelectedId = comboCell.Value?.ToString();

                    var filteredProducts = allProducts
                        .Where(p => !selectedIds.Contains(p.ID.ToString()) || p.ID.ToString() == currentSelectedId)
                        .ToList();

                    // ✨ ŞU KISIM KRİTİK ✨
                    comboCell.DisplayMember = "Name";
                    comboCell.ValueMember = "ID";
                    comboCell.DataSource = filteredProducts;
                }
            }
        }

        private void nmrRow_ValueChanged(object sender, EventArgs e)
        {
            var productService = new ProductService();
            var userProducts = productService.GetProductsByLoggedInUser();
            int maxAllowed = userProducts.Count;

            if (nmrRow.Value > maxAllowed)
            {
                string message;
                string caption;

                if (LanguageService.CurrentLanguage == "Türkçe")
                {
                    message = $"Kayıtlı ürün sayınızdan fazla ürün girdiniz.\n\nKayıtlı ürün sayınız: {maxAllowed}";
                    caption = "Uyarı";
                }
                else // İngilizce varsay
                {
                    message = $"You entered more products than you have.\n\nYour product count: {maxAllowed}";
                    caption = "Warning";
                }

                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                nmrRow.Value = previousValue;
            }
            else
            {
                previousValue = (int)nmrRow.Value;
            }
        }

        private void CompareScreen_Load(object sender, EventArgs e)
        {
            var productService = new ProductService();
            var userProducts = productService.GetProductsByLoggedInUser();
            int maxAllowed = userProducts.Count;

            nmrRow.Maximum = maxAllowed > 0 ? maxAllowed : 1; // Minimum 1 olsun
            nmrRow.Value = 1;
            previousValue = 1;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Mesajlar
            string message = isTurkish
                ? "Tüm satırları temizlemek istediğinize emin misiniz?"
                : "Are you sure you want to clear all rows?";

            string caption = isTurkish
                ? "Onay Gerekiyor"
                : "Confirmation Required";

            // Evet / Hayır (Yes / No) kutusu
            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 1. DataGridView temizle
                dgwTable.DataSource = null;
                dgwTable.Rows.Clear();
                dgwTable.Columns.Clear();

                // 2. NumericUpDown değeri 1 yap
                nmrRow.Value = 1;

                // 3. Kaydedilmiş satırları temizle
                savedRows.Clear();

                // 4. Yeni tabloyu 1 satırla oluştur
                CreateComparisonTable(1);
            }
        }
    }
}
