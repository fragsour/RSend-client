using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Client
{
    public static class HistoryManager
    {
        private static List<HistoryEntry> _entries;
        private static string _filePath;
        private static int _retentionMonths = 1;

        public static event EventHandler OnHistoryChanged;

        public static void Initialize(string appDataFolder)
        {
            Directory.CreateDirectory(appDataFolder);
            _filePath = Path.Combine(appDataFolder, "history.json");
            Load();
        }

        private static void Load()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    var serializer = new JavaScriptSerializer();
                    _entries = serializer.Deserialize<List<HistoryEntry>>(json) ?? new List<HistoryEntry>();
                }
                catch
                {
                    _entries = new List<HistoryEntry>();
                }
            }
            else
            {
                _entries = new List<HistoryEntry>();
            }
            CleanOldEntries();
            Save();
        }

        public static void Save()
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(_entries);
                File.WriteAllText(_filePath, json);
            }
            catch { }
        }

        public static void AddEntry(HistoryEntry entry)
        {
            if (entry == null) return;
            // Защита от null (без ??=, так как C# 7.3 не поддерживает)
            if (entry.Sender == null) entry.Sender = string.Empty;
            if (entry.Recipient == null) entry.Recipient = string.Empty;
            _entries.Add(entry);
            CleanOldEntries();
            Save();
            OnHistoryChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void CleanOldEntries()
        {
            if (_retentionMonths <= 0) return;
            DateTime cutoff = DateTime.Now.AddMonths(-_retentionMonths);
            int removed = _entries.RemoveAll(e => e.Timestamp < cutoff);
            if (removed > 0) Save();
        }

        public static void SetRetentionMonths(int months)
        {
            if (months < 1) months = 1;
            _retentionMonths = months;
            CleanOldEntries();
            Save();
        }

        public static List<HistoryEntry> GetEntries()
        {
            return _entries.OrderByDescending(e => e.Timestamp).ToList();
        }

        public static bool HasEntries => _entries.Count > 0;

        public static List<HistoryEntry> GetEntriesByDateRange(DateTime from, DateTime to)
        {
            return _entries
                .Where(e => e.Timestamp >= from && e.Timestamp <= to)
                .OrderBy(e => e.Timestamp)
                .ToList();
        }

        public static List<string> GetContacts()
        {
            var contacts = new HashSet<string>();
            foreach (var entry in _entries)
            {
                if (!string.IsNullOrEmpty(entry.Sender))
                    contacts.Add(entry.Sender);
                if (!string.IsNullOrEmpty(entry.Recipient))
                    contacts.Add(entry.Recipient);
            }
            return contacts.OrderBy(c => c).ToList();
        }

        public static List<HistoryEntry> GetEntriesForContact(string contact)
        {
            if (string.IsNullOrEmpty(contact))
                return GetEntries();

            return _entries
                .Where(e =>
                    (e.Sender != null && e.Sender.Equals(contact, StringComparison.OrdinalIgnoreCase)) ||
                    (e.Recipient != null && e.Recipient.Equals(contact, StringComparison.OrdinalIgnoreCase))
                )
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }

        public static List<HistoryEntry> SearchEntries(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return GetEntries();

            return _entries
                .Where(e => e.Message != null && e.Message.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }
    }
}