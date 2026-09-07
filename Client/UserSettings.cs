using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Client
{
    public static class UserSettings
    {
        public static int RetentionMonths { get; set; } = 1;
        public static bool ShowSmiles { get; set; } = true;

        private static string _settingsPath;

        public static void Initialize(string appDataFolder)
        {
            Directory.CreateDirectory(appDataFolder);
            _settingsPath = Path.Combine(appDataFolder, "settings.json");
            Load();
        }

        public static void Load()
        {
            if (File.Exists(_settingsPath))
            {
                try
                {
                    string json = File.ReadAllText(_settingsPath);
                    var serializer = new JavaScriptSerializer();
                    var data = serializer.Deserialize<Dictionary<string, object>>(json);
                    if (data != null)
                    {
                        if (data.ContainsKey("RetentionMonths"))
                            RetentionMonths = Convert.ToInt32(data["RetentionMonths"]);
                        if (data.ContainsKey("ShowSmiles"))
                            ShowSmiles = Convert.ToBoolean(data["ShowSmiles"]);
                    }
                }
                catch
                {
                    // Если файл повреждён – используем значения по умолчанию
                    RetentionMonths = 1;
                    ShowSmiles = true;
                }
            }
            else
            {
                // Файла нет – создаём с настройками по умолчанию
                RetentionMonths = 1;
                ShowSmiles = true;
                Save();
            }
        }

        public static void Save()
        {
            try
            {
                var data = new Dictionary<string, object>
                {
                    { "RetentionMonths", RetentionMonths },
                    { "ShowSmiles", ShowSmiles }
                };
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(data);
                File.WriteAllText(_settingsPath, json);
            }
            catch
            {
                // Игнорируем ошибки сохранения
            }
        }
    }
}