namespace neoStockMasterv2.Forms
{
    partial class ZReportScreen
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
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbBusiness = new GroupBox();
            dgwTax = new DataGridView();
            dgwNetProfit = new DataGridView();
            dgwGrossProfit = new DataGridView();
            lblNetProfit = new Label();
            lblGrossProfit = new Label();
            nmrTotalOrder = new NumericUpDown();
            lblTotalOrders = new Label();
            grbProductDetails = new GroupBox();
            label6 = new Label();
            btnPriceAnalysis = new Button();
            btnOrderHistory = new Button();
            lwProductDetails = new ListView();
            lblSalesShare = new Label();
            nmrSalesShare = new NumericUpDown();
            nmrOrderQuantity = new NumericUpDown();
            nmrStockQuantity = new NumericUpDown();
            lblOrderQuantity = new Label();
            lblStockQuantity = new Label();
            cmbProducts = new ComboBox();
            mcDate = new MonthCalendar();
            grbOrderStatus = new GroupBox();
            dgwOrders = new DataGridView();
            rbOrderFive = new RadioButton();
            rbOrderFour = new RadioButton();
            rbOrderThree = new RadioButton();
            rbOrderTwo = new RadioButton();
            rbOrderOne = new RadioButton();
            nmrOrderFive = new NumericUpDown();
            nmrOrderFour = new NumericUpDown();
            nmrOrderThree = new NumericUpDown();
            nmrOrderTwo = new NumericUpDown();
            nmrOrderOne = new NumericUpDown();
            lblOrderFive = new Label();
            lblOrderFourth = new Label();
            lblOrderThree = new Label();
            lblOrderTwo = new Label();
            lblOrderOne = new Label();
            grbProductStats = new GroupBox();
            fpMoney = new ScottPlot.WinForms.FormsPlot();
            fpDateMoney = new ScottPlot.WinForms.FormsPlot();
            ttGross = new ToolTip(components);
            ttProfit = new ToolTip(components);
            ttTax = new ToolTip(components);
            menuStripLanguage.SuspendLayout();
            grbBusiness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwTax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwNetProfit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwGrossProfit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalOrder).BeginInit();
            grbProductDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrSalesShare).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrStockQuantity).BeginInit();
            grbOrderStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderFive).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderFour).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderThree).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderTwo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderOne).BeginInit();
            grbProductStats.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(1321, 24);
            menuStripLanguage.TabIndex = 1;
            menuStripLanguage.Text = "menuStrip1";
            // 
            // türkçeToolStripMenuItem
            // 
            türkçeToolStripMenuItem.Image = Languages.Turkish.TurkFlag;
            türkçeToolStripMenuItem.Name = "türkçeToolStripMenuItem";
            türkçeToolStripMenuItem.Size = new Size(71, 20);
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
            // grbBusiness
            // 
            grbBusiness.Controls.Add(dgwTax);
            grbBusiness.Controls.Add(dgwNetProfit);
            grbBusiness.Controls.Add(dgwGrossProfit);
            grbBusiness.Controls.Add(lblNetProfit);
            grbBusiness.Controls.Add(lblGrossProfit);
            grbBusiness.Controls.Add(nmrTotalOrder);
            grbBusiness.Controls.Add(lblTotalOrders);
            grbBusiness.Location = new Point(12, 36);
            grbBusiness.Name = "grbBusiness";
            grbBusiness.Size = new Size(357, 274);
            grbBusiness.TabIndex = 2;
            grbBusiness.TabStop = false;
            grbBusiness.Text = "İşletme Verileri";
            // 
            // dgwTax
            // 
            dgwTax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwTax.Location = new Point(7, 195);
            dgwTax.Name = "dgwTax";
            dgwTax.Size = new Size(345, 73);
            dgwTax.TabIndex = 6;
            // 
            // dgwNetProfit
            // 
            dgwNetProfit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwNetProfit.Location = new Point(183, 66);
            dgwNetProfit.Name = "dgwNetProfit";
            dgwNetProfit.Size = new Size(169, 123);
            dgwNetProfit.TabIndex = 5;
            // 
            // dgwGrossProfit
            // 
            dgwGrossProfit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwGrossProfit.Location = new Point(7, 66);
            dgwGrossProfit.Name = "dgwGrossProfit";
            dgwGrossProfit.Size = new Size(169, 123);
            dgwGrossProfit.TabIndex = 4;
            // 
            // lblNetProfit
            // 
            lblNetProfit.AutoSize = true;
            lblNetProfit.Location = new Point(232, 48);
            lblNetProfit.Name = "lblNetProfit";
            lblNetProfit.Size = new Size(66, 15);
            lblNetProfit.TabIndex = 3;
            lblNetProfit.Text = "Net Kazanç";
            // 
            // lblGrossProfit
            // 
            lblGrossProfit.AutoSize = true;
            lblGrossProfit.Location = new Point(66, 48);
            lblGrossProfit.Name = "lblGrossProfit";
            lblGrossProfit.Size = new Size(69, 15);
            lblGrossProfit.TabIndex = 2;
            lblGrossProfit.Text = "Brüt Kazanç";
            // 
            // nmrTotalOrder
            // 
            nmrTotalOrder.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrTotalOrder.Location = new Point(178, 22);
            nmrTotalOrder.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrTotalOrder.Name = "nmrTotalOrder";
            nmrTotalOrder.Size = new Size(120, 23);
            nmrTotalOrder.TabIndex = 1;
            // 
            // lblTotalOrders
            // 
            lblTotalOrders.AutoSize = true;
            lblTotalOrders.Location = new Point(41, 25);
            lblTotalOrders.Name = "lblTotalOrders";
            lblTotalOrders.Size = new Size(115, 15);
            lblTotalOrders.TabIndex = 0;
            lblTotalOrders.Text = "Toplam Sipariş Adeti";
            // 
            // grbProductDetails
            // 
            grbProductDetails.Controls.Add(label6);
            grbProductDetails.Controls.Add(btnPriceAnalysis);
            grbProductDetails.Controls.Add(btnOrderHistory);
            grbProductDetails.Controls.Add(lwProductDetails);
            grbProductDetails.Controls.Add(lblSalesShare);
            grbProductDetails.Controls.Add(nmrSalesShare);
            grbProductDetails.Controls.Add(nmrOrderQuantity);
            grbProductDetails.Controls.Add(nmrStockQuantity);
            grbProductDetails.Controls.Add(lblOrderQuantity);
            grbProductDetails.Controls.Add(lblStockQuantity);
            grbProductDetails.Controls.Add(cmbProducts);
            grbProductDetails.Location = new Point(375, 36);
            grbProductDetails.Name = "grbProductDetails";
            grbProductDetails.Size = new Size(222, 274);
            grbProductDetails.TabIndex = 0;
            grbProductDetails.TabStop = false;
            grbProductDetails.Text = "Ürün Detayları";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(68, 121);
            label6.Name = "label6";
            label6.Size = new Size(17, 15);
            label6.TabIndex = 17;
            label6.Text = "%";
            // 
            // btnPriceAnalysis
            // 
            btnPriceAnalysis.Location = new Point(114, 152);
            btnPriceAnalysis.Name = "btnPriceAnalysis";
            btnPriceAnalysis.Size = new Size(102, 23);
            btnPriceAnalysis.TabIndex = 16;
            btnPriceAnalysis.Text = "Fiyat Analizi";
            btnPriceAnalysis.UseVisualStyleBackColor = true;
            btnPriceAnalysis.Click += btnPriceAnalysis_Click;
            // 
            // btnOrderHistory
            // 
            btnOrderHistory.Location = new Point(6, 152);
            btnOrderHistory.Name = "btnOrderHistory";
            btnOrderHistory.Size = new Size(102, 23);
            btnOrderHistory.TabIndex = 15;
            btnOrderHistory.Text = "Sipariş Geçmişi";
            btnOrderHistory.UseVisualStyleBackColor = true;
            btnOrderHistory.Click += btnOrderHistory_Click;
            // 
            // lwProductDetails
            // 
            lwProductDetails.Location = new Point(6, 181);
            lwProductDetails.Name = "lwProductDetails";
            lwProductDetails.Size = new Size(210, 87);
            lwProductDetails.TabIndex = 6;
            lwProductDetails.UseCompatibleStateImageBehavior = false;
            // 
            // lblSalesShare
            // 
            lblSalesShare.AutoSize = true;
            lblSalesShare.Location = new Point(6, 121);
            lblSalesShare.Name = "lblSalesShare";
            lblSalesShare.Size = new Size(56, 15);
            lblSalesShare.TabIndex = 7;
            lblSalesShare.Text = "Satış Payı";
            // 
            // nmrSalesShare
            // 
            nmrSalesShare.DecimalPlaces = 2;
            nmrSalesShare.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrSalesShare.Location = new Point(96, 119);
            nmrSalesShare.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrSalesShare.Name = "nmrSalesShare";
            nmrSalesShare.Size = new Size(120, 23);
            nmrSalesShare.TabIndex = 14;
            // 
            // nmrOrderQuantity
            // 
            nmrOrderQuantity.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderQuantity.Location = new Point(96, 91);
            nmrOrderQuantity.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderQuantity.Name = "nmrOrderQuantity";
            nmrOrderQuantity.Size = new Size(120, 23);
            nmrOrderQuantity.TabIndex = 12;
            // 
            // nmrStockQuantity
            // 
            nmrStockQuantity.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrStockQuantity.Location = new Point(96, 62);
            nmrStockQuantity.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrStockQuantity.Name = "nmrStockQuantity";
            nmrStockQuantity.Size = new Size(120, 23);
            nmrStockQuantity.TabIndex = 11;
            // 
            // lblOrderQuantity
            // 
            lblOrderQuantity.AutoSize = true;
            lblOrderQuantity.Location = new Point(6, 93);
            lblOrderQuantity.Name = "lblOrderQuantity";
            lblOrderQuantity.Size = new Size(72, 15);
            lblOrderQuantity.TabIndex = 5;
            lblOrderQuantity.Text = "Sipariş Adeti";
            // 
            // lblStockQuantity
            // 
            lblStockQuantity.AutoSize = true;
            lblStockQuantity.Location = new Point(6, 64);
            lblStockQuantity.Name = "lblStockQuantity";
            lblStockQuantity.Size = new Size(61, 15);
            lblStockQuantity.TabIndex = 4;
            lblStockQuantity.Text = "Stok Adeti";
            // 
            // cmbProducts
            // 
            cmbProducts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProducts.FormattingEnabled = true;
            cmbProducts.Location = new Point(6, 22);
            cmbProducts.Name = "cmbProducts";
            cmbProducts.Size = new Size(210, 23);
            cmbProducts.TabIndex = 3;
            cmbProducts.SelectedIndexChanged += cmbProducts_SelectedIndexChanged;
            // 
            // mcDate
            // 
            mcDate.Location = new Point(971, 102);
            mcDate.Name = "mcDate";
            mcDate.TabIndex = 3;
            // 
            // grbOrderStatus
            // 
            grbOrderStatus.Controls.Add(dgwOrders);
            grbOrderStatus.Controls.Add(rbOrderFive);
            grbOrderStatus.Controls.Add(rbOrderFour);
            grbOrderStatus.Controls.Add(rbOrderThree);
            grbOrderStatus.Controls.Add(rbOrderTwo);
            grbOrderStatus.Controls.Add(rbOrderOne);
            grbOrderStatus.Controls.Add(nmrOrderFive);
            grbOrderStatus.Controls.Add(nmrOrderFour);
            grbOrderStatus.Controls.Add(nmrOrderThree);
            grbOrderStatus.Controls.Add(nmrOrderTwo);
            grbOrderStatus.Controls.Add(nmrOrderOne);
            grbOrderStatus.Controls.Add(lblOrderFive);
            grbOrderStatus.Controls.Add(lblOrderFourth);
            grbOrderStatus.Controls.Add(lblOrderThree);
            grbOrderStatus.Controls.Add(lblOrderTwo);
            grbOrderStatus.Controls.Add(lblOrderOne);
            grbOrderStatus.Location = new Point(12, 316);
            grbOrderStatus.Name = "grbOrderStatus";
            grbOrderStatus.Size = new Size(585, 175);
            grbOrderStatus.TabIndex = 4;
            grbOrderStatus.TabStop = false;
            grbOrderStatus.Text = "Siparişlerin Durumu";
            // 
            // dgwOrders
            // 
            dgwOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwOrders.Location = new Point(278, 16);
            dgwOrders.Name = "dgwOrders";
            dgwOrders.Size = new Size(301, 150);
            dgwOrders.TabIndex = 15;
            // 
            // rbOrderFive
            // 
            rbOrderFive.AutoSize = true;
            rbOrderFive.Location = new Point(6, 146);
            rbOrderFive.Name = "rbOrderFive";
            rbOrderFive.Size = new Size(14, 13);
            rbOrderFive.TabIndex = 14;
            rbOrderFive.TabStop = true;
            rbOrderFive.UseVisualStyleBackColor = true;
            // 
            // rbOrderFour
            // 
            rbOrderFour.AutoSize = true;
            rbOrderFour.Location = new Point(6, 117);
            rbOrderFour.Name = "rbOrderFour";
            rbOrderFour.Size = new Size(14, 13);
            rbOrderFour.TabIndex = 13;
            rbOrderFour.TabStop = true;
            rbOrderFour.UseVisualStyleBackColor = true;
            // 
            // rbOrderThree
            // 
            rbOrderThree.AutoSize = true;
            rbOrderThree.Location = new Point(6, 87);
            rbOrderThree.Name = "rbOrderThree";
            rbOrderThree.Size = new Size(14, 13);
            rbOrderThree.TabIndex = 12;
            rbOrderThree.TabStop = true;
            rbOrderThree.UseVisualStyleBackColor = true;
            // 
            // rbOrderTwo
            // 
            rbOrderTwo.AutoSize = true;
            rbOrderTwo.Location = new Point(6, 58);
            rbOrderTwo.Name = "rbOrderTwo";
            rbOrderTwo.Size = new Size(14, 13);
            rbOrderTwo.TabIndex = 11;
            rbOrderTwo.TabStop = true;
            rbOrderTwo.UseVisualStyleBackColor = true;
            // 
            // rbOrderOne
            // 
            rbOrderOne.AutoSize = true;
            rbOrderOne.Location = new Point(6, 30);
            rbOrderOne.Name = "rbOrderOne";
            rbOrderOne.Size = new Size(14, 13);
            rbOrderOne.TabIndex = 10;
            rbOrderOne.TabStop = true;
            rbOrderOne.UseVisualStyleBackColor = true;
            // 
            // nmrOrderFive
            // 
            nmrOrderFive.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderFive.Location = new Point(140, 143);
            nmrOrderFive.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderFive.Name = "nmrOrderFive";
            nmrOrderFive.Size = new Size(120, 23);
            nmrOrderFive.TabIndex = 9;
            // 
            // nmrOrderFour
            // 
            nmrOrderFour.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderFour.Location = new Point(140, 114);
            nmrOrderFour.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderFour.Name = "nmrOrderFour";
            nmrOrderFour.Size = new Size(120, 23);
            nmrOrderFour.TabIndex = 8;
            // 
            // nmrOrderThree
            // 
            nmrOrderThree.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderThree.Location = new Point(140, 85);
            nmrOrderThree.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderThree.Name = "nmrOrderThree";
            nmrOrderThree.Size = new Size(120, 23);
            nmrOrderThree.TabIndex = 7;
            // 
            // nmrOrderTwo
            // 
            nmrOrderTwo.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderTwo.Location = new Point(140, 56);
            nmrOrderTwo.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderTwo.Name = "nmrOrderTwo";
            nmrOrderTwo.Size = new Size(120, 23);
            nmrOrderTwo.TabIndex = 6;
            // 
            // nmrOrderOne
            // 
            nmrOrderOne.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            nmrOrderOne.Location = new Point(140, 27);
            nmrOrderOne.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            nmrOrderOne.Name = "nmrOrderOne";
            nmrOrderOne.Size = new Size(120, 23);
            nmrOrderOne.TabIndex = 5;
            // 
            // lblOrderFive
            // 
            lblOrderFive.AutoSize = true;
            lblOrderFive.Location = new Point(23, 146);
            lblOrderFive.Name = "lblOrderFive";
            lblOrderFive.Size = new Size(28, 15);
            lblOrderFive.TabIndex = 4;
            lblOrderFive.Text = "Bitti";
            // 
            // lblOrderFourth
            // 
            lblOrderFourth.AutoSize = true;
            lblOrderFourth.Location = new Point(23, 116);
            lblOrderFourth.Name = "lblOrderFourth";
            lblOrderFourth.Size = new Size(64, 15);
            lblOrderFourth.TabIndex = 3;
            lblOrderFourth.Text = "Kargolandı";
            // 
            // lblOrderThree
            // 
            lblOrderThree.AutoSize = true;
            lblOrderThree.Location = new Point(23, 87);
            lblOrderThree.Name = "lblOrderThree";
            lblOrderThree.Size = new Size(99, 15);
            lblOrderThree.TabIndex = 2;
            lblOrderThree.Text = "Kargoya Verilecek";
            // 
            // lblOrderTwo
            // 
            lblOrderTwo.AutoSize = true;
            lblOrderTwo.Location = new Point(23, 58);
            lblOrderTwo.Name = "lblOrderTwo";
            lblOrderTwo.Size = new Size(70, 15);
            lblOrderTwo.TabIndex = 1;
            lblOrderTwo.Text = "Hazırlanıyor";
            // 
            // lblOrderOne
            // 
            lblOrderOne.AutoSize = true;
            lblOrderOne.Location = new Point(23, 29);
            lblOrderOne.Name = "lblOrderOne";
            lblOrderOne.Size = new Size(111, 15);
            lblOrderOne.TabIndex = 0;
            lblOrderOne.Text = "Siparişe Başlanmadı";
            // 
            // grbProductStats
            // 
            grbProductStats.Controls.Add(fpMoney);
            grbProductStats.Controls.Add(fpDateMoney);
            grbProductStats.Location = new Point(603, 36);
            grbProductStats.Name = "grbProductStats";
            grbProductStats.Size = new Size(340, 455);
            grbProductStats.TabIndex = 5;
            grbProductStats.TabStop = false;
            grbProductStats.Text = "Ürün Satış İstatistikleri";
            // 
            // fpMoney
            // 
            fpMoney.Location = new Point(6, 22);
            fpMoney.Name = "fpMoney";
            fpMoney.Size = new Size(328, 203);
            fpMoney.TabIndex = 6;
            // 
            // fpDateMoney
            // 
            fpDateMoney.Location = new Point(6, 231);
            fpDateMoney.Name = "fpDateMoney";
            fpDateMoney.Size = new Size(328, 218);
            fpDateMoney.TabIndex = 2;
            // 
            // ZReportScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1321, 495);
            Controls.Add(grbProductStats);
            Controls.Add(grbOrderStatus);
            Controls.Add(mcDate);
            Controls.Add(grbProductDetails);
            Controls.Add(grbBusiness);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "ZReportScreen";
            Text = " ";
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbBusiness.ResumeLayout(false);
            grbBusiness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwTax).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwNetProfit).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwGrossProfit).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalOrder).EndInit();
            grbProductDetails.ResumeLayout(false);
            grbProductDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrSalesShare).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrStockQuantity).EndInit();
            grbOrderStatus.ResumeLayout(false);
            grbOrderStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderFive).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderFour).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderThree).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderTwo).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrOrderOne).EndInit();
            grbProductStats.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbBusiness;
        private GroupBox grbProductDetails;
        private ComboBox cmbProducts;
        private Label lblTotalOrders;
        private NumericUpDown nmrTotalOrder;
        private Label lblNetProfit;
        private Label lblGrossProfit;
        private DataGridView dgwNetProfit;
        private DataGridView dgwGrossProfit;
        private Label lblSalesShare;
        private Label lblOrderQuantity;
        private Label lblStockQuantity;
        private NumericUpDown nmrSalesShare;
        private NumericUpDown nmrOrderQuantity;
        private NumericUpDown nmrStockQuantity;
        private MonthCalendar mcDate;
        private GroupBox grbOrderStatus;
        private Label lblOrderFive;
        private Label lblOrderFourth;
        private Label lblOrderThree;
        private Label lblOrderTwo;
        private Label lblOrderOne;
        private NumericUpDown nmrOrderFive;
        private NumericUpDown nmrOrderFour;
        private NumericUpDown nmrOrderThree;
        private NumericUpDown nmrOrderTwo;
        private NumericUpDown nmrOrderOne;
        private RadioButton rbOrderFive;
        private RadioButton rbOrderFour;
        private RadioButton rbOrderThree;
        private RadioButton rbOrderTwo;
        private RadioButton rbOrderOne;
        private DataGridView dgwOrders;
        private GroupBox grbProductStats;
        private DataGridView dgwTax;
        private ListView lwProductDetails;
        private Button btnPriceAnalysis;
        private Button btnOrderHistory;
        private Label label6;
        private ScottPlot.WinForms.FormsPlot fpDateMoney;
        private ScottPlot.WinForms.FormsPlot fpMoney;
        private ToolTip ttGross;
        private ToolTip ttProfit;
        private ToolTip ttTax;
    }
}