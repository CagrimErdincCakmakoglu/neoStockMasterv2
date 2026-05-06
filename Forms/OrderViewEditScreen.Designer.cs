namespace neoStockMasterv2.Forms
{
    partial class OrderViewEditScreen
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
            rbRead = new RadioButton();
            rbEdit = new RadioButton();
            rbDelete = new RadioButton();
            grbMethod = new GroupBox();
            grbOrders = new GroupBox();
            cmbOrders = new ComboBox();
            dgwProducts = new DataGridView();
            lwDisc = new ListView();
            lwTax = new ListView();
            lwDiscList = new ListView();
            lwTotal = new ListView();
            grbCargo = new GroupBox();
            nmrCargo = new NumericUpDown();
            grbTax = new GroupBox();
            cmbVAT = new ComboBox();
            cmbSCT = new ComboBox();
            grbDisc = new GroupBox();
            nmrDisc = new NumericUpDown();
            cmbDisc = new ComboBox();
            lblTotalPrice = new Label();
            lblTotalTax = new Label();
            lblTotalDisc = new Label();
            mskPhoneNo = new MaskedTextBox();
            txtCustomerName = new TextBox();
            txtCargoTracker = new TextBox();
            txtCargo = new TextBox();
            cmbCargo = new ComboBox();
            cmbOrderStatus = new ComboBox();
            cmbPayment = new ComboBox();
            grbPriceDetails = new GroupBox();
            chbPriceLock = new CheckBox();
            dtpOrder = new DateTimePicker();
            btnVer = new Button();
            menuStripLanguage.SuspendLayout();
            grbMethod.SuspendLayout();
            grbOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwProducts).BeginInit();
            grbCargo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrCargo).BeginInit();
            grbTax.SuspendLayout();
            grbDisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDisc).BeginInit();
            grbPriceDetails.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(705, 24);
            menuStripLanguage.TabIndex = 0;
            menuStripLanguage.Text = "menuStrip1";
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
            // rbRead
            // 
            rbRead.AutoSize = true;
            rbRead.Location = new Point(48, 22);
            rbRead.Name = "rbRead";
            rbRead.Size = new Size(78, 19);
            rbRead.TabIndex = 1;
            rbRead.TabStop = true;
            rbRead.Text = "Görüntüle";
            rbRead.UseVisualStyleBackColor = true;
            rbRead.CheckedChanged += rbRead_CheckedChanged;
            // 
            // rbEdit
            // 
            rbEdit.AutoSize = true;
            rbEdit.Location = new Point(146, 22);
            rbEdit.Name = "rbEdit";
            rbEdit.Size = new Size(67, 19);
            rbEdit.TabIndex = 2;
            rbEdit.TabStop = true;
            rbEdit.Text = "Düzenle";
            rbEdit.UseVisualStyleBackColor = true;
            rbEdit.CheckedChanged += rbEdit_CheckedChanged;
            // 
            // rbDelete
            // 
            rbDelete.AutoSize = true;
            rbDelete.Location = new Point(239, 22);
            rbDelete.Name = "rbDelete";
            rbDelete.Size = new Size(37, 19);
            rbDelete.TabIndex = 3;
            rbDelete.TabStop = true;
            rbDelete.Text = "Sil";
            rbDelete.UseVisualStyleBackColor = true;
            rbDelete.CheckedChanged += rbDelete_CheckedChanged;
            // 
            // grbMethod
            // 
            grbMethod.Controls.Add(rbEdit);
            grbMethod.Controls.Add(rbDelete);
            grbMethod.Controls.Add(rbRead);
            grbMethod.Location = new Point(361, 27);
            grbMethod.Name = "grbMethod";
            grbMethod.Size = new Size(336, 57);
            grbMethod.TabIndex = 4;
            grbMethod.TabStop = false;
            grbMethod.Text = "Yöntem";
            // 
            // grbOrders
            // 
            grbOrders.Controls.Add(cmbOrders);
            grbOrders.Location = new Point(12, 27);
            grbOrders.Name = "grbOrders";
            grbOrders.Size = new Size(343, 57);
            grbOrders.TabIndex = 5;
            grbOrders.TabStop = false;
            grbOrders.Text = "Siparişler";
            // 
            // cmbOrders
            // 
            cmbOrders.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrders.FormattingEnabled = true;
            cmbOrders.Location = new Point(6, 22);
            cmbOrders.Name = "cmbOrders";
            cmbOrders.Size = new Size(331, 23);
            cmbOrders.TabIndex = 0;
            cmbOrders.SelectedIndexChanged += cmbOrders_SelectedIndexChanged;
            // 
            // dgwProducts
            // 
            dgwProducts.AllowUserToAddRows = false;
            dgwProducts.AllowUserToDeleteRows = false;
            dgwProducts.AllowUserToResizeColumns = false;
            dgwProducts.AllowUserToResizeRows = false;
            dgwProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwProducts.Location = new Point(6, 22);
            dgwProducts.Name = "dgwProducts";
            dgwProducts.RowHeadersVisible = false;
            dgwProducts.Size = new Size(673, 160);
            dgwProducts.TabIndex = 6;
            dgwProducts.CellBeginEdit += dgwProducts_CellBeginEdit;
            dgwProducts.CellEndEdit += dgwProducts_CellEndEdit;
            dgwProducts.EditingControlShowing += dgwProducts_EditingControlShowing;
            // 
            // lwDisc
            // 
            lwDisc.Location = new Point(12, 284);
            lwDisc.MultiSelect = false;
            lwDisc.Name = "lwDisc";
            lwDisc.Size = new Size(685, 94);
            lwDisc.TabIndex = 11;
            lwDisc.UseCompatibleStateImageBehavior = false;
            // 
            // lwTax
            // 
            lwTax.Location = new Point(473, 459);
            lwTax.Name = "lwTax";
            lwTax.Size = new Size(212, 67);
            lwTax.TabIndex = 18;
            lwTax.UseCompatibleStateImageBehavior = false;
            // 
            // lwDiscList
            // 
            lwDiscList.Location = new Point(255, 459);
            lwDiscList.Name = "lwDiscList";
            lwDiscList.Size = new Size(212, 67);
            lwDiscList.TabIndex = 17;
            lwDiscList.UseCompatibleStateImageBehavior = false;
            // 
            // lwTotal
            // 
            lwTotal.Location = new Point(25, 459);
            lwTotal.Name = "lwTotal";
            lwTotal.Size = new Size(224, 67);
            lwTotal.TabIndex = 16;
            lwTotal.UseCompatibleStateImageBehavior = false;
            // 
            // grbCargo
            // 
            grbCargo.Controls.Add(nmrCargo);
            grbCargo.Location = new Point(551, 384);
            grbCargo.Name = "grbCargo";
            grbCargo.Size = new Size(140, 54);
            grbCargo.TabIndex = 21;
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
            // 
            // grbTax
            // 
            grbTax.Controls.Add(cmbVAT);
            grbTax.Controls.Add(cmbSCT);
            grbTax.Location = new Point(294, 384);
            grbTax.Name = "grbTax";
            grbTax.Size = new Size(251, 54);
            grbTax.TabIndex = 19;
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
            grbDisc.Location = new Point(12, 384);
            grbDisc.Name = "grbDisc";
            grbDisc.Size = new Size(276, 54);
            grbDisc.TabIndex = 20;
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
            // lblTotalPrice
            // 
            lblTotalPrice.AutoSize = true;
            lblTotalPrice.Location = new Point(96, 441);
            lblTotalPrice.Name = "lblTotalPrice";
            lblTotalPrice.Size = new Size(76, 15);
            lblTotalPrice.TabIndex = 22;
            lblTotalPrice.Text = "Toplam Tutar";
            // 
            // lblTotalTax
            // 
            lblTotalTax.AutoSize = true;
            lblTotalTax.Location = new Point(569, 441);
            lblTotalTax.Name = "lblTotalTax";
            lblTotalTax.Size = new Size(33, 15);
            lblTotalTax.TabIndex = 24;
            lblTotalTax.Text = "Vergi";
            // 
            // lblTotalDisc
            // 
            lblTotalDisc.AutoSize = true;
            lblTotalDisc.Location = new Point(329, 441);
            lblTotalDisc.Name = "lblTotalDisc";
            lblTotalDisc.Size = new Size(87, 15);
            lblTotalDisc.TabIndex = 23;
            lblTotalDisc.Text = "Toplam İndirim";
            // 
            // mskPhoneNo
            // 
            mskPhoneNo.Location = new Point(460, 561);
            mskPhoneNo.Mask = "(999) 000-0000";
            mskPhoneNo.Name = "mskPhoneNo";
            mskPhoneNo.Size = new Size(237, 23);
            mskPhoneNo.TabIndex = 31;
            // 
            // txtCustomerName
            // 
            txtCustomerName.Location = new Point(460, 532);
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.Size = new Size(237, 23);
            txtCustomerName.TabIndex = 30;
            txtCustomerName.Text = "Müşteri İsmini Yazınız";
            // 
            // txtCargoTracker
            // 
            txtCargoTracker.Location = new Point(142, 590);
            txtCargoTracker.Name = "txtCargoTracker";
            txtCargoTracker.Size = new Size(224, 23);
            txtCargoTracker.TabIndex = 29;
            txtCargoTracker.Text = "Kargo Takip Numarasını Yazınız";
            // 
            // txtCargo
            // 
            txtCargo.Location = new Point(230, 561);
            txtCargo.Name = "txtCargo";
            txtCargo.Size = new Size(224, 23);
            txtCargo.TabIndex = 28;
            txtCargo.Text = "Diğer Kargo Şirketinin Adını Yazınız";
            // 
            // cmbCargo
            // 
            cmbCargo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCargo.FormattingEnabled = true;
            cmbCargo.Location = new Point(230, 532);
            cmbCargo.Name = "cmbCargo";
            cmbCargo.Size = new Size(224, 23);
            cmbCargo.TabIndex = 27;
            cmbCargo.SelectedIndexChanged += cmbCargo_SelectedIndexChanged;
            // 
            // cmbOrderStatus
            // 
            cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrderStatus.FormattingEnabled = true;
            cmbOrderStatus.Location = new Point(12, 561);
            cmbOrderStatus.Name = "cmbOrderStatus";
            cmbOrderStatus.Size = new Size(212, 23);
            cmbOrderStatus.TabIndex = 26;
            cmbOrderStatus.SelectedIndexChanged += cmbOrderStatus_SelectedIndexChanged;
            // 
            // cmbPayment
            // 
            cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayment.FormattingEnabled = true;
            cmbPayment.Location = new Point(12, 532);
            cmbPayment.Name = "cmbPayment";
            cmbPayment.Size = new Size(212, 23);
            cmbPayment.TabIndex = 25;
            cmbPayment.SelectedIndexChanged += cmbPayment_SelectedIndexChanged;
            // 
            // grbPriceDetails
            // 
            grbPriceDetails.Controls.Add(chbPriceLock);
            grbPriceDetails.Controls.Add(dgwProducts);
            grbPriceDetails.Location = new Point(12, 90);
            grbPriceDetails.Name = "grbPriceDetails";
            grbPriceDetails.Size = new Size(685, 188);
            grbPriceDetails.TabIndex = 34;
            grbPriceDetails.TabStop = false;
            grbPriceDetails.Text = "Fiyat Detayları";
            // 
            // chbPriceLock
            // 
            chbPriceLock.AutoSize = true;
            chbPriceLock.Location = new Point(168, 0);
            chbPriceLock.Name = "chbPriceLock";
            chbPriceLock.Size = new Size(80, 19);
            chbPriceLock.TabIndex = 36;
            chbPriceLock.Text = "Fiyat Kilidi";
            chbPriceLock.UseVisualStyleBackColor = true;
            // 
            // dtpOrder
            // 
            dtpOrder.Location = new Point(379, 590);
            dtpOrder.Name = "dtpOrder";
            dtpOrder.Size = new Size(200, 23);
            dtpOrder.TabIndex = 7;
            // 
            // btnVer
            // 
            btnVer.Location = new Point(280, 632);
            btnVer.Name = "btnVer";
            btnVer.Size = new Size(174, 41);
            btnVer.TabIndex = 35;
            btnVer.Text = "btnVer";
            btnVer.UseVisualStyleBackColor = true;
            btnVer.Click += btnVer_Click;
            // 
            // OrderViewEditScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(705, 686);
            Controls.Add(btnVer);
            Controls.Add(dtpOrder);
            Controls.Add(grbPriceDetails);
            Controls.Add(mskPhoneNo);
            Controls.Add(txtCustomerName);
            Controls.Add(txtCargoTracker);
            Controls.Add(txtCargo);
            Controls.Add(cmbCargo);
            Controls.Add(cmbOrderStatus);
            Controls.Add(cmbPayment);
            Controls.Add(lblTotalTax);
            Controls.Add(lblTotalDisc);
            Controls.Add(lblTotalPrice);
            Controls.Add(grbCargo);
            Controls.Add(grbTax);
            Controls.Add(grbDisc);
            Controls.Add(lwTax);
            Controls.Add(lwDiscList);
            Controls.Add(lwTotal);
            Controls.Add(lwDisc);
            Controls.Add(grbOrders);
            Controls.Add(grbMethod);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "OrderViewEditScreen";
            Text = "Siparişleri Görüntüle - Düzenle";
            FormClosed += OrderViewEditScreen_FormClosed;
            Load += OrderViewEditScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbMethod.ResumeLayout(false);
            grbMethod.PerformLayout();
            grbOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwProducts).EndInit();
            grbCargo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrCargo).EndInit();
            grbTax.ResumeLayout(false);
            grbDisc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrDisc).EndInit();
            grbPriceDetails.ResumeLayout(false);
            grbPriceDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private RadioButton rbRead;
        private RadioButton rbEdit;
        private RadioButton rbDelete;
        private GroupBox grbMethod;
        private GroupBox grbOrders;
        private ComboBox cmbOrders;
        private DataGridView dgwProducts;
        private ListView lwDisc;
        private ListView lwTax;
        private ListView lwDiscList;
        private ListView lwTotal;
        private GroupBox grbCargo;
        private NumericUpDown nmrCargo;
        private GroupBox grbTax;
        private ComboBox cmbVAT;
        private ComboBox cmbSCT;
        private GroupBox grbDisc;
        private NumericUpDown nmrDisc;
        private ComboBox cmbDisc;
        private Label lblTotalPrice;
        private Label lblTotalTax;
        private Label lblTotalDisc;
        private MaskedTextBox mskPhoneNo;
        private TextBox txtCustomerName;
        private TextBox txtCargoTracker;
        private TextBox txtCargo;
        private ComboBox cmbCargo;
        private ComboBox cmbOrderStatus;
        private ComboBox cmbPayment;
        private GroupBox grbPriceDetails;
        private DateTimePicker dtpOrder;
        private Button btnVer;
        private CheckBox chbPriceLock;
    }
}