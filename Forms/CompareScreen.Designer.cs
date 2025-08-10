namespace neoStockMasterv2.Forms
{
    partial class CompareScreen
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
            grbRow = new GroupBox();
            nmrRow = new NumericUpDown();
            lblRow = new Label();
            btnApply = new Button();
            btnClear = new Button();
            grbTable = new GroupBox();
            dgwTable = new DataGridView();
            grbMethod = new GroupBox();
            menuStripLanguage.SuspendLayout();
            grbRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrRow).BeginInit();
            grbTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwTable).BeginInit();
            grbMethod.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(702, 24);
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
            // grbRow
            // 
            grbRow.Controls.Add(nmrRow);
            grbRow.Controls.Add(lblRow);
            grbRow.Location = new Point(156, 38);
            grbRow.Name = "grbRow";
            grbRow.Size = new Size(248, 64);
            grbRow.TabIndex = 1;
            grbRow.TabStop = false;
            grbRow.Text = "Satır Sayısı Belirle";
            // 
            // nmrRow
            // 
            nmrRow.Location = new Point(89, 26);
            nmrRow.Maximum = new decimal(new int[] { -1981284353, -1966660860, 0, 0 });
            nmrRow.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nmrRow.Name = "nmrRow";
            nmrRow.Size = new Size(153, 23);
            nmrRow.TabIndex = 1;
            nmrRow.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nmrRow.ValueChanged += nmrRow_ValueChanged;
            // 
            // lblRow
            // 
            lblRow.AutoSize = true;
            lblRow.Location = new Point(6, 28);
            lblRow.Name = "lblRow";
            lblRow.Size = new Size(62, 15);
            lblRow.TabIndex = 0;
            lblRow.Text = "Satır Sayısı";
            // 
            // btnApply
            // 
            btnApply.Location = new Point(15, 28);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 2;
            btnApply.Text = "Uygula";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(106, 28);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 3;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // grbTable
            // 
            grbTable.Controls.Add(dgwTable);
            grbTable.Location = new Point(12, 108);
            grbTable.Name = "grbTable";
            grbTable.Size = new Size(684, 228);
            grbTable.TabIndex = 4;
            grbTable.TabStop = false;
            grbTable.Text = "Tablo";
            // 
            // dgwTable
            // 
            dgwTable.AllowUserToAddRows = false;
            dgwTable.AllowUserToDeleteRows = false;
            dgwTable.AllowUserToResizeColumns = false;
            dgwTable.AllowUserToResizeRows = false;
            dgwTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwTable.Location = new Point(6, 22);
            dgwTable.Name = "dgwTable";
            dgwTable.RowHeadersVisible = false;
            dgwTable.Size = new Size(672, 199);
            dgwTable.TabIndex = 0;
            // 
            // grbMethod
            // 
            grbMethod.Controls.Add(btnClear);
            grbMethod.Controls.Add(btnApply);
            grbMethod.Location = new Point(410, 38);
            grbMethod.Name = "grbMethod";
            grbMethod.Size = new Size(193, 64);
            grbMethod.TabIndex = 2;
            grbMethod.TabStop = false;
            grbMethod.Text = "Eylem";
            // 
            // CompareScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(702, 342);
            Controls.Add(grbMethod);
            Controls.Add(grbTable);
            Controls.Add(grbRow);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            Name = "CompareScreen";
            Text = "CompareScreen";
            Load += CompareScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbRow.ResumeLayout(false);
            grbRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrRow).EndInit();
            grbTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwTable).EndInit();
            grbMethod.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbRow;
        private Label lblRow;
        private NumericUpDown nmrRow;
        private Button btnApply;
        private Button btnClear;
        private GroupBox grbTable;
        private DataGridView dgwTable;
        private GroupBox grbMethod;
    }
}