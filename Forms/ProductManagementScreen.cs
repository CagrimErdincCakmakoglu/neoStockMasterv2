using OfficeOpenXml;
using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OfficeOpenXml.Table;
using static System.Windows.Forms.DataFormats;
using System.Globalization;


namespace neoStockMasterv2.Forms
{
    public partial class ProductManagementScreen : Form
    {
        public static User LoggedInUser { get; set; }
        private bool isMouseOverPictureBox = false;
        private string tooltipMessage = "";
        private ProductService _productService;

        public ProductManagementScreen(string selectedLanguage)
        {
            InitializeComponent();

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
            PopulateOrderComboBox();
            _productService = new ProductService(); //DI yöntemi

        }

        private void ProductManagementScreen_Load(object sender, EventArgs e)
        {
            CheckExcelInstallation();
            SetPersistentToolTip(pbInfo);
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            PopulateOrderComboBox();
            LoadUserProducts();
            ApplySorting();

        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            lblActivity.Text = LanguageService.GetString("İşlemi Seç");
            btnAddProduct.Text = LanguageService.GetString("Ürün Ekle");
            btnEditProduct.Text = LanguageService.GetString("Ürün Düzenle / Sil");
            btnToExcel.Text = LanguageService.GetString("Excel'e Aktar");
            btnMaximize.Text = LanguageService.GetString("Genişlet");
            btnCompare.Text = LanguageService.GetString("Karşılaştır");
            grbOrdery.Text = LanguageService.GetString("Sıralama");
            grbProducts.Text = LanguageService.GetString("Ürünler");


            this.Text = LanguageService.GetString("Ürün İşlemleri");

            // Excel butonunun tooltip'ini güncelle (eğer devre dışıysa)
            if (!btnToExcel.Enabled)
            {
                string tooltipMessage = LanguageService.CurrentLanguage == "Türkçe" ?
                    "Excel yüklü olmadığı için bu özellik devre dışı bırakıldı" :
                    "This feature is disabled because Excel is not installed";

                ttExcel.SetToolTip(btnToExcel, tooltipMessage);
            }

            AdjustLabelPosition();
            UpdateSelectedLanguage();
        }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;

            SetPersistentToolTip(pbInfo);
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

        private void PopulateOrderComboBox()
        {
            cmbOrdery.Items.Clear();
            if (LanguageService.CurrentLanguage == "Türkçe")
            {
                cmbOrdery.Items.AddRange(new string[]
                {
                    "A'dan Z'ye", "Z'den A'ya", "Ucuzdan Pahalıya", "Pahalıdan Ucuza", "Eskiden Yeniye", "Yeniden Eskiye", "Azdan Çoğa", "Çoktan Aza"
                });
            }
            else
            {
                cmbOrdery.Items.AddRange(new string[]
                {
                    "A to Z", "Z to A", "Cheapest to Expensive", "Expensive to Cheapest", "Oldest to Newest", "Newest to Oldest", "Least to Most", "Most to Least"
                });
            }
            cmbOrdery.SelectedIndex = 0;
        }

