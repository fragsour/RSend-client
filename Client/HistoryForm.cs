using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client
{
    public partial class HistoryForm : Form
    {
        private List<HistoryEntry> _currentEntries;

        public HistoryForm()
        {
            InitializeComponent();
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            this.Text = "История переписки";
            btnRefresh.Text = "Обновить";
            btnSearch.Text = "Найти";
            lblContact.Text = "Контакт:";
            lblSearch.Text = "Поиск:";

            // Заполнение списка контактов
            LoadContacts();
            // Загрузка всех записей
            LoadHistory(null);
        }

        private void LoadContacts()
        {
            cmbContacts.Items.Clear();
            cmbContacts.Items.Add("Все контакты");
            var contacts = HistoryManager.GetContacts();
            foreach (var contact in contacts)
                cmbContacts.Items.Add(contact);
            cmbContacts.SelectedIndex = 0;
        }

        private void LoadHistory(string contact)
        {
            if (contact == "Все контакты" || string.IsNullOrEmpty(contact))
                _currentEntries = HistoryManager.GetEntries();
            else
                _currentEntries = HistoryManager.GetEntriesForContact(contact);

            ApplySearch();
        }

        private void ApplySearch()
        {
            string searchText = txtSearch.Text.Trim();
            var filtered = _currentEntries;

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.FindAll(e =>
                    e.Message.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            listViewHistory.Items.Clear();
            foreach (var entry in filtered)
            {
                ListViewItem item = new ListViewItem(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(entry.IsIncoming ? "Входящее" : "Исходящее");
                item.SubItems.Add(entry.Sender);
                item.SubItems.Add(entry.Recipient ?? "");
                item.SubItems.Add(entry.Message);
                listViewHistory.Items.Add(item);
            }

            // Автоматическая подгонка ширины колонок
            foreach (ColumnHeader col in listViewHistory.Columns)
            {
                col.Width = -2;
            }
        }

        private void cmbContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistory(cmbContacts.SelectedItem?.ToString());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplySearch();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ApplySearch();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadContacts();
            LoadHistory(cmbContacts.SelectedItem?.ToString());
        }
    }
}