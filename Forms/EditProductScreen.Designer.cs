namespace neoStockMasterv2.Forms
{
    partial class EditProductScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbProductList = new GroupBox();
            cmbProducts = new ComboBox();
            lblStock = new Label();
            lblPrice = new Label();
            lblCost = new Label();
            lblName = new Label();
            lblPercent = new Label();
            txtName = new TextBox();
            nmrCost = new NumericUpDown();
            nmrPrice = new NumericUpDown();
            nmrStock = new NumericUpDown();
            nmrPercent = new NumericUpDown();
            grbMethod = new GroupBox();
            rdbDelete = new RadioButton();
            rdbUpdStock = new RadioButton();
            rdbEdit = new RadioButton();
            grbEdit = new GroupBox();
            cmbPrice = new ComboBox();
            cmbCost = new ComboBox();
            btnUpdate = new Button();
            btnOrijinal = new Button();
            btnClear = new Button();
            grbDelete = new GroupBox();
            btnDel = new Button();
            nmrDelStock = new NumericUpDown();
            cmbDelPrice = new ComboBox();
            nmrDelPrice = new NumericUpDown();
            cmbDelCost = new ComboBox();
            nmrDelCost = new NumericUpDown();
            txtDelName = new TextBox();
            lblDelStock = new Label();
            lblDelPrice = new Label();
            lblDelCost = new Label();
            lblDelName = new Label();
            grbStock = new GroupBox();
            btnStAdd = new Button();
            btnStClear = new Button();
            nmrUpgAddStock = new NumericUpDown();
            lblStAdd = new Label();
            nmrUpgStock = new NumericUpDown();
            txtUpgName = new TextBox();
            lblStStok = new Label();
            lblStName = new Label();
            menuStripLanguage.SuspendLayout();
            grbProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrCost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrPercent).BeginInit();
            grbMethod.SuspendLayout();
            grbEdit.SuspendLayout();
            grbDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDelStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrDelPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrDelCost).BeginInit();
            grbStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrUpgAddStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrUpgStock).BeginInit();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(290, 24);
            menuStripLanguage.TabIndex = 0;
            // 
            // türkçeToolStripMenuItem
            // 
            türkçeToolStripMenuItem.Image = Languages.Turkish.TurkFlag;
            türkçeToolStripMenuItem.Name = "türkçeToolStripMenuItem";
            türkçeToolStripMenuItem.Size = new Size(70, 20);
            türkçeToolStripMenuItem.Text = "Türkçe";
            türkçeToolStripMenuItem.Click += türkçeToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.Image = Languages.English.EngFlag;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(73, 20);
            englishToolStripMenuItem.Text = "English";
            englishToolStripMenuItem.Click += englishToolStripMenuItem_Click;
            // 
            // grbProductList
            // 
            grbProductList.Controls.Add(cmbProducts);
            grbProductList.Location = new Point(12, 32);
            grbProductList.Name = "grbProductList";
            grbProductList.Size = new Size(268, 60);
            grbProductList.TabIndex = 1;
            grbProductList.TabStop = false;
            grbProductList.Text = "Ürünler";
            // 
            // cmbProducts
            // 
            cmbProducts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProducts.FormattingEnabled = true;
            cmbProducts.Location = new Point(6, 22);
            cmbProducts.Name = "cmbProducts";
            cmbProducts.Size = new Size(256, 23);
            cmbProducts.TabIndex = 0;
            cmbProducts.SelectedIndexChanged += cmbProducts_SelectedIndexChanged;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(6, 115);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(30, 15);
            lblStock.TabIndex = 4;
            lblStock.Text = "Stok";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(6, 86);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(32, 15);
            lblPrice.TabIndex = 3;
            lblPrice.Text = "Fiyat";
            // 
            // lblCost
            // 
            lblCost.AutoSize = true;
            lblCost.Location = new Point(6, 57);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(46, 15);
            lblCost.TabIndex = 2;
            lblCost.Text = "Maliyet";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(6, 29);
            lblName.Name = "lblName";
            lblName.Size = new Size(29, 15);
            lblName.TabIndex = 1;
            lblName.Text = "İsim";
            // 
            // lblPercent
            // 
            lblPercent.AutoSize = true;
            lblPercent.Location = new Point(6, 144);
            lblPercent.Name = "lblPercent";
            lblPercent.Size = new Size(67, 15);
            lblPercent.TabIndex = 5;
            lblPercent.Text = "Kâr Yüzdesi";
            // 
            // txtName
            // 
            txtName.Location = new Point(87, 26);
            txtName.Name = "txtName";
            txtName.Size = new Size(175, 23);
            txtName.TabIndex = 6;
            // 
            // nmrCost
            // 
            nmrCost.DecimalPlaces = 2;
            nmrCost.Location = new Point(87, 57);
            nmrCost.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrCost.Name = "nmrCost";
            nmrCost.Size = new Size(96, 23);
            nmrCost.TabIndex = 7;
            nmrCost.ValueChanged += nmrCost_ValueChanged;
            nmrCost.KeyUp += nmrCost_KeyUp;
            // 
            // nmrPrice
            // 
            nmrPrice.DecimalPlaces = 2;
            nmrPrice.Location = new Point(87, 84);
            nmrPrice.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrPrice.Name = "nmrPrice";
            nmrPrice.Size = new Size(96, 23);
            nmrPrice.TabIndex = 8;
            nmrPrice.ValueChanged += nmrPrice_ValueChanged;
            nmrPrice.KeyUp += nmrPrice_KeyUp;
            // 
            // nmrStock
            // 
            nmrStock.Location = new Point(87, 113);
            nmrStock.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrStock.Name = "nmrStock";
            nmrStock.Size = new Size(175, 23);
            nmrStock.TabIndex = 9;
            // 
            // nmrPercent
            // 
            nmrPercent.Location = new Point(87, 142);
            nmrPercent.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrPercent.Minimum = new decimal(new int[] { 1661992959, 1808227885, 5, int.MinValue });
            nmrPercent.Name = "nmrPercent";
            nmrPercent.Size = new Size(175, 23);
            nmrPercent.TabIndex = 10;
            nmrPercent.ValueChanged += nmrPercent_ValueChanged;
            nmrPercent.KeyUp += nmrPercent_KeyUp;
            // 
            // grbMethod
            // 
            grbMethod.Controls.Add(rdbDelete);
            grbMethod.Controls.Add(rdbUpdStock);
            grbMethod.Controls.Add(rdbEdit);
            grbMethod.Location = new Point(12, 98);
            grbMethod.Name = "grbMethod";
            grbMethod.Size = new Size(268, 52);
            grbMethod.TabIndex = 11;
            grbMethod.TabStop = false;
            grbMethod.Text = "Yöntem";
            // 
            // rdbDelete
            // 
            rdbDelete.AutoSize = true;
            rdbDelete.Location = new Point(201, 22);
            rdbDelete.Name = "rdbDelete";
            rdbDelete.Size = new Size(37, 19);
            rdbDelete.TabIndex = 13;
            rdbDelete.TabStop = true;
            rdbDelete.Text = "Sil";
            rdbDelete.UseVisualStyleBackColor = true;
            rdbDelete.CheckedChanged += rdbDelete_CheckedChanged;
            // 
            // rdbUpdStock
            // 
            rdbUpdStock.AutoSize = true;
            rdbUpdStock.Location = new Point(106, 22);
            rdbUpdStock.Name = "rdbUpdStock";
            rdbUpdStock.Size = new Size(72, 19);
            rdbUpdStock.TabIndex = 12;
            rdbUpdStock.TabStop = true;
            rdbUpdStock.Text = "Stok Ekle";
            rdbUpdStock.UseVisualStyleBackColor = true;
            rdbUpdStock.CheckedChanged += rdbUpdStock_CheckedChanged;
            // 
            // rdbEdit
            // 
            rdbEdit.AutoSize = true;
            rdbEdit.Location = new Point(23, 22);
            rdbEdit.Name = "rdbEdit";
            rdbEdit.Size = new Size(67, 19);
            rdbEdit.TabIndex = 12;
            rdbEdit.TabStop = true;
            rdbEdit.Text = "Düzenle";
            rdbEdit.UseVisualStyleBackColor = true;
            rdbEdit.CheckedChanged += rdbEdit_CheckedChanged;
            // 
            // grbEdit
            // 
            grbEdit.Controls.Add(cmbPrice);
            grbEdit.Controls.Add(cmbCost);
            grbEdit.Controls.Add(btnUpdate);
            grbEdit.Controls.Add(btnOrijinal);
            grbEdit.Controls.Add(btnClear);
            grbEdit.Controls.Add(lblName);
            grbEdit.Controls.Add(nmrPercent);
            grbEdit.Controls.Add(lblPrice);
            grbEdit.Controls.Add(lblStock);
            grbEdit.Controls.Add(nmrStock);
            grbEdit.Controls.Add(lblCost);
            grbEdit.Controls.Add(lblPercent);
            grbEdit.Controls.Add(nmrPrice);
            grbEdit.Controls.Add(txtName);
            grbEdit.Controls.Add(nmrCost);
            grbEdit.Location = new Point(12, 156);
            grbEdit.Name = "grbEdit";
            grbEdit.Size = new Size(268, 219);
            grbEdit.TabIndex = 12;
            grbEdit.TabStop = false;
            grbEdit.Text = "Ürün Bilgilerini Düzenle";
            // 
            // cmbPrice
            // 
            cmbPrice.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrice.FormattingEnabled = true;
            cmbPrice.Location = new Point(185, 84);
            cmbPrice.Name = "cmbPrice";
            cmbPrice.Size = new Size(77, 23);
            cmbPrice.TabIndex = 15;
            cmbPrice.SelectedIndexChanged += cmbPrice_SelectedIndexChanged;
            // 
            // cmbCost
            // 
            cmbCost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCost.FormattingEnabled = true;
            cmbCost.Location = new Point(185, 57);
            cmbCost.Name = "cmbCost";
            cmbCost.Size = new Size(77, 23);
            cmbCost.TabIndex = 14;
            cmbCost.SelectedIndexChanged += cmbCost_SelectedIndexChanged;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(178, 180);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.TabIndex = 13;
            btnUpdate.Text = "Güncelle";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnOrijinal
            // 
            btnOrijinal.Location = new Point(87, 180);
            btnOrijinal.Name = "btnOrijinal";
            btnOrijinal.Size = new Size(85, 23);
            btnOrijinal.TabIndex = 12;
            btnOrijinal.Text = "Orijinal";
            btnOrijinal.UseVisualStyleBackColor = true;
            btnOrijinal.Click += btnOrijinal_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(6, 180);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 11;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // grbDelete
            // 
            grbDelete.Controls.Add(btnDel);
            grbDelete.Controls.Add(nmrDelStock);
            grbDelete.Controls.Add(cmbDelPrice);
            grbDelete.Controls.Add(nmrDelPrice);
            grbDelete.Controls.Add(cmbDelCost);
            grbDelete.Controls.Add(nmrDelCost);
            grbDelete.Controls.Add(txtDelName);
            grbDelete.Controls.Add(lblDelStock);
            grbDelete.Controls.Add(lblDelPrice);
            grbDelete.Controls.Add(lblDelCost);
            grbDelete.Controls.Add(lblDelName);
            grbDelete.Location = new Point(12, 381);
            grbDelete.Name = "grbDelete";
            grbDelete.Size = new Size(268, 173);
            grbDelete.TabIndex = 15;
            grbDelete.TabStop = false;
            grbDelete.Text = "Ürünü Sil";
            // 
            // btnDel
            // 
            btnDel.Location = new Point(6, 137);
            btnDel.Name = "btnDel";
            btnDel.Size = new Size(256, 23);
            btnDel.TabIndex = 17;
            btnDel.Text = "Sil";
            btnDel.UseVisualStyleBackColor = true;
            btnDel.Click += btnDel_Click;
            // 
            // nmrDelStock
            // 
            nmrDelStock.Location = new Point(87, 105);
            nmrDelStock.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrDelStock.Name = "nmrDelStock";
            nmrDelStock.Size = new Size(175, 23);
            nmrDelStock.TabIndex = 16;
            // 
            // cmbDelPrice
            // 
            cmbDelPrice.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDelPrice.FormattingEnabled = true;
            cmbDelPrice.Location = new Point(185, 76);
            cmbDelPrice.Name = "cmbDelPrice";
            cmbDelPrice.Size = new Size(77, 23);
            cmbDelPrice.TabIndex = 8;
            // 
            // nmrDelPrice
            // 
            nmrDelPrice.Location = new Point(87, 77);
            nmrDelPrice.Maximum = new decimal(new int[] { -559939585, 902409669, 54, 0 });
            nmrDelPrice.Name = "nmrDelPrice";
            nmrDelPrice.Size = new Size(96, 23);
            nmrDelPrice.TabIndex = 7;
            // 
            // cmbDelCost
            // 
            cmbDelCost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDelCost.FormattingEnabled = true;
            cmbDelCost.Location = new Point(185, 48);
            cmbDelCost.Name = "cmbDelCost";
            cmbDelCost.Size = new Size(77, 23);
            cmbDelCost.TabIndex = 6;
            // 
            // nmrDelCost
            // 
            nmrDelCost.Location = new Point(87, 48);
            nmrDelCost.Maximum = new decimal(new int[] { -559939585, 902409669, 54, 0 });
            nmrDelCost.Name = "nmrDelCost";
            nmrDelCost.Size = new Size(96, 23);
            nmrDelCost.TabIndex = 5;
            // 
            // txtDelName
            // 
            txtDelName.Location = new Point(87, 19);
            txtDelName.Name = "txtDelName";
            txtDelName.Size = new Size(175, 23);
            txtDelName.TabIndex = 4;
            // 
            // lblDelStock
            // 
            lblDelStock.AutoSize = true;
            lblDelStock.Location = new Point(9, 107);
            lblDelStock.Name = "lblDelStock";
            lblDelStock.Size = new Size(30, 15);
            lblDelStock.TabIndex = 3;
            lblDelStock.Text = "Stok";
            // 
            // lblDelPrice
            // 
            lblDelPrice.AutoSize = true;
            lblDelPrice.Location = new Point(10, 79);
            lblDelPrice.Name = "lblDelPrice";
            lblDelPrice.Size = new Size(32, 15);
            lblDelPrice.TabIndex = 2;
            lblDelPrice.Text = "Fiyat";
            // 
            // lblDelCost
            // 
            lblDelCost.AutoSize = true;
            lblDelCost.Location = new Point(9, 50);
            lblDelCost.Name = "lblDelCost";
            lblDelCost.Size = new Size(46, 15);
            lblDelCost.TabIndex = 1;
            lblDelCost.Text = "Maliyet";
            // 
            // lblDelName
            // 
            lblDelName.AutoSize = true;
            lblDelName.Location = new Point(10, 22);
            lblDelName.Name = "lblDelName";
            lblDelName.Size = new Size(29, 15);
            lblDelName.TabIndex = 0;
            lblDelName.Text = "İsim";
            // 
            // grbStock
            // 
            grbStock.Controls.Add(btnStAdd);
            grbStock.Controls.Add(btnStClear);
            grbStock.Controls.Add(nmrUpgAddStock);
            grbStock.Controls.Add(lblStAdd);
            grbStock.Controls.Add(nmrUpgStock);
            grbStock.Controls.Add(txtUpgName);
            grbStock.Controls.Add(lblStStok);
            grbStock.Controls.Add(lblStName);
            grbStock.Location = new Point(12, 560);
            grbStock.Name = "grbStock";
            grbStock.Size = new Size(268, 147);
            grbStock.TabIndex = 14;
            grbStock.TabStop = false;
            grbStock.Text = "Stok Ekle";
            // 
            // btnStAdd
            // 
            btnStAdd.Location = new Point(140, 112);
            btnStAdd.Name = "btnStAdd";
            btnStAdd.Size = new Size(119, 23);
            btnStAdd.TabIndex = 18;
            btnStAdd.Text = "Ekle";
            btnStAdd.UseVisualStyleBackColor = true;
            btnStAdd.Click += btnStAdd_Click;
            // 
            // btnStClear
            // 
            btnStClear.Location = new Point(6, 112);
            btnStClear.Name = "btnStClear";
            btnStClear.Size = new Size(119, 23);
            btnStClear.TabIndex = 17;
            btnStClear.Text = "Temizle";
            btnStClear.UseVisualStyleBackColor = true;
            btnStClear.Click += btnStClear_Click;
            // 
            // nmrUpgAddStock
            // 
            nmrUpgAddStock.Location = new Point(87, 80);
            nmrUpgAddStock.Maximum = new decimal(new int[] { -559939585, 902409669, 54, 0 });
            nmrUpgAddStock.Name = "nmrUpgAddStock";
            nmrUpgAddStock.Size = new Size(166, 23);
            nmrUpgAddStock.TabIndex = 16;
            // 
            // lblStAdd
            // 
            lblStAdd.AutoSize = true;
            lblStAdd.Location = new Point(10, 82);
            lblStAdd.Name = "lblStAdd";
            lblStAdd.Size = new Size(59, 15);
            lblStAdd.TabIndex = 15;
            lblStAdd.Text = "Eklenecek";
            // 
            // nmrUpgStock
            // 
            nmrUpgStock.Location = new Point(87, 51);
            nmrUpgStock.Maximum = new decimal(new int[] { -559939585, 902409669, 54, 0 });
            nmrUpgStock.Name = "nmrUpgStock";
            nmrUpgStock.Size = new Size(166, 23);
            nmrUpgStock.TabIndex = 3;
            // 
            // txtUpgName
            // 
            txtUpgName.Location = new Point(87, 22);
            txtUpgName.Name = "txtUpgName";
            txtUpgName.Size = new Size(166, 23);
            txtUpgName.TabIndex = 2;
            // 
            // lblStStok
            // 
            lblStStok.AutoSize = true;
            lblStStok.Location = new Point(9, 53);
            lblStStok.Name = "lblStStok";
            lblStStok.Size = new Size(30, 15);
            lblStStok.TabIndex = 1;
            lblStStok.Text = "Stok";
            // 
            // lblStName
            // 
            lblStName.AutoSize = true;
            lblStName.Location = new Point(10, 25);
            lblStName.Name = "lblStName";
            lblStName.Size = new Size(29, 15);
            lblStName.TabIndex = 0;
            lblStName.Text = "İsim";
            // 
            // EditProductScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(290, 735);
            Controls.Add(grbDelete);
            Controls.Add(grbStock);
            Controls.Add(grbEdit);
            Controls.Add(grbMethod);
            Controls.Add(grbProductList);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            Name = "EditProductScreen";
            Load += EditProductScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbProductList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrCost).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrPercent).EndInit();
            grbMethod.ResumeLayout(false);
            grbMethod.PerformLayout();
            grbEdit.ResumeLayout(false);
            grbEdit.PerformLayout();
            grbDelete.ResumeLayout(false);
            grbDelete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDelStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrDelPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrDelCost).EndInit();
            grbStock.ResumeLayout(false);
            grbStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrUpgAddStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrUpgStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbProductList;
        private Label lblName;
        private ComboBox cmbProducts;
        private Label lblPrice;
        private Label lblCost;
        private Label lblStock;
        private Label lblPercent;
        private TextBox txtName;
        private NumericUpDown nmrPrice;
        private NumericUpDown nmrCost;
        private NumericUpDown nmrPercent;
        private NumericUpDown nmrStock;
        private GroupBox grbMethod;
        private RadioButton rdbUpdStock;
        private RadioButton rdbEdit;
        private GroupBox grbEdit;
        private Button btnOrijinal;
        private Button btnClear;
        private Button btnUpdate;
        private GroupBox grbStock;
        private NumericUpDown nmrUpgStock;
        private TextBox txtUpgName;
        private Label lblStStok;
        private Label lblStName;
        private Button btnStAdd;
        private Button btnStClear;
        private NumericUpDown nmrUpgAddStock;
        private Label lblStAdd;
        private ComboBox cmbPrice;
        private ComboBox cmbCost;
        private RadioButton rdbDelete;
        private GroupBox grbDelete;
        private NumericUpDown nmrDelPrice;
        private ComboBox cmbDelCost;
        private NumericUpDown nmrDelCost;
        private TextBox txtDelName;
        private Label lblDelStock;
        private Label lblDelPrice;
        private Label lblDelCost;
        private Label lblDelName;
        private ComboBox cmbDelPrice;
        private Button btnDel;
        private NumericUpDown nmrDelStock;
    }
}