        private void AdjustLabelPosition()
        {
            if (LanguageService.CurrentLanguage == "English")
            {
                lblActivity.Left = 90; // İngilizce için belirlediğin konum
            }
            else
            {
                lblActivity.Left = 110; // 2Türkçe için belirlediğin konum
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            AddProductScreen addProductScreen = new AddProductScreen(currentLanguage);

            // MainMenu'nun IsAlwaysOnTop özelliğini kontrol et
            if (Owner is MainMenu mainMenu)
            {
                addProductScreen.TopMost = mainMenu.IsAlwaysOnTop;
            }

            // Event aboneliği
            addProductScreen.ProductAdded += (s, ev) =>
            {
                if (InvokeRequired)
                {
                    Invoke(new System.Action(() =>
                    {
                        LoadUserProducts();
                    }));
                }
                else
                {
                    LoadUserProducts();
                }
            };

            addProductScreen.ShowDialog();
        }

        private void LoadUserProducts()
        {
            ProductService productService = new ProductService();
            var userProducts = productService.GetProductsByLoggedInUser();

            if (userProducts == null || userProducts.Count == 0)
            {
                dgwProducts.DataSource = null;
                return;
            }

            var productTable = new System.Data.DataTable();

            bool isEnglish = LanguageService.CurrentLanguage == "English";

            // Kolon başlıklarını dil bazlı ayarla
            productTable.Columns.Add(isEnglish ? "Name" : "İsim");
            productTable.Columns.Add(isEnglish ? "Cost" : "Maliyet");
            productTable.Columns.Add(isEnglish ? "Cost Currency" : "Maliyet Para Birimi");
            productTable.Columns.Add(isEnglish ? "Price" : "Fiyat");
            productTable.Columns.Add(isEnglish ? "Price Currency" : "Fiyat Para Birimi");
            productTable.Columns.Add(isEnglish ? "Stock Quantity" : "Stok Adeti");
            productTable.Columns.Add(isEnglish ? "Added Date" : "Eklenme Tarihi");

            // Tarih formatı dil bazlı
            string dateFormat = isEnglish ? "MM-dd-yyyy" : "dd-MM-yyyy";

            foreach (var product in userProducts)
            {
                productTable.Rows.Add(
                    product.Name,
                    product.Cost,
                    product.CostCurrency,
                    product.Price,
                    product.PriceCurrency,
                    product.Stock,
                    product.DateAdded.ToString(dateFormat)
                );
            }

            productTable.DefaultView.Sort = productTable.Columns[0].ColumnName + " ASC";
            dgwProducts.DataSource = productTable.DefaultView.ToTable();

            dgwProducts.DataSource = productTable;

            // Başlıkların rengi kırmızı
            dgwProducts.EnableHeadersVisualStyles = false; // Temayı devre dışı bırak ki renk çalışsın
            dgwProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.Red;

            // Varsayılan seçimleri kaldır
            dgwProducts.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgwProducts.MultiSelect = false;
        }

        private void dgwProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Önce tüm hücreleri resetle
            foreach (DataGridViewRow row in dgwProducts.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            // Tıklanan satırın tamamını LightBlue yap
            foreach (DataGridViewCell cell in dgwProducts.Rows[e.RowIndex].Cells)
            {
                cell.Style.BackColor = Color.LightBlue;
            }

            // Sütunda sadece tıklanan satıra kadar olan kısmı LightBlue yap
            for (int i = 0; i <= e.RowIndex; i++)
            {
                dgwProducts.Rows[i].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
            }

            // Tıklanan hücreyi beyaz yap
            dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;

        }

        private void CheckExcelInstallation()
        {
            bool isExcelInstalled = IsExcelInstalled();
            btnToExcel.Enabled = isExcelInstalled;

            // İsteğe bağlı: Eğer Excel yüklü değilse tooltip ile bilgi verebilirsiniz
            if (!isExcelInstalled)
            {
                string tooltipMessage = LanguageService.CurrentLanguage == "Türkçe" ?
                    "Excel yüklü olmadığı için bu özellik devre dışı bırakıldı" :
                    "This feature is disabled because Excel is not installed";

                ttExcel.SetToolTip(btnToExcel, tooltipMessage);
            }
        }

        private bool IsExcelInstalled()
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Excel.Application"))
                {
                    return key != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            // Dil kontrolüne göre onay mesajı
            string message = LanguageService.CurrentLanguage == "Türkçe" ?
                "Ürünler tablosunu Excel'e aktarmak istediğinize emin misiniz?" :
                "Are you sure you want to export the products table to Excel?";

            string caption = LanguageService.CurrentLanguage == "Türkçe" ?
                "Excel'e Aktar" :
                "Export to Excel";

            // Kullanıcı onayı al
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ExportToExcel();
            }

        }

        private void ExportToExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                string userName = LoggedInUser.Name;
                string fileName = LanguageService.CurrentLanguage == "Türkçe" ?
                    $"{userName}'nın Ürünleri.xlsx" :
                    $"{userName}'s Products.xlsx";

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Products");

