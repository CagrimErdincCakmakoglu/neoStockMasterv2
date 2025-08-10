namespace neoStockMasterv2.Forms
{
    partial class PricingOrderScreen
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
            grbPriceDetails = new GroupBox();
            chbLockForex = new CheckBox();
            chbLockPrice = new CheckBox();
            grbTax = new GroupBox();
            cmbVAT = new ComboBox();
            cmbSCT = new ComboBox();
            grbDisc = new GroupBox();
            nmrDisc = new NumericUpDown();
            cmbDisc = new ComboBox();
            nmrTotalTax = new NumericUpDown();
            lblTotalTax = new Label();
            nmrTotalDisc = new NumericUpDown();
            lblTotalDisc = new Label();
            nmrTotalPrice = new NumericUpDown();
            lblTotalPrice = new Label();
            dgwProducts = new DataGridView();
            grbCustomerMsg = new GroupBox();
            btnWhatsapp = new Button();
            btnCopy = new Button();
            btnClear = new Button();
            richTextBox1 = new RichTextBox();
            grbOrderDetails = new GroupBox();
            btnAddOrder = new Button();
            mskPhoneNo = new MaskedTextBox();
            txtCustomerName = new TextBox();
            txtCargoTracker = new TextBox();
            txtCargo = new TextBox();
            cmbCargo = new ComboBox();
            cmbOrderStatus = new ComboBox();
            cmbPayment = new ComboBox();
            dgwOrderDetails = new DataGridView();
            menuStripLanguage.SuspendLayout();
            grbPriceDetails.SuspendLayout();
            grbTax.SuspendLayout();
            grbDisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDisc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalTax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalDisc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).BeginInit();
            grbCustomerMsg.SuspendLayout();
            grbOrderDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).BeginInit();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(968, 24);
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
            // grbPriceDetails
            // 
            grbPriceDetails.Controls.Add(chbLockForex);
            grbPriceDetails.Controls.Add(chbLockPrice);
            grbPriceDetails.Controls.Add(grbTax);
            grbPriceDetails.Controls.Add(grbDisc);
            grbPriceDetails.Controls.Add(nmrTotalTax);
            grbPriceDetails.Controls.Add(lblTotalTax);
            grbPriceDetails.Controls.Add(nmrTotalDisc);
            grbPriceDetails.Controls.Add(lblTotalDisc);
            grbPriceDetails.Controls.Add(nmrTotalPrice);
            grbPriceDetails.Controls.Add(lblTotalPrice);
            grbPriceDetails.Controls.Add(dgwProducts);
            grbPriceDetails.Location = new Point(12, 27);
            grbPriceDetails.Name = "grbPriceDetails";
            grbPriceDetails.Size = new Size(691, 338);
            grbPriceDetails.TabIndex = 1;
            grbPriceDetails.TabStop = false;
            grbPriceDetails.Text = "Fiyat Detayları";
            // 
            // chbLockForex
            // 
            chbLockForex.AutoSize = true;
            chbLockForex.Checked = true;
            chbLockForex.CheckState = CheckState.Checked;
            chbLockForex.Location = new Point(478, 0);
            chbLockForex.Name = "chbLockForex";
            chbLockForex.Size = new Size(114, 19);
            chbLockForex.TabIndex = 9;
            chbLockForex.Text = "Satış Döviz Kilitle";
            chbLockForex.UseVisualStyleBackColor = true;
            chbLockForex.CheckedChanged += chbLockForex_CheckedChanged;
            // 
            // chbLockPrice
            // 
            chbLockPrice.AutoSize = true;
            chbLockPrice.Checked = true;
            chbLockPrice.CheckState = CheckState.Checked;
            chbLockPrice.Location = new Point(397, 0);
            chbLockPrice.Name = "chbLockPrice";
            chbLockPrice.Size = new Size(83, 19);
            chbLockPrice.TabIndex = 8;
            chbLockPrice.Text = "Fiyat Kilitle";
            chbLockPrice.UseVisualStyleBackColor = true;
            chbLockPrice.CheckedChanged += chbLockPrice_CheckedChanged;
            // 
            // grbTax
            // 
            grbTax.Controls.Add(cmbVAT);
            grbTax.Controls.Add(cmbSCT);
            grbTax.Location = new Point(356, 260);
            grbTax.Name = "grbTax";
            grbTax.Size = new Size(301, 54);
            grbTax.TabIndex = 2;
            grbTax.TabStop = false;
            grbTax.Text = "Vergi";
            // 
            // cmbVAT
            // 
            cmbVAT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVAT.FormattingEnabled = true;
            cmbVAT.Location = new Point(156, 21);
            cmbVAT.Name = "cmbVAT";
            cmbVAT.Size = new Size(139, 23);
            cmbVAT.TabIndex = 1;
            // 
            // cmbSCT
            // 
            cmbSCT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSCT.FormattingEnabled = true;
            cmbSCT.Location = new Point(6, 21);
            cmbSCT.Name = "cmbSCT";
            cmbSCT.Size = new Size(139, 23);
            cmbSCT.TabIndex = 0;
            // 
            // grbDisc
            // 
            grbDisc.Controls.Add(nmrDisc);
            grbDisc.Controls.Add(cmbDisc);
            grbDisc.Location = new Point(23, 260);
            grbDisc.Name = "grbDisc";
            grbDisc.Size = new Size(301, 54);
            grbDisc.TabIndex = 7;
            grbDisc.TabStop = false;
            grbDisc.Text = "İndirim";
            // 
            // nmrDisc
            // 
            nmrDisc.DecimalPlaces = 2;
            nmrDisc.Location = new Point(166, 22);
            nmrDisc.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrDisc.Name = "nmrDisc";
            nmrDisc.Size = new Size(120, 23);
            nmrDisc.TabIndex = 3;
            // 
            // cmbDisc
            // 
            cmbDisc.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisc.FormattingEnabled = true;
            cmbDisc.Location = new Point(13, 22);
            cmbDisc.Name = "cmbDisc";
            cmbDisc.Size = new Size(121, 23);
            cmbDisc.TabIndex = 2;
            // 
            // nmrTotalTax
            // 
            nmrTotalTax.DecimalPlaces = 2;
            nmrTotalTax.Enabled = false;
            nmrTotalTax.Location = new Point(537, 221);
            nmrTotalTax.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrTotalTax.Name = "nmrTotalTax";
            nmrTotalTax.Size = new Size(120, 23);
            nmrTotalTax.TabIndex = 6;
            // 
            // lblTotalTax
            // 
            lblTotalTax.AutoSize = true;
            lblTotalTax.Location = new Point(498, 223);
            lblTotalTax.Name = "lblTotalTax";
            lblTotalTax.Size = new Size(33, 15);
            lblTotalTax.TabIndex = 5;
            lblTotalTax.Text = "Vergi";
            // 
            // nmrTotalDisc
            // 
            nmrTotalDisc.DecimalPlaces = 2;
            nmrTotalDisc.Enabled = false;
            nmrTotalDisc.Location = new Point(341, 221);
            nmrTotalDisc.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrTotalDisc.Name = "nmrTotalDisc";
            nmrTotalDisc.Size = new Size(120, 23);
            nmrTotalDisc.TabIndex = 4;
            // 
            // lblTotalDisc
            // 
            lblTotalDisc.AutoSize = true;
            lblTotalDisc.Location = new Point(248, 223);
            lblTotalDisc.Name = "lblTotalDisc";
            lblTotalDisc.Size = new Size(87, 15);
            lblTotalDisc.TabIndex = 3;
            lblTotalDisc.Text = "Toplam İndirim";
            // 
            // nmrTotalPrice
            // 
            nmrTotalPrice.DecimalPlaces = 2;
            nmrTotalPrice.Enabled = false;
            nmrTotalPrice.Location = new Point(101, 221);
            nmrTotalPrice.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrTotalPrice.Name = "nmrTotalPrice";
            nmrTotalPrice.Size = new Size(120, 23);
            nmrTotalPrice.TabIndex = 2;
            // 
            // lblTotalPrice
            // 
            lblTotalPrice.AutoSize = true;
            lblTotalPrice.Location = new Point(19, 223);
            lblTotalPrice.Name = "lblTotalPrice";
            lblTotalPrice.Size = new Size(76, 15);
            lblTotalPrice.TabIndex = 1;
            lblTotalPrice.Text = "Toplam Tutar";
            // 
            // dgwProducts
            // 
            dgwProducts.AllowUserToAddRows = false;
            dgwProducts.AllowUserToDeleteRows = false;
            dgwProducts.AllowUserToResizeColumns = false;
            dgwProducts.AllowUserToResizeRows = false;
            dgwProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwProducts.Location = new Point(6, 22);
            dgwProducts.MultiSelect = false;
            dgwProducts.Name = "dgwProducts";
            dgwProducts.RowHeadersVisible = false;
            dgwProducts.Size = new Size(679, 188);
            dgwProducts.TabIndex = 0;
            dgwProducts.CellValueChanged += dgwProducts_CellValueChanged;
            dgwProducts.EditingControlShowing += dgwProducts_EditingControlShowing;
            dgwProducts.KeyUp += dgwProducts_KeyUp;
            // 
            // grbCustomerMsg
            // 
            grbCustomerMsg.Controls.Add(btnWhatsapp);
            grbCustomerMsg.Controls.Add(btnCopy);
            grbCustomerMsg.Controls.Add(btnClear);
            grbCustomerMsg.Controls.Add(richTextBox1);
            grbCustomerMsg.Location = new Point(12, 371);
            grbCustomerMsg.Name = "grbCustomerMsg";
            grbCustomerMsg.Size = new Size(691, 112);
            grbCustomerMsg.TabIndex = 2;
            grbCustomerMsg.TabStop = false;
            grbCustomerMsg.Text = "Müşteri Bilgilendirme Metni";
            // 
            // btnWhatsapp
            // 
            btnWhatsapp.Location = new Point(356, 74);
            btnWhatsapp.Name = "btnWhatsapp";
            btnWhatsapp.Size = new Size(145, 23);
            btnWhatsapp.TabIndex = 3;
            btnWhatsapp.Text = "WhatsApp'ı Aç";
            btnWhatsapp.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(249, 74);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(75, 23);
            btnCopy.TabIndex = 2;
            btnCopy.Text = "Kopyala";
            btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(146, 74);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 1;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(6, 22);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(678, 46);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // grbOrderDetails
            // 
            grbOrderDetails.Controls.Add(btnAddOrder);
            grbOrderDetails.Controls.Add(mskPhoneNo);
            grbOrderDetails.Controls.Add(txtCustomerName);
            grbOrderDetails.Controls.Add(txtCargoTracker);
            grbOrderDetails.Controls.Add(txtCargo);
            grbOrderDetails.Controls.Add(cmbCargo);
            grbOrderDetails.Controls.Add(cmbOrderStatus);
            grbOrderDetails.Controls.Add(cmbPayment);
            grbOrderDetails.Controls.Add(dgwOrderDetails);
            grbOrderDetails.Location = new Point(709, 27);
            grbOrderDetails.Name = "grbOrderDetails";
            grbOrderDetails.Size = new Size(249, 456);
            grbOrderDetails.TabIndex = 3;
            grbOrderDetails.TabStop = false;
            grbOrderDetails.Text = "Sipariş Detayları";
            // 
            // btnAddOrder
            // 
            btnAddOrder.Location = new Point(6, 425);
            btnAddOrder.Name = "btnAddOrder";
            btnAddOrder.Size = new Size(237, 23);
            btnAddOrder.TabIndex = 8;
            btnAddOrder.Text = "Sipariş Ekle";
            btnAddOrder.UseVisualStyleBackColor = true;
            // 
            // mskPhoneNo
            // 
            mskPhoneNo.Location = new Point(6, 396);
            mskPhoneNo.Mask = "(999) 000-0000";
            mskPhoneNo.Name = "mskPhoneNo";
            mskPhoneNo.Size = new Size(237, 23);
            mskPhoneNo.TabIndex = 7;
            // 
            // txtCustomerName
            // 
            txtCustomerName.Location = new Point(6, 367);
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.Size = new Size(237, 23);
            txtCustomerName.TabIndex = 6;
            txtCustomerName.Text = "Müşteri İsmini Yazınız";
            txtCustomerName.Enter += txtCustomerName_Enter;
            txtCustomerName.Leave += txtCustomerName_Leave;
            // 
            // txtCargoTracker
            // 
            txtCargoTracker.Location = new Point(6, 339);
            txtCargoTracker.Name = "txtCargoTracker";
            txtCargoTracker.Size = new Size(237, 23);
            txtCargoTracker.TabIndex = 5;
            txtCargoTracker.Text = "Kargo Takip Numarasını Yazınız";
            txtCargoTracker.Enter += txtCargoTracker_Enter;
            txtCargoTracker.Leave += txtCargoTracker_Leave;
            // 
            // txtCargo
            // 
            txtCargo.Location = new Point(6, 310);
            txtCargo.Name = "txtCargo";
            txtCargo.Size = new Size(237, 23);
            txtCargo.TabIndex = 4;
            txtCargo.Text = "Diğer Kargo Şirketinin Adını Yazınız";
            txtCargo.Enter += txtCargo_Enter;
            txtCargo.Leave += txtCargo_Leave;
            // 
            // cmbCargo
            // 
            cmbCargo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCargo.FormattingEnabled = true;
            cmbCargo.Location = new Point(6, 281);
            cmbCargo.Name = "cmbCargo";
            cmbCargo.Size = new Size(237, 23);
            cmbCargo.TabIndex = 3;
            cmbCargo.SelectedIndexChanged += cmbCargo_SelectedIndexChanged;
            // 
            // cmbOrderStatus
            // 
            cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrderStatus.FormattingEnabled = true;
            cmbOrderStatus.Location = new Point(6, 252);
            cmbOrderStatus.Name = "cmbOrderStatus";
            cmbOrderStatus.Size = new Size(237, 23);
            cmbOrderStatus.TabIndex = 2;
            cmbOrderStatus.SelectedIndexChanged += cmbOrderStatus_SelectedIndexChanged;
            // 
            // cmbPayment
            // 
            cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayment.FormattingEnabled = true;
            cmbPayment.Location = new Point(6, 223);
            cmbPayment.Name = "cmbPayment";
            cmbPayment.Size = new Size(237, 23);
            cmbPayment.TabIndex = 1;
            cmbPayment.SelectedIndexChanged += cmbPayment_SelectedIndexChanged;
            // 
            // dgwOrderDetails
            // 
            dgwOrderDetails.AllowUserToAddRows = false;
            dgwOrderDetails.AllowUserToDeleteRows = false;
            dgwOrderDetails.AllowUserToResizeColumns = false;
            dgwOrderDetails.AllowUserToResizeRows = false;
            dgwOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwOrderDetails.Location = new Point(3, 22);
            dgwOrderDetails.Name = "dgwOrderDetails";
            dgwOrderDetails.RowHeadersVisible = false;
            dgwOrderDetails.Size = new Size(240, 188);
            dgwOrderDetails.TabIndex = 0;
            // 
            // PricingOrderScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 496);
            Controls.Add(grbOrderDetails);
            Controls.Add(grbCustomerMsg);
            Controls.Add(grbPriceDetails);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            Name = "PricingOrderScreen";
            Text = "Fiyat Hesaplama - Sipariş Oluşturma";
            Load += PricingOrderScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbPriceDetails.ResumeLayout(false);
            grbPriceDetails.PerformLayout();
            grbTax.ResumeLayout(false);
            grbDisc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrDisc).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalTax).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalDisc).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmrTotalPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).EndInit();
            grbCustomerMsg.ResumeLayout(false);
            grbOrderDetails.ResumeLayout(false);
            grbOrderDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbPriceDetails;
        private Label lblTotalPrice;
        private DataGridView dgwProducts;
        private NumericUpDown nmrTotalPrice;
        private NumericUpDown nmrTotalTax;
        private Label lblTotalTax;
        private NumericUpDown nmrTotalDisc;
        private Label lblTotalDisc;
        private GroupBox grbDisc;
        private NumericUpDown nmrDisc;
        private ComboBox cmbDisc;
        private GroupBox grbTax;
        private ComboBox cmbSCT;
        private GroupBox grbCustomerMsg;
        private Button btnCopy;
        private Button btnClear;
        private RichTextBox richTextBox1;
        private Button btnWhatsapp;
        private GroupBox grbOrderDetails;
        private DataGridView dgwOrderDetails;
        private Button btnAddOrder;
        private MaskedTextBox mskPhoneNo;
        private TextBox txtCustomerName;
        private TextBox txtCargoTracker;
        private TextBox txtCargo;
        private ComboBox cmbCargo;
        private ComboBox cmbOrderStatus;
        private ComboBox cmbPayment;
        private ComboBox cmbVAT;
        private CheckBox chbLockForex;
        private CheckBox chbLockPrice;
    }
}