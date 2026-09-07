using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class SettingsControl : UserControl
    {
        private Dictionary<string, string> SettingList;

        public SettingsControl(Dictionary<string, string> settingList)
        {
            InitializeComponent();
            SettingList = settingList;
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Заполнение комбобокса
            cmbRetention.Items.Clear();
            cmbRetention.Items.Add("1 месяц");
            cmbRetention.Items.Add("3 месяца");
            cmbRetention.Items.Add("1 год");

            int current = UserSettings.RetentionMonths;
            if (current == 1) cmbRetention.SelectedIndex = 0;
            else if (current == 3) cmbRetention.SelectedIndex = 1;
            else if (current == 12) cmbRetention.SelectedIndex = 2;
            else cmbRetention.SelectedIndex = 0;

            // Установка дат
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;

            // Показ панели экспорта
            ShowExportPanel(cmbRetention.SelectedIndex);

            // Тексты
            lblRetention.Text = "Срок хранения истории:";
            lblPeriod.Text = "Период для экспорта:";
            btnExport.Text = "Экспортировать в .txt";

            // Подписка на событие изменения
            cmbRetention.SelectedIndexChanged += cmbRetention_SelectedIndexChanged;
        }

        private void cmbRetention_SelectedIndexChanged(object sender, EventArgs e)
        {
            int months = 1;
            switch (cmbRetention.SelectedIndex)
            {
                case 0: months = 1; break;
                case 1: months = 3; break;
                case 2: months = 12; break;
            }
            UserSettings.RetentionMonths = months;
            UserSettings.Save();
            HistoryManager.SetRetentionMonths(months);

            ShowExportPanel(cmbRetention.SelectedIndex);
        }

        private void ShowExportPanel(int index)
        {
            bool visible = (index == 1 || index == 2);
            pnlExport.Visible = visible;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

                var entries = HistoryManager.GetEntriesByDateRange(from, to);
                if (entries.Count == 0)
                {
                    MessageBox.Show("За выбранный период нет записей.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Текстовые файлы (*.txt)|*.txt";
                sfd.DefaultExt = "txt";
                sfd.FileName = $"History_{from:yyyyMMdd}_{to:yyyyMMdd}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("Дата и время\tОтправитель\tПолучатель\tСообщение");
                        foreach (var entry in entries)
                        {
                            string direction = entry.IsIncoming ? "Входящее" : "Исходящее";
                            sw.WriteLine($"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}\t{entry.Sender}\t{entry.Recipient}\t{entry.Message}");
                        }
                    }
                    MessageBox.Show("Экспорт выполнен успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}