                    // Oluşturulma tarihi sağ üstte I1 ve J1
                    string dateLabel = LanguageService.CurrentLanguage == "Türkçe" ?
                        "Oluşturma Tarihi:" : "Creation Date:";
                    worksheet.Cells[1, 9].Value = dateLabel; // I1
                    worksheet.Cells[1, 10].Value = DateTime.Now; // J1
                    worksheet.Cells[1, 10].Style.Numberformat.Format =
                        LanguageService.CurrentLanguage == "Türkçe" ?
                        "dd.MM.yyyy HH:mm" : "MM/dd/yyyy HH:mm";

                    // I ve J sütun genişliklerini ayarla (tarih sığsın)
                    worksheet.Column(9).Width = dateLabel.Length + 3;
                    worksheet.Column(10).Width = 22; // Tarih için sabit genişlik

                    // Başlıklar A1'den başlasın
                    int startRow = 1;
                    for (int i = 0; i < dgwProducts.Columns.Count; i++)
                    {
                        worksheet.Cells[startRow, i + 1].Value = dgwProducts.Columns[i].HeaderText;
                    }

                    // Veriler A2'den başlar
                    for (int i = 0; i < dgwProducts.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgwProducts.Columns.Count; j++)
                        {
                            worksheet.Cells[startRow + i + 1, j + 1].Value =
                                dgwProducts.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    // Tablo Aralığı (Sadece ürün verileri)
                    int totalRows = dgwProducts.Rows.Count + 1;
                    int totalCols = dgwProducts.Columns.Count;
                    var tableRange = worksheet.Cells[startRow, 1, startRow + dgwProducts.Rows.Count, totalCols];

                    // Tablo oluştur (Excel Table)
                    var table = worksheet.Tables.Add(tableRange, "ProductsTable");
                    table.ShowHeader = true;
                    table.TableStyle = TableStyles.Medium9;

                    // Başlık stilleri
                    using (var headerCells = worksheet.Cells[startRow, 1, startRow, totalCols])
                    {
                        headerCells.Style.Font.Bold = true;
                        headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        headerCells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        headerCells.Style.WrapText = false; // Alt alta olmasını istemiyoruz
                    }

                    // Satır yüksekliği sabit
                    worksheet.Row(1).Height = 18;

                    // 🔍 Genişlikleri hesapla
                    for (int col = 1; col <= totalCols; col++)
                    {
                        int maxLength = 0;

                        // Başlık metni
                        var headerText = worksheet.Cells[startRow, col].Value?.ToString();
                        if (!string.IsNullOrEmpty(headerText))
                            maxLength = headerText.Length;

                        // Satırlardaki hücreler
                        for (int row = startRow + 1; row <= startRow + dgwProducts.Rows.Count; row++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
                            if (!string.IsNullOrEmpty(cellValue) && cellValue.Length > maxLength)
                            {
                                maxLength = cellValue.Length;
                            }
                        }

                        // Sıralama simgesi çakışmasından dolayı fazladan boşluk
                        int extraPadding = (!string.IsNullOrEmpty(headerText) && headerText.Length >= maxLength) ? 7 : 3;

                        worksheet.Column(col).Width = maxLength + extraPadding;
                    }

                    // Border sadece tablo için
                    var tableCells = worksheet.Cells[startRow, 1, startRow + dgwProducts.Rows.Count, totalCols];
                    tableCells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    tableCells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    tableCells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    tableCells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    // Tarih etiketi stil
                    worksheet.Cells[1, 9].Style.Font.Bold = true;


                    // Dosyayı kaydet
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        FileName = fileName
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);

                        // 🔐 ŞİFRE KORUMA BURADA
                        package.Workbook.Protection.LockStructure = true;
                        package.Workbook.Protection.SetPassword("1234");

                        foreach (var sheet in package.Workbook.Worksheets)
                        {
                            sheet.Protection.IsProtected = true;
                            sheet.Protection.SetPassword(LoggedInUser.Password);

                            sheet.Protection.AllowSelectLockedCells = true;
                            sheet.Protection.AllowSelectUnlockedCells = true;
                            sheet.Protection.AllowEditObject = false;
                            sheet.Protection.AllowEditScenarios = false;
                            sheet.Protection.AllowAutoFilter = false;
                            sheet.Protection.AllowSort = false;
                            sheet.Protection.AllowDeleteColumns = false;
                            sheet.Protection.AllowDeleteRows = false;
                            sheet.Protection.AllowFormatCells = false;
                            sheet.Protection.AllowFormatColumns = false;
                            sheet.Protection.AllowFormatRows = false;
                            sheet.Protection.AllowInsertColumns = false;
                            sheet.Protection.AllowInsertRows = false;
                            sheet.Protection.AllowInsertHyperlinks = false;
                        }

