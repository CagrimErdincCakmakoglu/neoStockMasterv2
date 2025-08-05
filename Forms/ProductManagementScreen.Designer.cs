namespace neoStockMasterv2.Forms
{
    partial class ProductManagementScreen
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductManagementScreen));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbActivity = new GroupBox();
            pbInfo = new PictureBox();
            btnToExcel = new Button();
            btnEditProduct = new Button();
            btnAddProduct = new Button();
            lblActivity = new Label();
            grbOrdery = new GroupBox();
            cmbOrdery = new ComboBox();
            grbProducts = new GroupBox();
            btnCompare = new Button();
            btnMaximize = new Button();
            dgwProducts = new DataGridView();
            ttExcel = new ToolTip(components);
            menuStripLanguage.SuspendLayout();
            grbActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbInfo).BeginInit();
            grbOrdery.SuspendLayout();
            grbProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).BeginInit();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(302, 24);
            menuStripLanguage.TabIndex = 0;
            menuStripLanguage.Text = "menuStripLanguage";
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
            // grbActivity
            // 
            grbActivity.Controls.Add(pbInfo);
            grbActivity.Controls.Add(btnToExcel);
            grbActivity.Controls.Add(btnEditProduct);
            grbActivity.Controls.Add(btnAddProduct);
            grbActivity.Controls.Add(lblActivity);
            grbActivity.Location = new Point(12, 36);
            grbActivity.Name = "grbActivity";
            grbActivity.Size = new Size(285, 100);
            grbActivity.TabIndex = 1;
            grbActivity.TabStop = false;
            // 
            // pbInfo
            // 
            pbInfo.Image = (Image)resources.GetObject("pbInfo.Image");
            pbInfo.Location = new Point(6, 62);
            pbInfo.Name = "pbInfo";
            pbInfo.Size = new Size(25, 23);
            pbInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbInfo.TabIndex = 5;
            pbInfo.TabStop = false;
            // 
            // btnToExcel
            // 
            btnToExcel.Location = new Point(37, 62);
            btnToExcel.Name = "btnToExcel";
            btnToExcel.Size = new Size(242, 23);
            btnToExcel.TabIndex = 4;
            btnToExcel.Text = "Excel'e Aktar";
            btnToExcel.UseVisualStyleBackColor = true;
            btnToExcel.Click += btnToExcel_Click;
            // 
            // btnEditProduct
            // 
            btnEditProduct.Location = new Point(142, 22);
            btnEditProduct.Name = "btnEditProduct";
            btnEditProduct.Size = new Size(137, 34);
            btnEditProduct.TabIndex = 3;
            btnEditProduct.Text = "Ürün Düzenle / Sil";
            btnEditProduct.UseVisualStyleBackColor = true;
            btnEditProduct.Click += btnEditProduct_Click;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Location = new Point(6, 22);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(130, 34);
            btnAddProduct.TabIndex = 2;
            btnAddProduct.Text = "Ürün Ekle";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // lblActivity
            // 
            lblActivity.AutoSize = true;
            lblActivity.Location = new Point(113, 0);
            lblActivity.Name = "lblActivity";
            lblActivity.Size = new Size(59, 15);
            lblActivity.TabIndex = 0;
            lblActivity.Text = "İşlemi Seç";
            // 
            // grbOrdery
            // 
            grbOrdery.Controls.Add(cmbOrdery);
            grbOrdery.Location = new Point(12, 142);
            grbOrdery.Name = "grbOrdery";
            grbOrdery.Size = new Size(285, 56);
            grbOrdery.TabIndex = 2;
            grbOrdery.TabStop = false;
            grbOrdery.Text = "Sıralama";
            // 
            // cmbOrdery
            // 
            cmbOrdery.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrdery.FormattingEnabled = true;
            cmbOrdery.Location = new Point(6, 22);
            cmbOrdery.Name = "cmbOrdery";
            cmbOrdery.Size = new Size(273, 23);
            cmbOrdery.TabIndex = 0;
            cmbOrdery.SelectedIndexChanged += cmbOrdery_SelectedIndexChanged;
            // 
            // grbProducts
            // 
            grbProducts.Controls.Add(btnCompare);
            grbProducts.Controls.Add(btnMaximize);
            grbProducts.Controls.Add(dgwProducts);
            grbProducts.Location = new Point(12, 204);
            grbProducts.Name = "grbProducts";
            grbProducts.Size = new Size(285, 294);
            grbProducts.TabIndex = 3;
            grbProducts.TabStop = false;
            grbProducts.Text = "Ürünler";
            // 
            // btnCompare
            // 
            btnCompare.Location = new Point(143, 22);
            btnCompare.Name = "btnCompare";
            btnCompare.Size = new Size(136, 23);
            btnCompare.TabIndex = 2;
            btnCompare.Text = "Karşılaştır";
            btnCompare.UseVisualStyleBackColor = true;
            btnCompare.Click += btnCompare_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.Location = new Point(6, 22);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(130, 23);
            btnMaximize.TabIndex = 1;
            btnMaximize.Text = "Genişlet";
            btnMaximize.UseVisualStyleBackColor = true;
            btnMaximize.Click += btnMaximize_Click;
            // 
            // dgwProducts
            // 
            dgwProducts.AllowUserToAddRows = false;
            dgwProducts.AllowUserToDeleteRows = false;
            dgwProducts.AllowUserToResizeColumns = false;
            dgwProducts.AllowUserToResizeRows = false;
            dgwProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgwProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgwProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwProducts.Location = new Point(3, 51);
            dgwProducts.MultiSelect = false;
            dgwProducts.Name = "dgwProducts";
            dgwProducts.ReadOnly = true;
            dgwProducts.RowHeadersVisible = false;
            dgwProducts.Size = new Size(279, 240);
            dgwProducts.TabIndex = 0;
            dgwProducts.CellClick += dgwProducts_CellClick;
            dgwProducts.DataBindingComplete += dgwProducts_DataBindingComplete;
            // 
            // ProductManagementScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 510);
            Controls.Add(grbProducts);
            Controls.Add(grbOrdery);
            Controls.Add(grbActivity);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "ProductManagementScreen";
            Text = "Ürün İşlemleri";
            Load += ProductManagementScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbActivity.ResumeLayout(false);
            grbActivity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbInfo).EndInit();
            grbOrdery.ResumeLayout(false);
            grbProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbActivity;
        private Label lblActivity;
        private Button btnToExcel;
        private Button btnEditProduct;
        private Button btnAddProduct;
        private GroupBox grbOrdery;
        private ComboBox cmbOrdery;
        private GroupBox grbProducts;
        private DataGridView dgwProducts;
        private ToolTip ttExcel;
        private PictureBox pbInfo;
        private Button btnCompare;
        private Button btnMaximize;
    }
}