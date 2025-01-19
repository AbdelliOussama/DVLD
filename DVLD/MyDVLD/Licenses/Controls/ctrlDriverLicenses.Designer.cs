namespace MyDVLD.Licenses.Controls
{
    partial class ctrlDriverLicenses
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbDriverLicenses = new System.Windows.Forms.GroupBox();
            this.tcDriverLicenses = new System.Windows.Forms.TabControl();
            this.tpLocalLicenses = new System.Windows.Forms.TabPage();
            this.dgvLocalLicenses = new System.Windows.Forms.DataGridView();
            this.lblLocalLicensesRecords = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tpInternationallicenses = new System.Windows.Forms.TabPage();
            this.dgvInternationalLicenses = new System.Windows.Forms.DataGridView();
            this.lblInternationalLicensesRecordsCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmsLocalLicenseHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLocalLiceseInfoToolStripMenuItem_Click = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsInternationalLicenseHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showInternationalLicenseInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDriverLicenses.SuspendLayout();
            this.tcDriverLicenses.SuspendLayout();
            this.tpLocalLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicenses)).BeginInit();
            this.tpInternationallicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalLicenses)).BeginInit();
            this.cmsLocalLicenseHistory.SuspendLayout();
            this.cmsInternationalLicenseHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDriverLicenses
            // 
            this.gbDriverLicenses.Controls.Add(this.tcDriverLicenses);
            this.gbDriverLicenses.Location = new System.Drawing.Point(3, 3);
            this.gbDriverLicenses.Name = "gbDriverLicenses";
            this.gbDriverLicenses.Size = new System.Drawing.Size(890, 304);
            this.gbDriverLicenses.TabIndex = 0;
            this.gbDriverLicenses.TabStop = false;
            this.gbDriverLicenses.Text = "Driver Licenses";
            // 
            // tcDriverLicenses
            // 
            this.tcDriverLicenses.Controls.Add(this.tpLocalLicenses);
            this.tcDriverLicenses.Controls.Add(this.tpInternationallicenses);
            this.tcDriverLicenses.Location = new System.Drawing.Point(6, 29);
            this.tcDriverLicenses.Name = "tcDriverLicenses";
            this.tcDriverLicenses.SelectedIndex = 0;
            this.tcDriverLicenses.Size = new System.Drawing.Size(878, 269);
            this.tcDriverLicenses.TabIndex = 0;
            // 
            // tpLocalLicenses
            // 
            this.tpLocalLicenses.BackColor = System.Drawing.Color.LightPink;
            this.tpLocalLicenses.Controls.Add(this.dgvLocalLicenses);
            this.tpLocalLicenses.Controls.Add(this.lblLocalLicensesRecords);
            this.tpLocalLicenses.Controls.Add(this.label2);
            this.tpLocalLicenses.Location = new System.Drawing.Point(4, 34);
            this.tpLocalLicenses.Name = "tpLocalLicenses";
            this.tpLocalLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tpLocalLicenses.Size = new System.Drawing.Size(870, 231);
            this.tpLocalLicenses.TabIndex = 0;
            this.tpLocalLicenses.Text = "Local";
            // 
            // dgvLocalLicenses
            // 
            this.dgvLocalLicenses.BackgroundColor = System.Drawing.Color.LightPink;
            this.dgvLocalLicenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocalLicenses.Location = new System.Drawing.Point(3, 6);
            this.dgvLocalLicenses.Name = "dgvLocalLicenses";
            this.dgvLocalLicenses.RowHeadersWidth = 51;
            this.dgvLocalLicenses.RowTemplate.Height = 24;
            this.dgvLocalLicenses.Size = new System.Drawing.Size(864, 194);
            this.dgvLocalLicenses.TabIndex = 139;
            // 
            // lblLocalLicensesRecords
            // 
            this.lblLocalLicensesRecords.AutoSize = true;
            this.lblLocalLicensesRecords.Location = new System.Drawing.Point(128, 203);
            this.lblLocalLicensesRecords.Name = "lblLocalLicensesRecords";
            this.lblLocalLicensesRecords.Size = new System.Drawing.Size(34, 25);
            this.lblLocalLicensesRecords.TabIndex = 138;
            this.lblLocalLicensesRecords.Text = "??";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 25);
            this.label2.TabIndex = 137;
            this.label2.Text = "# Records:";
            // 
            // tpInternationallicenses
            // 
            this.tpInternationallicenses.BackColor = System.Drawing.Color.LightPink;
            this.tpInternationallicenses.Controls.Add(this.dgvInternationalLicenses);
            this.tpInternationallicenses.Controls.Add(this.lblInternationalLicensesRecordsCount);
            this.tpInternationallicenses.Controls.Add(this.label3);
            this.tpInternationallicenses.Location = new System.Drawing.Point(4, 34);
            this.tpInternationallicenses.Name = "tpInternationallicenses";
            this.tpInternationallicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tpInternationallicenses.Size = new System.Drawing.Size(870, 231);
            this.tpInternationallicenses.TabIndex = 1;
            this.tpInternationallicenses.Text = "International";
            // 
            // dgvInternationalLicenses
            // 
            this.dgvInternationalLicenses.BackgroundColor = System.Drawing.Color.LightPink;
            this.dgvInternationalLicenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInternationalLicenses.Location = new System.Drawing.Point(3, 6);
            this.dgvInternationalLicenses.Name = "dgvInternationalLicenses";
            this.dgvInternationalLicenses.RowHeadersWidth = 51;
            this.dgvInternationalLicenses.RowTemplate.Height = 24;
            this.dgvInternationalLicenses.Size = new System.Drawing.Size(864, 194);
            this.dgvInternationalLicenses.TabIndex = 140;
            // 
            // lblInternationalLicensesRecordsCount
            // 
            this.lblInternationalLicensesRecordsCount.AutoSize = true;
            this.lblInternationalLicensesRecordsCount.Location = new System.Drawing.Point(128, 203);
            this.lblInternationalLicensesRecordsCount.Name = "lblInternationalLicensesRecordsCount";
            this.lblInternationalLicensesRecordsCount.Size = new System.Drawing.Size(34, 25);
            this.lblInternationalLicensesRecordsCount.TabIndex = 138;
            this.lblInternationalLicensesRecordsCount.Text = "??";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 25);
            this.label3.TabIndex = 137;
            this.label3.Text = "# Records:";
            // 
            // cmsLocalLicenseHistory
            // 
            this.cmsLocalLicenseHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsLocalLicenseHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLocalLiceseInfoToolStripMenuItem_Click});
            this.cmsLocalLicenseHistory.Name = "contextMenuStrip1";
            this.cmsLocalLicenseHistory.Size = new System.Drawing.Size(227, 70);
            // 
            // showLocalLiceseInfoToolStripMenuItem_Click
            // 
            this.showLocalLiceseInfoToolStripMenuItem_Click.Image = global::MyDVLD.Properties.Resources.License_View_32;
            this.showLocalLiceseInfoToolStripMenuItem_Click.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLocalLiceseInfoToolStripMenuItem_Click.Name = "showLocalLiceseInfoToolStripMenuItem_Click";
            this.showLocalLiceseInfoToolStripMenuItem_Click.Size = new System.Drawing.Size(226, 38);
            this.showLocalLiceseInfoToolStripMenuItem_Click.Text = "Show License Info";
            this.showLocalLiceseInfoToolStripMenuItem_Click.Click += new System.EventHandler(this.showLocalLiceseInfoToolStripMenuItem_Click_Click);
            // 
            // cmsInternationalLicenseHistory
            // 
            this.cmsInternationalLicenseHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsInternationalLicenseHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInternationalLicenseInfoToolStripMenuItem});
            this.cmsInternationalLicenseHistory.Name = "contextMenuStrip2";
            this.cmsInternationalLicenseHistory.Size = new System.Drawing.Size(213, 42);
            // 
            // showInternationalLicenseInfoToolStripMenuItem
            // 
            this.showInternationalLicenseInfoToolStripMenuItem.Image = global::MyDVLD.Properties.Resources.License_View_32;
            this.showInternationalLicenseInfoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showInternationalLicenseInfoToolStripMenuItem.Name = "showInternationalLicenseInfoToolStripMenuItem";
            this.showInternationalLicenseInfoToolStripMenuItem.Size = new System.Drawing.Size(212, 38);
            this.showInternationalLicenseInfoToolStripMenuItem.Text = "Show License Info";
            this.showInternationalLicenseInfoToolStripMenuItem.Click += new System.EventHandler(this.showInternationalLicenseInfoToolStripMenuItem_Click);
            // 
            // ctrlDriverLicenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightPink;
            this.Controls.Add(this.gbDriverLicenses);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ctrlDriverLicenses";
            this.Size = new System.Drawing.Size(896, 317);
            this.gbDriverLicenses.ResumeLayout(false);
            this.tcDriverLicenses.ResumeLayout(false);
            this.tpLocalLicenses.ResumeLayout(false);
            this.tpLocalLicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicenses)).EndInit();
            this.tpInternationallicenses.ResumeLayout(false);
            this.tpInternationallicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalLicenses)).EndInit();
            this.cmsLocalLicenseHistory.ResumeLayout(false);
            this.cmsInternationalLicenseHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDriverLicenses;
        private System.Windows.Forms.TabControl tcDriverLicenses;
        private System.Windows.Forms.TabPage tpLocalLicenses;
        private System.Windows.Forms.TabPage tpInternationallicenses;
        private System.Windows.Forms.DataGridView dgvLocalLicenses;
        private System.Windows.Forms.Label lblLocalLicensesRecords;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInternationalLicensesRecordsCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvInternationalLicenses;
        private System.Windows.Forms.ContextMenuStrip cmsLocalLicenseHistory;
        private System.Windows.Forms.ToolStripMenuItem showLocalLiceseInfoToolStripMenuItem_Click;
        private System.Windows.Forms.ContextMenuStrip cmsInternationalLicenseHistory;
        private System.Windows.Forms.ToolStripMenuItem showInternationalLicenseInfoToolStripMenuItem;
    }
}
