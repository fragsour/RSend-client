namespace Client
{
    partial class HistoryForm
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
            this.listViewHistory = new System.Windows.Forms.ListView();
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDirection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSender = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRecipient = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblContact = new System.Windows.Forms.Label();
            this.cmbContacts = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewHistory
            // 
            this.listViewHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colDirection,
            this.colSender,
            this.colRecipient,
            this.colMessage});
            this.listViewHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewHistory.FullRowSelect = true;
            this.listViewHistory.Location = new System.Drawing.Point(0, 40);
            this.listViewHistory.Name = "listViewHistory";
            this.listViewHistory.Size = new System.Drawing.Size(800, 410);
            this.listViewHistory.TabIndex = 0;
            this.listViewHistory.UseCompatibleStateImageBehavior = false;
            this.listViewHistory.View = System.Windows.Forms.View.Details;
            // 
            // colDate
            // 
            this.colDate.Text = "Дата/время";
            this.colDate.Width = 140;
            // 
            // colDirection
            // 
            this.colDirection.Text = "Направление";
            this.colDirection.Width = 80;
            // 
            // colSender
            // 
            this.colSender.Text = "Отправитель";
            this.colSender.Width = 120;
            // 
            // colRecipient
            // 
            this.colRecipient.Text = "Получатель";
            this.colRecipient.Width = 120;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Сообщение";
            this.colMessage.Width = 300;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblSearch);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.lblContact);
            this.panelTop.Controls.Add(this.cmbContacts);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 40);
            this.panelTop.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(350, 12);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 13);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "Поиск:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(398, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 20);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(554, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Найти";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Location = new System.Drawing.Point(12, 12);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(52, 13);
            this.lblContact.TabIndex = 2;
            this.lblContact.Text = "Контакт:";
            // 
            // cmbContacts
            // 
            this.cmbContacts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContacts.Location = new System.Drawing.Point(70, 9);
            this.cmbContacts.Name = "cmbContacts";
            this.cmbContacts.Size = new System.Drawing.Size(200, 21);
            this.cmbContacts.TabIndex = 1;
            this.cmbContacts.SelectedIndexChanged += new System.EventHandler(this.cmbContacts_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(710, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewHistory);
            this.Controls.Add(this.panelTop);
            this.Name = "HistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "История переписки";
            this.Load += new System.EventHandler(this.HistoryForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView listViewHistory;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colDirection;
        private System.Windows.Forms.ColumnHeader colSender;
        private System.Windows.Forms.ColumnHeader colRecipient;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ComboBox cmbContacts;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
    }
}