                        package.SaveAs(excelFile);

                        string successMessage = LanguageService.CurrentLanguage == "Türkçe" ?
                            "Ürünler tablo olarak başarıyla Excel'e aktarıldı!" :
                            "Products successfully exported as a table to Excel!";

                        MessageBox.Show(successMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = LanguageService.CurrentLanguage == "Türkçe" ?
                    $"Hata: {ex.Message}" : $"Error: {ex.Message}";
                MessageBox.Show(errorMessage);
            }
        }

        private void dgwProducts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgwProducts.ClearSelection(); //Form açıldığında DGW hücre seçili açılmaz.
        }

        private void SetPersistentToolTip(PictureBox pictureBox, int delay = 500)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            string text = currentLanguage == "Türkçe" ? "Excel dosyası düzenlemeye kapalı olacaktır.\n\nEğer düzenlemek istiyorsanız şu adımları Excel dosyası üzerinden takip ediniz:\n1- Gözden Geçir\n2- Sayfa Korumasını Kaldır\n3- Şifre, StockMaster programındaki üyeliğinizin şifresidir." : "The Excel file will be locked for editing.\n\nIf you want to enable editing, please follow these steps in the Excel file::\n1- Go to the Review tab\n2- Go to the Review tab\n3- The password is the same as your StockMaster account password.";

            ttExcel.InitialDelay = delay;
            ttExcel.ReshowDelay = delay;
            ttExcel.AutoPopDelay = int.MaxValue;
            ttExcel.ShowAlways = true;

            ttExcel.SetToolTip(pictureBox, text);
        }

        private void cmbOrdery_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySorting();
        }

        private void ApplySorting()
        {
            if (dgwProducts.DataSource is DataTable productTable)
            {
                string selectedOption = cmbOrdery.SelectedItem.ToString();
                string sortColumn = "";
                string sortDirection = "ASC"; // default: ascending

                bool isEnglish = LanguageService.CurrentLanguage != "Türkçe";

                // Kolon isimlerini dile göre ayarla
                string nameColumn = isEnglish ? "Name" : "İsim";
                string priceColumn = isEnglish ? "Price" : "Fiyat";
                string dateColumn = isEnglish ? "Added Date" : "Eklenme Tarihi";
                string stockColumn = isEnglish ? "Stock Quantity" : "Stok Adeti";

                // Sıralama seçeneğine göre kolon ve yön belirle
                switch (selectedOption)
                {
                    case "A'dan Z'ye":
                    case "A to Z":
                        sortColumn = nameColumn;
                        break;

                    case "Z'den A'ya":
                    case "Z to A":
                        sortColumn = nameColumn;
                        sortDirection = "DESC";
                        break;

                    case "Ucuzdan Pahalıya":
                    case "Cheapest to Expensive":
                        sortColumn = priceColumn;
                        break;

                    case "Pahalıdan Ucuza":
                    case "Expensive to Cheapest":
                        sortColumn = priceColumn;
                        sortDirection = "DESC";
                        break;

                    case "Eskiden Yeniye":
                    case "Oldest to Newest":
                        sortColumn = dateColumn;
                        break;

                    case "Yeniden Eskiye":
                    case "Newest to Oldest":
                        sortColumn = dateColumn;
                        sortDirection = "DESC";
                        break;

                    case "Azdan Çoğa":
                    case "Least to Most":
                        sortColumn = stockColumn;
                        break;

                    case "Çoktan Aza":
                    case "Most to Least":
                        sortColumn = stockColumn;
                        sortDirection = "DESC";
                        break;
                }

                if (!string.IsNullOrEmpty(sortColumn) && productTable.Columns.Contains(sortColumn))
                {
                    productTable.DefaultView.Sort = $"{sortColumn} {sortDirection}";
                    dgwProducts.DataSource = productTable.DefaultView.ToTable();
                }
            }
        }   

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            EditProductScreen editProductScreen = new EditProductScreen(currentLanguage);

            // MainMenu'nun IsAlwaysOnTop özelliğini kontrol et
            if (Owner is MainMenu mainMenu)
            {
                editProductScreen.TopMost = mainMenu.IsAlwaysOnTop;
            }

            editProductScreen.ProductUpdated += (s, ev) =>
            {
                if (InvokeRequired)
                {
                    Invoke(new System.Action(() =>
                    {
                        LoadUserProducts();
                    }));
                }
                else
                {
                    LoadUserProducts();
                }
            };

            editProductScreen.ShowDialog();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            Form maxForm = new Form();

            // Dil kontrolü (TR için Türkçe başlık, diğer durumlarda İngilizce)
            string currentLanguage = LanguageService.CurrentLanguage;
            maxForm.Text = currentLanguage == "Türkçe" ? "Genişletilmiş Ürün Listesi" : "Maximized Product List";

            maxForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            maxForm.StartPosition = FormStartPosition.CenterScreen;
            maxForm.MaximizeBox = false;
            maxForm.MinimizeBox = false;

            if (Owner is MainMenu mainMenu)
            {
                maxForm.TopMost = mainMenu.IsAlwaysOnTop;
            }

            DataGridView dgvMax = new DataGridView();
            dgvMax.ReadOnly = true;
            dgvMax.AllowUserToAddRows = false;
            dgvMax.AllowUserToDeleteRows = false;
            dgvMax.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMax.RowHeadersVisible = false;
            dgvMax.ScrollBars = ScrollBars.None; // Scrollbar kapalı, çünkü tüm içerik görünecek
            dgvMax.AllowUserToResizeColumns = false;
            dgvMax.AllowUserToResizeRows = false;
            dgvMax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvMax.EnableHeadersVisualStyles = false;
            dgvMax.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dgvMax.ColumnHeadersDefaultCellStyle.ForeColor = Color.Red;

            foreach (DataGridViewColumn col in dgwProducts.Columns)
            {
                var newCol = (DataGridViewColumn)col.Clone();
                newCol.SortMode = DataGridViewColumnSortMode.Automatic;
                dgvMax.Columns.Add(newCol);
            }

            foreach (DataGridViewRow row in dgwProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    int idx = dgvMax.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dgvMax.Rows[idx].Cells[i].Value = row.Cells[i].Value;
                    }
                }
            }

            for (int i = 0; i < dgwProducts.Columns.Count; i++)
            {
                dgvMax.Columns[i].Width = dgwProducts.Columns[i].Width;
            }

            // Genişlik hesaplama
            int totalWidth = dgvMax.RowHeadersVisible ? dgvMax.RowHeadersWidth : 0;
            foreach (DataGridViewColumn col in dgvMax.Columns)
                totalWidth += col.Width;

            // Yükseklik hesaplama
            int totalHeight = dgvMax.ColumnHeadersHeight;
            foreach (DataGridViewRow row in dgvMax.Rows)
                totalHeight += row.Height;

            // Form gösterildiğinde seçim temizle
            maxForm.Shown += (s, args) =>
            {
                dgvMax.ClearSelection();
            };

            dgvMax.Location = new Point(0, 0);
            dgvMax.Size = new Size(totalWidth + 2, totalHeight + 2); // 2px tolerans

            maxForm.Controls.Add(dgvMax);
            maxForm.ClientSize = new Size(dgvMax.Width, dgvMax.Height); // Pencere tam olarak DGV kadar

            maxForm.ShowDialog();
        }
        private void btnCompare_Click(object sender, EventArgs e)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            CompareScreen compareScreen = new CompareScreen(currentLanguage);

            // MainMenu'nun IsAlwaysOnTop özelliğini kontrol et
            if (Owner is MainMenu mainMenu)
            {
                compareScreen.TopMost = mainMenu.IsAlwaysOnTop;
            }

            compareScreen.ShowDialog();
        }
    }
}
