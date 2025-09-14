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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PricingOrderScreen));
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbPriceDetails = new GroupBox();
            lwTax = new ListView();
            lwDiscList = new ListView();
            lwTotal = new ListView();
            grbCargo = new GroupBox();
            nmrCargo = new NumericUpDown();
            lwDisc = new ListView();
            chbLockForex = new CheckBox();
            chbLockPrice = new CheckBox();
            grbTax = new GroupBox();
            cmbVAT = new ComboBox();
            cmbSCT = new ComboBox();
            grbDisc = new GroupBox();
            nmrDisc = new NumericUpDown();
            cmbDisc = new ComboBox();
            lblTotalTax = new Label();
            lblTotalDisc = new Label();
            lblTotalPrice = new Label();
            dgwProducts = new DataGridView();
            grbCustomerMsg = new GroupBox();
            btnWhatsapp = new Button();
            btnCopy = new Button();
            btnClear = new Button();
            rchMessage = new RichTextBox();
            pbScale = new PictureBox();
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
            hoverTimer = new System.Windows.Forms.Timer(components);
            btnTotal = new Button();
            grbTable = new GroupBox();
            btnAll = new Button();
            btnTax = new Button();
            btnDisc = new Button();
            menuStripLanguage.SuspendLayout();
            grbPriceDetails.SuspendLayout();
            grbCargo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrCargo).BeginInit();
            grbTax.SuspendLayout();
            grbDisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDisc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).BeginInit();
            grbCustomerMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbScale).BeginInit();
            grbOrderDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).BeginInit();
            grbTable.SuspendLayout();
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
            grbPriceDetails.Controls.Add(lwTax);
            grbPriceDetails.Controls.Add(lwDiscList);
            grbPriceDetails.Controls.Add(lwTotal);
            grbPriceDetails.Controls.Add(grbCargo);
            grbPriceDetails.Controls.Add(lwDisc);
            grbPriceDetails.Controls.Add(chbLockForex);
            grbPriceDetails.Controls.Add(chbLockPrice);
            grbPriceDetails.Controls.Add(grbTax);
            grbPriceDetails.Controls.Add(grbDisc);
            grbPriceDetails.Controls.Add(lblTotalTax);
            grbPriceDetails.Controls.Add(lblTotalDisc);
            grbPriceDetails.Controls.Add(lblTotalPrice);
            grbPriceDetails.Controls.Add(dgwProducts);
            grbPriceDetails.Location = new Point(12, 27);
            grbPriceDetails.Name = "grbPriceDetails";
            grbPriceDetails.Size = new Size(691, 456);
            grbPriceDetails.TabIndex = 1;
            grbPriceDetails.TabStop = false;
            grbPriceDetails.Text = "Fiyat Detayları";
            // 
            // lwTax
            // 
            lwTax.Location = new Point(531, 221);
            lwTax.Name = "lwTax";
            lwTax.Size = new Size(154, 67);
            lwTax.TabIndex = 15;
            lwTax.UseCompatibleStateImageBehavior = false;
            lwTax.DoubleClick += lwTax_DoubleClick;
            // 
            // lwDiscList
            // 
            lwDiscList.Location = new Point(341, 221);
            lwDiscList.Name = "lwDiscList";
            lwDiscList.Size = new Size(154, 67);
            lwDiscList.TabIndex = 14;
            lwDiscList.UseCompatibleStateImageBehavior = false;
            lwDiscList.DoubleClick += lwDiscList_DoubleClick;
            // 
            // lwTotal
            // 
            lwTotal.Location = new Point(88, 221);
            lwTotal.Name = "lwTotal";
            lwTotal.Size = new Size(154, 67);
            lwTotal.TabIndex = 13;
            lwTotal.UseCompatibleStateImageBehavior = false;
            lwTotal.DoubleClick += lwTotal_DoubleClick;
            // 
            // grbCargo
            // 
            grbCargo.Controls.Add(nmrCargo);
            grbCargo.Location = new Point(545, 396);
            grbCargo.Name = "grbCargo";
            grbCargo.Size = new Size(140, 54);
            grbCargo.TabIndex = 12;
            grbCargo.TabStop = false;
            grbCargo.Text = "Kargo Bedeli";
            // 
            // nmrCargo
            // 
            nmrCargo.DecimalPlaces = 2;
            nmrCargo.Location = new Point(6, 21);
            nmrCargo.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrCargo.Name = "nmrCargo";
            nmrCargo.Size = new Size(128, 23);
            nmrCargo.TabIndex = 0;
            nmrCargo.ValueChanged += nmrCargo_ValueChanged;
            nmrCargo.KeyUp += nmrCargo_KeyUp;
            // 
            // lwDisc
            // 
            lwDisc.Location = new Point(6, 294);
            lwDisc.MultiSelect = false;
            lwDisc.Name = "lwDisc";
            lwDisc.Size = new Size(679, 94);
            lwDisc.TabIndex = 10;
            lwDisc.UseCompatibleStateImageBehavior = false;
            lwDisc.ColumnWidthChanging += lwDisc_ColumnWidthChanging;
            lwDisc.ItemCheck += lwDisc_ItemCheck;
            lwDisc.ItemChecked += lwDisc_ItemChecked;
            lwDisc.MouseClick += lwDisc_MouseClick;
            // 
            // chbLockForex
            // 
            chbLockForex.AutoSize = true;
            chbLockForex.Checked = true;
            chbLockForex.CheckState = CheckState.Checked;
            chbLockForex.Location = new Point(491, 0);
            chbLockForex.Name = "chbLockForex";
            chbLockForex.Size = new Size(125, 19);
            chbLockForex.TabIndex = 9;
            chbLockForex.Text = "Para Birimini Kilitle";
            chbLockForex.UseVisualStyleBackColor = true;
            chbLockForex.CheckedChanged += chbLockForex_CheckedChanged;
            // 
            // chbLockPrice
            // 
            chbLockPrice.AutoSize = true;
            chbLockPrice.Checked = true;
            chbLockPrice.CheckState = CheckState.Checked;
            chbLockPrice.Location = new Point(388, 0);
            chbLockPrice.Name = "chbLockPrice";
            chbLockPrice.Size = new Size(99, 19);
            chbLockPrice.TabIndex = 8;
            chbLockPrice.Text = "Fiyatları Kilitle";
            chbLockPrice.UseVisualStyleBackColor = true;
            chbLockPrice.CheckedChanged += chbLockPrice_CheckedChanged;
            // 
            // grbTax
            // 
            grbTax.Controls.Add(cmbVAT);
            grbTax.Controls.Add(cmbSCT);
            grbTax.Location = new Point(288, 396);
            grbTax.Name = "grbTax";
            grbTax.Size = new Size(251, 54);
            grbTax.TabIndex = 2;
            grbTax.TabStop = false;
            grbTax.Text = "Vergi";
            // 
            // cmbVAT
            // 
            cmbVAT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVAT.FormattingEnabled = true;
            cmbVAT.Location = new Point(127, 21);
            cmbVAT.Name = "cmbVAT";
            cmbVAT.Size = new Size(116, 23);
            cmbVAT.TabIndex = 1;
            cmbVAT.SelectedIndexChanged += cmbVAT_SelectedIndexChanged;
            // 
            // cmbSCT
            // 
            cmbSCT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSCT.FormattingEnabled = true;
            cmbSCT.Location = new Point(6, 21);
            cmbSCT.Name = "cmbSCT";
            cmbSCT.Size = new Size(116, 23);
            cmbSCT.TabIndex = 0;
            cmbSCT.SelectedIndexChanged += cmbSCT_SelectedIndexChanged;
            // 
            // grbDisc
            // 
            grbDisc.Controls.Add(nmrDisc);
            grbDisc.Controls.Add(cmbDisc);
            grbDisc.Location = new Point(6, 396);
            grbDisc.Name = "grbDisc";
            grbDisc.Size = new Size(276, 54);
            grbDisc.TabIndex = 7;
            grbDisc.TabStop = false;
            grbDisc.Text = "İndirim";
            // 
            // nmrDisc
            // 
            nmrDisc.DecimalPlaces = 2;
            nmrDisc.Location = new Point(140, 23);
            nmrDisc.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrDisc.Name = "nmrDisc";
            nmrDisc.Size = new Size(120, 23);
            nmrDisc.TabIndex = 3;
            nmrDisc.ValueChanged += nmrDisc_ValueChanged;
            nmrDisc.KeyUp += nmrDisc_KeyUp;
            // 
            // cmbDisc
            // 
            cmbDisc.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisc.FormattingEnabled = true;
            cmbDisc.Location = new Point(13, 22);
            cmbDisc.Name = "cmbDisc";
            cmbDisc.Size = new Size(121, 23);
            cmbDisc.TabIndex = 2;
            cmbDisc.SelectedIndexChanged += cmbDisc_SelectedIndexChanged;
            // 
            // lblTotalTax
            // 
            lblTotalTax.AutoSize = true;
            lblTotalTax.Location = new Point(498, 226);
            lblTotalTax.Name = "lblTotalTax";
            lblTotalTax.Size = new Size(33, 15);
            lblTotalTax.TabIndex = 5;
            lblTotalTax.Text = "Vergi";
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
            // lblTotalPrice
            // 
            lblTotalPrice.AutoSize = true;
            lblTotalPrice.Location = new Point(6, 223);
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
            grbCustomerMsg.Controls.Add(rchMessage);
            grbCustomerMsg.Location = new Point(12, 516);
            grbCustomerMsg.Name = "grbCustomerMsg";
            grbCustomerMsg.Size = new Size(691, 162);
            grbCustomerMsg.TabIndex = 2;
            grbCustomerMsg.TabStop = false;
            grbCustomerMsg.Text = "Müşteri Bilgilendirme Metni";
            // 
            // btnWhatsapp
            // 
            btnWhatsapp.Location = new Point(373, 128);
            btnWhatsapp.Name = "btnWhatsapp";
            btnWhatsapp.Size = new Size(145, 23);
            btnWhatsapp.TabIndex = 3;
            btnWhatsapp.Text = "WhatsApp'ı Aç";
            btnWhatsapp.UseVisualStyleBackColor = true;
            btnWhatsapp.Click += btnWhatsapp_Click;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(260, 128);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(75, 23);
            btnCopy.TabIndex = 2;
            btnCopy.Text = "Kopyala";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(146, 128);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 1;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // rchMessage
            // 
            rchMessage.Location = new Point(6, 22);
            rchMessage.Name = "rchMessage";
            rchMessage.ReadOnly = true;
            rchMessage.Size = new Size(678, 100);
            rchMessage.TabIndex = 0;
            rchMessage.Text = "";
            // 
            // pbScale
            // 
            pbScale.Image = (Image)resources.GetObject("pbScale.Image");
            pbScale.Location = new Point(503, 489);
            pbScale.Name = "pbScale";
            pbScale.Size = new Size(27, 21);
            pbScale.SizeMode = PictureBoxSizeMode.StretchImage;
            pbScale.TabIndex = 4;
            pbScale.TabStop = false;
            pbScale.Click += pbScale_Click;
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
            btnAddOrder.Click += btnAddOrder_Click;
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
            // hoverTimer
            // 
            hoverTimer.Interval = 1000;
            hoverTimer.Tick += hoverTimer_Tick;
            // 
            // btnTotal
            // 
            btnTotal.Location = new Point(6, 22);
            btnTotal.Name = "btnTotal";
            btnTotal.Size = new Size(231, 30);
            btnTotal.TabIndex = 5;
            btnTotal.Text = "Toplam Tutar'ı Genişlet";
            btnTotal.UseVisualStyleBackColor = true;
            btnTotal.Click += btnTotal_Click;
            // 
            // grbTable
            // 
            grbTable.Controls.Add(btnAll);
            grbTable.Controls.Add(btnTax);
            grbTable.Controls.Add(btnDisc);
            grbTable.Controls.Add(btnTotal);
            grbTable.Location = new Point(715, 516);
            grbTable.Name = "grbTable";
            grbTable.Size = new Size(243, 162);
            grbTable.TabIndex = 6;
            grbTable.TabStop = false;
            grbTable.Text = "Tablo Kontrolleri";
            // 
            // btnAll
            // 
            btnAll.Location = new Point(6, 126);
            btnAll.Name = "btnAll";
            btnAll.Size = new Size(231, 30);
            btnAll.TabIndex = 8;
            btnAll.Text = "Bütün Tabloları Genişlet";
            btnAll.UseVisualStyleBackColor = true;
            btnAll.Click += btnAll_Click;
            // 
            // btnTax
            // 
            btnTax.Location = new Point(6, 92);
            btnTax.Name = "btnTax";
            btnTax.Size = new Size(231, 30);
            btnTax.TabIndex = 7;
            btnTax.Text = "VergiyiGenişlet";
            btnTax.UseVisualStyleBackColor = true;
            btnTax.Click += btnTax_Click;
            // 
            // btnDisc
            // 
            btnDisc.Location = new Point(6, 58);
            btnDisc.Name = "btnDisc";
            btnDisc.Size = new Size(231, 30);
            btnDisc.TabIndex = 6;
            btnDisc.Text = "Toplam İndirim'i Genişlet";
            btnDisc.UseVisualStyleBackColor = true;
            btnDisc.Click += btnDisc_Click;
            // 
            // PricingOrderScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 685);
            Controls.Add(grbTable);
            Controls.Add(pbScale);
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
            grbCargo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrCargo).EndInit();
            grbTax.ResumeLayout(false);
            grbDisc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrDisc).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).EndInit();
            grbCustomerMsg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbScale).EndInit();
            grbOrderDetails.ResumeLayout(false);
            grbOrderDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).EndInit();
            grbTable.ResumeLayout(false);
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
        private Label lblTotalTax;
        private Label lblTotalDisc;
        private GroupBox grbDisc;
        private NumericUpDown nmrDisc;
        private ComboBox cmbDisc;
        private GroupBox grbTax;
        private ComboBox cmbSCT;
        private GroupBox grbCustomerMsg;
        private Button btnCopy;
        private Button btnClear;
        private RichTextBox rchMessage;
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
        private ListView lwDisc;
        private PictureBox pbScale;
        private GroupBox grbCargo;
        private NumericUpDown nmrCargo;
        private ListView lwDiscList;
        private ListView lwTotal;
        private ListView lwTax;
        private System.Windows.Forms.Timer hoverTimer;
        private Button btnTotal;
        private GroupBox grbTable;
        private Button btnAll;
        private Button btnTax;
        private Button btnDisc;
    }
}