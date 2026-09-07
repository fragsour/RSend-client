namespace Client
{
    partial class SettingsControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbRetention = new System.Windows.Forms.ComboBox();
            this.lblRetention = new System.Windows.Forms.Label();
            this.pnlExport = new System.Windows.Forms.Panel();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.pnlExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbRetention
            // 
            this.cmbRetention.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRetention.Location = new System.Drawing.Point(160, 12);
            this.cmbRetention.Name = "cmbRetention";
            this.cmbRetention.Size = new System.Drawing.Size(150, 21);
            this.cmbRetention.TabIndex = 0;
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(12, 15);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(154, 13);
            this.lblRetention.TabIndex = 3;
            this.lblRetention.Text = "Срок хранения истории (мес):";
            // 
            // pnlExport
            // 
            this.pnlExport.Controls.Add(this.dtpTo);
            this.pnlExport.Controls.Add(this.dtpFrom);
            this.pnlExport.Controls.Add(this.btnExport);
            this.pnlExport.Controls.Add(this.lblPeriod);
            this.pnlExport.Location = new System.Drawing.Point(12, 45);
            this.pnlExport.Name = "pnlExport";
            this.pnlExport.Size = new System.Drawing.Size(310, 100);
            this.pnlExport.TabIndex = 4;
            this.pnlExport.Visible = false;
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(160, 10);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(130, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(20, 10);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(130, 20);
            this.dtpFrom.TabIndex = 2;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(100, 50);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(130, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Экспортировать";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(17, 0);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(94, 13);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Период экспорта:";
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlExport);
            this.Controls.Add(this.lblRetention);
            this.Controls.Add(this.cmbRetention);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.pnlExport.ResumeLayout(false);
            this.pnlExport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbRetention;
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.Panel pnlExport;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblPeriod;
    }
}