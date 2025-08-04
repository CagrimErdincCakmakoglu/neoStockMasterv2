namespace neoStockMasterv2.Forms
{
    partial class AddProductScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProductScreen));
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbProduct = new GroupBox();
            nmrPercent = new NumericUpDown();
            nmrStock = new NumericUpDown();
            cmbPrice = new ComboBox();
            nmrPrice = new NumericUpDown();
            cmbCost = new ComboBox();
            nmrCost = new NumericUpDown();
            txtName = new TextBox();
            lblPercent = new Label();
            lblStock = new Label();
            lblPrice = new Label();
            lblCost = new Label();
            lblName = new Label();
            grbExchangeInfo = new GroupBox();
            btnRefresh = new Button();
            cmbSymbol5th = new ComboBox();
            cmbSymbol4th = new ComboBox();
            cmbSymbol3rd = new ComboBox();
            cmbSymbol2nd = new ComboBox();
            cmbSymbol1st = new ComboBox();
            cmbForex5th = new ComboBox();
            cmbForex4th = new ComboBox();
            cmbForex3rd = new ComboBox();
            cmbForex2nd = new ComboBox();
            cmbForex1st = new ComboBox();
            cmbBanks = new ComboBox();
            nmrForexSelling5th = new NumericUpDown();
            nmrForexSelling4th = new NumericUpDown();
            nmrForexSelling3rd = new NumericUpDown();
            nmrForexSelling2nd = new NumericUpDown();
            nmrForexSelling1st = new NumericUpDown();
            lblSelling = new Label();
            lblBuying = new Label();
            nmrForexBuying5th = new NumericUpDown();
            nmrForexBuying4th = new NumericUpDown();
            nmrForexBuying3rd = new NumericUpDown();
            nmrForexBuying2nd = new NumericUpDown();
            nmrForexBuying1st = new NumericUpDown();
            btnClear = new Button();
            btnAdd = new Button();
            grbCrossRates = new GroupBox();
            pbSwap = new PictureBox();
            grbTo = new GroupBox();
            lblToMoney = new Label();
            cmbToMoney = new ComboBox();
            nmrToMoneyUnite = new NumericUpDown();
            lblToMoneyUnite = new Label();
            grbFrom = new GroupBox();
            lblFromMoney = new Label();
            cmbFromMoney = new ComboBox();
            lblFromMoneyUnite = new Label();
            nmrFromMoneyUnite = new NumericUpDown();
            ttSwap = new ToolTip(components);
            menuStripLanguage.SuspendLayout();
            grbProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrPercent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrCost).BeginInit();
            grbExchangeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling5th).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling4th).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling3rd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling2nd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling1st).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying5th).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying4th).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying3rd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying2nd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying1st).BeginInit();
            grbCrossRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbSwap).BeginInit();
            grbTo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrToMoneyUnite).BeginInit();
            grbFrom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrFromMoneyUnite).BeginInit();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(1010, 24);
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
            // grbProduct
            // 
            grbProduct.Controls.Add(nmrPercent);
            grbProduct.Controls.Add(nmrStock);
            grbProduct.Controls.Add(cmbPrice);
            grbProduct.Controls.Add(nmrPrice);
            grbProduct.Controls.Add(cmbCost);
            grbProduct.Controls.Add(nmrCost);
            grbProduct.Controls.Add(txtName);
            grbProduct.Controls.Add(lblPercent);
            grbProduct.Controls.Add(lblStock);
            grbProduct.Controls.Add(lblPrice);
            grbProduct.Controls.Add(lblCost);
            grbProduct.Controls.Add(lblName);
            grbProduct.Location = new Point(12, 27);
            grbProduct.Name = "grbProduct";
            grbProduct.Size = new Size(355, 174);
            grbProduct.TabIndex = 1;
            grbProduct.TabStop = false;
            grbProduct.Text = "Ürün Bilgileri";
            // 
            // nmrPercent
            // 
            nmrPercent.Location = new Point(121, 138);
            nmrPercent.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrPercent.Minimum = new decimal(new int[] { 1661992959, 1808227885, 5, int.MinValue });
            nmrPercent.Name = "nmrPercent";
            nmrPercent.Size = new Size(137, 23);
            nmrPercent.TabIndex = 10;
            nmrPercent.ValueChanged += nmrPercent_ValueChanged;
            nmrPercent.KeyUp += nmrPercent_KeyUp;
            // 
            // nmrStock
            // 
            nmrStock.Location = new Point(121, 109);
            nmrStock.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrStock.Name = "nmrStock";
            nmrStock.Size = new Size(137, 23);
            nmrStock.TabIndex = 9;
            // 
            // cmbPrice
            // 
            cmbPrice.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrice.FormattingEnabled = true;
            cmbPrice.Location = new Point(264, 79);
            cmbPrice.Name = "cmbPrice";
            cmbPrice.Size = new Size(85, 23);
            cmbPrice.TabIndex = 8;
            cmbPrice.SelectedIndexChanged += cmbPrice_SelectedIndexChanged;
            // 
            // nmrPrice
            // 
            nmrPrice.DecimalPlaces = 2;
            nmrPrice.Location = new Point(121, 80);
            nmrPrice.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrPrice.Name = "nmrPrice";
            nmrPrice.Size = new Size(137, 23);
            nmrPrice.TabIndex = 7;
            nmrPrice.ValueChanged += nmrPrice_ValueChanged;
            nmrPrice.KeyUp += nmrPrice_KeyUp;
            // 
            // cmbCost
            // 
            cmbCost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCost.FormattingEnabled = true;
            cmbCost.Location = new Point(264, 50);
            cmbCost.Name = "cmbCost";
            cmbCost.Size = new Size(85, 23);
            cmbCost.TabIndex = 2;
            cmbCost.SelectedIndexChanged += cmbCost_SelectedIndexChanged;
            // 
            // nmrCost
            // 
            nmrCost.DecimalPlaces = 2;
            nmrCost.Location = new Point(121, 51);
            nmrCost.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrCost.Name = "nmrCost";
            nmrCost.Size = new Size(137, 23);
            nmrCost.TabIndex = 6;
            nmrCost.ValueChanged += nmrCost_ValueChanged;
            nmrCost.KeyUp += nmrCost_KeyUp;
            // 
            // txtName
            // 
            txtName.Location = new Point(121, 22);
            txtName.Name = "txtName";
            txtName.Size = new Size(137, 23);
            txtName.TabIndex = 5;
            // 
            // lblPercent
            // 
            lblPercent.AutoSize = true;
            lblPercent.Location = new Point(6, 140);
            lblPercent.Name = "lblPercent";
            lblPercent.Size = new Size(67, 15);
            lblPercent.TabIndex = 4;
            lblPercent.Text = "Kâr Yüzdesi";
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(6, 111);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(30, 15);
            lblStock.TabIndex = 3;
            lblStock.Text = "Stok";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(6, 82);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(32, 15);
            lblPrice.TabIndex = 2;
            lblPrice.Text = "Fiyat";
            // 
            // lblCost
            // 
            lblCost.AutoSize = true;
            lblCost.Location = new Point(6, 53);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(46, 15);
            lblCost.TabIndex = 1;
            lblCost.Text = "Maliyet";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(6, 25);
            lblName.Name = "lblName";
            lblName.Size = new Size(29, 15);
            lblName.TabIndex = 0;
            lblName.Text = "İsim";
            // 
            // grbExchangeInfo
            // 
            grbExchangeInfo.Controls.Add(btnRefresh);
            grbExchangeInfo.Controls.Add(cmbSymbol5th);
            grbExchangeInfo.Controls.Add(cmbSymbol4th);
            grbExchangeInfo.Controls.Add(cmbSymbol3rd);
            grbExchangeInfo.Controls.Add(cmbSymbol2nd);
            grbExchangeInfo.Controls.Add(cmbSymbol1st);
            grbExchangeInfo.Controls.Add(cmbForex5th);
            grbExchangeInfo.Controls.Add(cmbForex4th);
            grbExchangeInfo.Controls.Add(cmbForex3rd);
            grbExchangeInfo.Controls.Add(cmbForex2nd);
            grbExchangeInfo.Controls.Add(cmbForex1st);
            grbExchangeInfo.Controls.Add(cmbBanks);
            grbExchangeInfo.Controls.Add(nmrForexSelling5th);
            grbExchangeInfo.Controls.Add(nmrForexSelling4th);
            grbExchangeInfo.Controls.Add(nmrForexSelling3rd);
            grbExchangeInfo.Controls.Add(nmrForexSelling2nd);
            grbExchangeInfo.Controls.Add(nmrForexSelling1st);
            grbExchangeInfo.Controls.Add(lblSelling);
            grbExchangeInfo.Controls.Add(lblBuying);
            grbExchangeInfo.Controls.Add(nmrForexBuying5th);
            grbExchangeInfo.Controls.Add(nmrForexBuying4th);
            grbExchangeInfo.Controls.Add(nmrForexBuying3rd);
            grbExchangeInfo.Controls.Add(nmrForexBuying2nd);
            grbExchangeInfo.Controls.Add(nmrForexBuying1st);
            grbExchangeInfo.Location = new Point(373, 27);
            grbExchangeInfo.Name = "grbExchangeInfo";
            grbExchangeInfo.Size = new Size(407, 209);
            grbExchangeInfo.TabIndex = 0;
            grbExchangeInfo.TabStop = false;
            grbExchangeInfo.Text = "Döviz Bilgisi";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(264, 14);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(60, 23);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Yenile";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // cmbSymbol5th
            // 
            cmbSymbol5th.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSymbol5th.FormattingEnabled = true;
            cmbSymbol5th.Location = new Point(140, 170);
            cmbSymbol5th.Name = "cmbSymbol5th";
            cmbSymbol5th.Size = new Size(58, 23);
            cmbSymbol5th.TabIndex = 34;
            // 
            // cmbSymbol4th
            // 
            cmbSymbol4th.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSymbol4th.FormattingEnabled = true;
            cmbSymbol4th.Location = new Point(140, 143);
            cmbSymbol4th.Name = "cmbSymbol4th";
            cmbSymbol4th.Size = new Size(58, 23);
            cmbSymbol4th.TabIndex = 33;
            // 
            // cmbSymbol3rd
            // 
            cmbSymbol3rd.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSymbol3rd.FormattingEnabled = true;
            cmbSymbol3rd.Location = new Point(140, 115);
            cmbSymbol3rd.Name = "cmbSymbol3rd";
            cmbSymbol3rd.Size = new Size(58, 23);
            cmbSymbol3rd.TabIndex = 32;
            // 
            // cmbSymbol2nd
            // 
            cmbSymbol2nd.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSymbol2nd.FormattingEnabled = true;
            cmbSymbol2nd.Location = new Point(140, 86);
            cmbSymbol2nd.Name = "cmbSymbol2nd";
            cmbSymbol2nd.Size = new Size(58, 23);
            cmbSymbol2nd.TabIndex = 31;
            // 
            // cmbSymbol1st
            // 
            cmbSymbol1st.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSymbol1st.FormattingEnabled = true;
            cmbSymbol1st.Location = new Point(140, 57);
            cmbSymbol1st.Name = "cmbSymbol1st";
            cmbSymbol1st.Size = new Size(58, 23);
            cmbSymbol1st.TabIndex = 5;
            // 
            // cmbForex5th
            // 
            cmbForex5th.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForex5th.FormattingEnabled = true;
            cmbForex5th.Location = new Point(17, 170);
            cmbForex5th.Name = "cmbForex5th";
            cmbForex5th.Size = new Size(117, 23);
            cmbForex5th.TabIndex = 30;
            // 
            // cmbForex4th
            // 
            cmbForex4th.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForex4th.FormattingEnabled = true;
            cmbForex4th.Location = new Point(17, 141);
            cmbForex4th.Name = "cmbForex4th";
            cmbForex4th.Size = new Size(117, 23);
            cmbForex4th.TabIndex = 29;
            // 
            // cmbForex3rd
            // 
            cmbForex3rd.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForex3rd.FormattingEnabled = true;
            cmbForex3rd.Location = new Point(17, 114);
            cmbForex3rd.Name = "cmbForex3rd";
            cmbForex3rd.Size = new Size(117, 23);
            cmbForex3rd.TabIndex = 28;
            // 
            // cmbForex2nd
            // 
            cmbForex2nd.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForex2nd.FormattingEnabled = true;
            cmbForex2nd.Location = new Point(17, 85);
            cmbForex2nd.Name = "cmbForex2nd";
            cmbForex2nd.Size = new Size(117, 23);
            cmbForex2nd.TabIndex = 27;
            // 
            // cmbForex1st
            // 
            cmbForex1st.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForex1st.FormattingEnabled = true;
            cmbForex1st.Location = new Point(17, 56);
            cmbForex1st.Name = "cmbForex1st";
            cmbForex1st.Size = new Size(117, 23);
            cmbForex1st.TabIndex = 26;
            // 
            // cmbBanks
            // 
            cmbBanks.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBanks.FormattingEnabled = true;
            cmbBanks.Location = new Point(17, 25);
            cmbBanks.Name = "cmbBanks";
            cmbBanks.Size = new Size(155, 23);
            cmbBanks.TabIndex = 4;
            cmbBanks.SelectedIndexChanged += cmbBanks_SelectedIndexChanged;
            // 
            // nmrForexSelling5th
            // 
            nmrForexSelling5th.DecimalPlaces = 4;
            nmrForexSelling5th.Location = new Point(304, 172);
            nmrForexSelling5th.Name = "nmrForexSelling5th";
            nmrForexSelling5th.Size = new Size(94, 23);
            nmrForexSelling5th.TabIndex = 25;
            // 
            // nmrForexSelling4th
            // 
            nmrForexSelling4th.DecimalPlaces = 4;
            nmrForexSelling4th.Location = new Point(304, 143);
            nmrForexSelling4th.Name = "nmrForexSelling4th";
            nmrForexSelling4th.Size = new Size(94, 23);
            nmrForexSelling4th.TabIndex = 24;
            // 
            // nmrForexSelling3rd
            // 
            nmrForexSelling3rd.DecimalPlaces = 4;
            nmrForexSelling3rd.Location = new Point(304, 113);
            nmrForexSelling3rd.Name = "nmrForexSelling3rd";
            nmrForexSelling3rd.Size = new Size(94, 23);
            nmrForexSelling3rd.TabIndex = 23;
            // 
            // nmrForexSelling2nd
            // 
            nmrForexSelling2nd.DecimalPlaces = 4;
            nmrForexSelling2nd.Location = new Point(304, 86);
            nmrForexSelling2nd.Name = "nmrForexSelling2nd";
            nmrForexSelling2nd.Size = new Size(94, 23);
            nmrForexSelling2nd.TabIndex = 22;
            // 
            // nmrForexSelling1st
            // 
            nmrForexSelling1st.DecimalPlaces = 4;
            nmrForexSelling1st.Location = new Point(304, 57);
            nmrForexSelling1st.Name = "nmrForexSelling1st";
            nmrForexSelling1st.Size = new Size(94, 23);
            nmrForexSelling1st.TabIndex = 21;
            // 
            // lblSelling
            // 
            lblSelling.AutoSize = true;
            lblSelling.Location = new Point(325, 34);
            lblSelling.Name = "lblSelling";
            lblSelling.Size = new Size(31, 15);
            lblSelling.TabIndex = 20;
            lblSelling.Text = "Satış";
            // 
            // lblBuying
            // 
            lblBuying.AutoSize = true;
            lblBuying.Location = new Point(233, 34);
            lblBuying.Name = "lblBuying";
            lblBuying.Size = new Size(26, 15);
            lblBuying.TabIndex = 19;
            lblBuying.Text = "Alış";
            // 
            // nmrForexBuying5th
            // 
            nmrForexBuying5th.DecimalPlaces = 4;
            nmrForexBuying5th.Location = new Point(204, 172);
            nmrForexBuying5th.Name = "nmrForexBuying5th";
            nmrForexBuying5th.Size = new Size(94, 23);
            nmrForexBuying5th.TabIndex = 18;
            // 
            // nmrForexBuying4th
            // 
            nmrForexBuying4th.DecimalPlaces = 4;
            nmrForexBuying4th.Location = new Point(204, 144);
            nmrForexBuying4th.Name = "nmrForexBuying4th";
            nmrForexBuying4th.Size = new Size(94, 23);
            nmrForexBuying4th.TabIndex = 15;
            // 
            // nmrForexBuying3rd
            // 
            nmrForexBuying3rd.DecimalPlaces = 4;
            nmrForexBuying3rd.Location = new Point(204, 115);
            nmrForexBuying3rd.Name = "nmrForexBuying3rd";
            nmrForexBuying3rd.Size = new Size(94, 23);
            nmrForexBuying3rd.TabIndex = 14;
            // 
            // nmrForexBuying2nd
            // 
            nmrForexBuying2nd.DecimalPlaces = 4;
            nmrForexBuying2nd.Location = new Point(204, 86);
            nmrForexBuying2nd.Name = "nmrForexBuying2nd";
            nmrForexBuying2nd.Size = new Size(94, 23);
            nmrForexBuying2nd.TabIndex = 13;
            // 
            // nmrForexBuying1st
            // 
            nmrForexBuying1st.DecimalPlaces = 4;
            nmrForexBuying1st.Location = new Point(204, 57);
            nmrForexBuying1st.Name = "nmrForexBuying1st";
            nmrForexBuying1st.Size = new Size(94, 23);
            nmrForexBuying1st.TabIndex = 12;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(108, 207);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(92, 30);
            btnClear.TabIndex = 2;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(219, 207);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(92, 30);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Ekle";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // grbCrossRates
            // 
            grbCrossRates.Controls.Add(pbSwap);
            grbCrossRates.Controls.Add(grbTo);
            grbCrossRates.Controls.Add(grbFrom);
            grbCrossRates.Location = new Point(786, 28);
            grbCrossRates.Name = "grbCrossRates";
            grbCrossRates.Size = new Size(215, 208);
            grbCrossRates.TabIndex = 4;
            grbCrossRates.TabStop = false;
            grbCrossRates.Text = "Çarpraz Kurlar";
            // 
            // pbSwap
            // 
            pbSwap.BackColor = Color.Transparent;
            pbSwap.Cursor = Cursors.Hand;
            pbSwap.Image = (Image)resources.GetObject("pbSwap.Image");
            pbSwap.Location = new Point(178, 9);
            pbSwap.Name = "pbSwap";
            pbSwap.Size = new Size(25, 20);
            pbSwap.SizeMode = PictureBoxSizeMode.StretchImage;
            pbSwap.TabIndex = 8;
            pbSwap.TabStop = false;
            pbSwap.Click += pbSwap_Click;
            pbSwap.MouseEnter += pbSwap_MouseEnter;
            pbSwap.MouseLeave += pbSwap_MouseLeave;
            // 
            // grbTo
            // 
            grbTo.Controls.Add(lblToMoney);
            grbTo.Controls.Add(cmbToMoney);
            grbTo.Controls.Add(nmrToMoneyUnite);
            grbTo.Controls.Add(lblToMoneyUnite);
            grbTo.Location = new Point(6, 109);
            grbTo.Name = "grbTo";
            grbTo.Size = new Size(200, 83);
            grbTo.TabIndex = 9;
            grbTo.TabStop = false;
            grbTo.Text = "Çevrilmek İstenen";
            // 
            // lblToMoney
            // 
            lblToMoney.AutoSize = true;
            lblToMoney.Location = new Point(6, 24);
            lblToMoney.Name = "lblToMoney";
            lblToMoney.Size = new Size(64, 15);
            lblToMoney.TabIndex = 6;
            lblToMoney.Text = "Para Birimi";
            // 
            // cmbToMoney
            // 
            cmbToMoney.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbToMoney.FormattingEnabled = true;
            cmbToMoney.Location = new Point(93, 21);
            cmbToMoney.Name = "cmbToMoney";
            cmbToMoney.Size = new Size(101, 23);
            cmbToMoney.TabIndex = 2;
            cmbToMoney.DropDown += cmbToMoney_DropDown;
            cmbToMoney.SelectedIndexChanged += cmbToMoney_SelectedIndexChanged;
            // 
            // nmrToMoneyUnite
            // 
            nmrToMoneyUnite.DecimalPlaces = 2;
            nmrToMoneyUnite.Location = new Point(93, 50);
            nmrToMoneyUnite.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrToMoneyUnite.Name = "nmrToMoneyUnite";
            nmrToMoneyUnite.Size = new Size(101, 23);
            nmrToMoneyUnite.TabIndex = 3;
            // 
            // lblToMoneyUnite
            // 
            lblToMoneyUnite.AutoSize = true;
            lblToMoneyUnite.Location = new Point(6, 52);
            lblToMoneyUnite.Name = "lblToMoneyUnite";
            lblToMoneyUnite.Size = new Size(75, 15);
            lblToMoneyUnite.TabIndex = 7;
            lblToMoneyUnite.Text = "Birim Miktarı";
            // 
            // grbFrom
            // 
            grbFrom.Controls.Add(lblFromMoney);
            grbFrom.Controls.Add(cmbFromMoney);
            grbFrom.Controls.Add(lblFromMoneyUnite);
            grbFrom.Controls.Add(nmrFromMoneyUnite);
            grbFrom.Location = new Point(6, 22);
            grbFrom.Name = "grbFrom";
            grbFrom.Size = new Size(200, 81);
            grbFrom.TabIndex = 8;
            grbFrom.TabStop = false;
            grbFrom.Text = "Çevrilen";
            // 
            // lblFromMoney
            // 
            lblFromMoney.AutoSize = true;
            lblFromMoney.Location = new Point(6, 22);
            lblFromMoney.Name = "lblFromMoney";
            lblFromMoney.Size = new Size(64, 15);
            lblFromMoney.TabIndex = 4;
            lblFromMoney.Text = "Para Birimi";
            // 
            // cmbFromMoney
            // 
            cmbFromMoney.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFromMoney.FormattingEnabled = true;
            cmbFromMoney.Location = new Point(93, 19);
            cmbFromMoney.Name = "cmbFromMoney";
            cmbFromMoney.Size = new Size(101, 23);
            cmbFromMoney.TabIndex = 0;
            cmbFromMoney.DropDown += cmbFromMoney_DropDown;
            cmbFromMoney.SelectedIndexChanged += cmbFromMoney_SelectedIndexChanged;
            // 
            // lblFromMoneyUnite
            // 
            lblFromMoneyUnite.AutoSize = true;
            lblFromMoneyUnite.Location = new Point(6, 50);
            lblFromMoneyUnite.Name = "lblFromMoneyUnite";
            lblFromMoneyUnite.Size = new Size(75, 15);
            lblFromMoneyUnite.TabIndex = 5;
            lblFromMoneyUnite.Text = "Birim Miktarı";
            // 
            // nmrFromMoneyUnite
            // 
            nmrFromMoneyUnite.DecimalPlaces = 2;
            nmrFromMoneyUnite.Location = new Point(93, 48);
            nmrFromMoneyUnite.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrFromMoneyUnite.Name = "nmrFromMoneyUnite";
            nmrFromMoneyUnite.Size = new Size(101, 23);
            nmrFromMoneyUnite.TabIndex = 1;
            // 
            // AddProductScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1010, 248);
            Controls.Add(grbCrossRates);
            Controls.Add(btnAdd);
            Controls.Add(btnClear);
            Controls.Add(grbExchangeInfo);
            Controls.Add(grbProduct);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "AddProductScreen";
            Text = "Ürün Ekle";
            Load += AddProductScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbProduct.ResumeLayout(false);
            grbProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrPercent).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrCost).EndInit();
            grbExchangeInfo.ResumeLayout(false);
            grbExchangeInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling5th).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling4th).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling3rd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling2nd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexSelling1st).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying5th).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying4th).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying3rd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying2nd).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrForexBuying1st).EndInit();
            grbCrossRates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbSwap).EndInit();
            grbTo.ResumeLayout(false);
            grbTo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrToMoneyUnite).EndInit();
            grbFrom.ResumeLayout(false);
            grbFrom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrFromMoneyUnite).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbProduct;
        private NumericUpDown nmrCost;
        private TextBox txtName;
        private Label lblPercent;
        private Label lblStock;
        private Label lblPrice;
        private Label lblCost;
        private Label lblName;
        private GroupBox grbExchangeInfo;
        private ComboBox cmbCost;
        private ComboBox cmbPrice;
        private NumericUpDown nmrPrice;
        private NumericUpDown nmrPercent;
        private NumericUpDown nmrStock;
        private Button btnClear;
        private Button btnAdd;
        private NumericUpDown nmrForexBuying4th;
        private NumericUpDown nmrForexBuying3rd;
        private NumericUpDown nmrForexBuying2nd;
        private NumericUpDown nmrForexBuying1st;
        private NumericUpDown nmrForexBuying5th;
        private NumericUpDown nmrForexSelling5th;
        private NumericUpDown nmrForexSelling4th;
        private NumericUpDown nmrForexSelling3rd;
        private NumericUpDown nmrForexSelling2nd;
        private NumericUpDown nmrForexSelling1st;
        private Label lblSelling;
        private Label lblBuying;
        private ComboBox cmbBanks;
        private ComboBox cmbForex5th;
        private ComboBox cmbForex4th;
        private ComboBox cmbForex3rd;
        private ComboBox cmbForex2nd;
        private ComboBox cmbForex1st;
        private GroupBox grbCrossRates;
        private NumericUpDown nmrToMoneyUnite;
        private ComboBox cmbToMoney;
        private NumericUpDown nmrFromMoneyUnite;
        private ComboBox cmbFromMoney;
        private Label lblToMoneyUnite;
        private Label lblToMoney;
        private Label lblFromMoneyUnite;
        private Label lblFromMoney;
        private GroupBox grbFrom;
        private GroupBox grbTo;
        private ComboBox cmbSymbol5th;
        private ComboBox cmbSymbol4th;
        private ComboBox cmbSymbol3rd;
        private ComboBox cmbSymbol2nd;
        private ComboBox cmbSymbol1st;
        private Button btnRefresh;
        private PictureBox pbSwap;
        private ToolTip ttSwap;
    }
}