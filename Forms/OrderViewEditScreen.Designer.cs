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
            menuStrip1 = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            grbMethod = new GroupBox();
            grbOrders = new GroupBox();
            cmbOrders = new ComboBox();
            dgwOrderDetails = new DataGridView();
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
            menuStrip1.SuspendLayout();
            grbMethod.SuspendLayout();
            grbOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).BeginInit();
            grbCargo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrCargo).BeginInit();
            grbTax.SuspendLayout();
            grbDisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrDisc).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.MenuBar;
            menuStrip1.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(704, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // türkçeToolStripMenuItem
            // 
            türkçeToolStripMenuItem.Image = Languages.Turkish.TurkFlag;
            türkçeToolStripMenuItem.Name = "türkçeToolStripMenuItem";
            türkçeToolStripMenuItem.Size = new Size(70, 20);
            türkçeToolStripMenuItem.Text = "Türkçe";
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.Image = Languages.English.EngFlag;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(73, 20);
            englishToolStripMenuItem.Text = "English";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(69, 22);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(78, 19);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "Görüntüle";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(196, 22);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(67, 19);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "Düzenle";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(312, 22);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(37, 19);
            radioButton3.TabIndex = 3;
            radioButton3.TabStop = true;
            radioButton3.Text = "Sil";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // grbMethod
            // 
            grbMethod.Controls.Add(radioButton1);
            grbMethod.Controls.Add(radioButton3);
            grbMethod.Controls.Add(radioButton2);
            grbMethod.Location = new Point(152, 90);
            grbMethod.Name = "grbMethod";
            grbMethod.Size = new Size(421, 54);
            grbMethod.TabIndex = 4;
            grbMethod.TabStop = false;
            grbMethod.Text = "Yöntem";
            // 
            // grbOrders
            // 
            grbOrders.Controls.Add(cmbOrders);
            grbOrders.Location = new Point(152, 27);
            grbOrders.Name = "grbOrders";
            grbOrders.Size = new Size(421, 57);
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
            cmbOrders.Size = new Size(409, 23);
            cmbOrders.TabIndex = 0;
            // 
            // dgwOrderDetails
            // 
            dgwOrderDetails.AllowUserToAddRows = false;
            dgwOrderDetails.AllowUserToDeleteRows = false;
            dgwOrderDetails.AllowUserToResizeColumns = false;
            dgwOrderDetails.AllowUserToResizeRows = false;
            dgwOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwOrderDetails.Location = new Point(12, 150);
            dgwOrderDetails.Name = "dgwOrderDetails";
            dgwOrderDetails.RowHeadersVisible = false;
            dgwOrderDetails.Size = new Size(679, 188);
            dgwOrderDetails.TabIndex = 6;
            // 
            // lwDisc
            // 
            lwDisc.Location = new Point(12, 344);
            lwDisc.MultiSelect = false;
            lwDisc.Name = "lwDisc";
            lwDisc.Size = new Size(679, 94);
            lwDisc.TabIndex = 11;
            lwDisc.UseCompatibleStateImageBehavior = false;
            // 
            // lwTax
            // 
            lwTax.Location = new Point(431, 528);
            lwTax.Name = "lwTax";
            lwTax.Size = new Size(154, 67);
            lwTax.TabIndex = 18;
            lwTax.UseCompatibleStateImageBehavior = false;
            // 
            // lwDiscList
            // 
            lwDiscList.Location = new Point(271, 528);
            lwDiscList.Name = "lwDiscList";
            lwDiscList.Size = new Size(154, 67);
            lwDiscList.TabIndex = 17;
            lwDiscList.UseCompatibleStateImageBehavior = false;
            // 
            // lwTotal
            // 
            lwTotal.Location = new Point(111, 528);
            lwTotal.Name = "lwTotal";
            lwTotal.Size = new Size(154, 67);
            lwTotal.TabIndex = 16;
            lwTotal.UseCompatibleStateImageBehavior = false;
            // 
            // grbCargo
            // 
            grbCargo.Controls.Add(nmrCargo);
            grbCargo.Location = new Point(551, 444);
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
            // 
            // grbTax
            // 
            grbTax.Controls.Add(cmbVAT);
            grbTax.Controls.Add(cmbSCT);
            grbTax.Location = new Point(294, 444);
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
            // 
            // cmbSCT
            // 
            cmbSCT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSCT.FormattingEnabled = true;
            cmbSCT.Location = new Point(6, 21);
            cmbSCT.Name = "cmbSCT";
            cmbSCT.Size = new Size(116, 23);
            cmbSCT.TabIndex = 0;
            // 
            // grbDisc
            // 
            grbDisc.Controls.Add(nmrDisc);
            grbDisc.Controls.Add(cmbDisc);
            grbDisc.Location = new Point(12, 444);
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
            // lblTotalPrice
            // 
            lblTotalPrice.AutoSize = true;
            lblTotalPrice.Location = new Point(158, 510);
            lblTotalPrice.Name = "lblTotalPrice";
            lblTotalPrice.Size = new Size(76, 15);
            lblTotalPrice.TabIndex = 22;
            lblTotalPrice.Text = "Toplam Tutar";
            // 
            // lblTotalTax
            // 
            lblTotalTax.AutoSize = true;
            lblTotalTax.Location = new Point(483, 510);
            lblTotalTax.Name = "lblTotalTax";
            lblTotalTax.Size = new Size(33, 15);
            lblTotalTax.TabIndex = 24;
            lblTotalTax.Text = "Vergi";
            // 
            // lblTotalDisc
            // 
            lblTotalDisc.AutoSize = true;
            lblTotalDisc.Location = new Point(294, 510);
            lblTotalDisc.Name = "lblTotalDisc";
            lblTotalDisc.Size = new Size(87, 15);
            lblTotalDisc.TabIndex = 23;
            lblTotalDisc.Text = "Toplam İndirim";
            // 
            // mskPhoneNo
            // 
            mskPhoneNo.Location = new Point(460, 630);
            mskPhoneNo.Mask = "(999) 000-0000";
            mskPhoneNo.Name = "mskPhoneNo";
            mskPhoneNo.Size = new Size(237, 23);
            mskPhoneNo.TabIndex = 31;
            // 
            // txtCustomerName
            // 
            txtCustomerName.Location = new Point(460, 601);
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.Size = new Size(237, 23);
            txtCustomerName.TabIndex = 30;
            txtCustomerName.Text = "Müşteri İsmini Yazınız";
            // 
            // txtCargoTracker
            // 
            txtCargoTracker.Location = new Point(230, 659);
            txtCargoTracker.Name = "txtCargoTracker";
            txtCargoTracker.Size = new Size(224, 23);
            txtCargoTracker.TabIndex = 29;
            txtCargoTracker.Text = "Kargo Takip Numarasını Yazınız";
            // 
            // txtCargo
            // 
            txtCargo.Location = new Point(230, 630);
            txtCargo.Name = "txtCargo";
            txtCargo.Size = new Size(224, 23);
            txtCargo.TabIndex = 28;
            txtCargo.Text = "Diğer Kargo Şirketinin Adını Yazınız";
            // 
            // cmbCargo
            // 
            cmbCargo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCargo.FormattingEnabled = true;
            cmbCargo.Location = new Point(230, 601);
            cmbCargo.Name = "cmbCargo";
            cmbCargo.Size = new Size(224, 23);
            cmbCargo.TabIndex = 27;
            // 
            // cmbOrderStatus
            // 
            cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrderStatus.FormattingEnabled = true;
            cmbOrderStatus.Location = new Point(12, 630);
            cmbOrderStatus.Name = "cmbOrderStatus";
            cmbOrderStatus.Size = new Size(212, 23);
            cmbOrderStatus.TabIndex = 26;
            // 
            // cmbPayment
            // 
            cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayment.FormattingEnabled = true;
            cmbPayment.Location = new Point(12, 601);
            cmbPayment.Name = "cmbPayment";
            cmbPayment.Size = new Size(212, 23);
            cmbPayment.TabIndex = 25;
            // 
            // OrderViewEditScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 850);
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
            Controls.Add(dgwOrderDetails);
            Controls.Add(grbOrders);
            Controls.Add(grbMethod);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Name = "OrderViewEditScreen";
            Text = "Siparişleri Görüntüle - Düzenle";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            grbMethod.ResumeLayout(false);
            grbMethod.PerformLayout();
            grbOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwOrderDetails).EndInit();
            grbCargo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrCargo).EndInit();
            grbTax.ResumeLayout(false);
            grbDisc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrDisc).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private GroupBox grbMethod;
        private GroupBox grbOrders;
        private ComboBox cmbOrders;
        private DataGridView dgwOrderDetails;
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
    }
}