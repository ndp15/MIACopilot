using System.IO;
using System.Text.Json;
using MIACopilotApp.Models;

namespace MIACopilotApp
{
    public static class DataStore
    {
        private const string FilePath = "app_data.json";

        public static AppData Data { get; set; } = new AppData();

        // Loads data from the JSON file
        public static void LoadData()
        {
            if (!File.Exists(FilePath))
            {
                Data = new AppData();
                return;
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                var loadedData = JsonSerializer.Deserialize<AppData>(json);
                if (loadedData != null)
                {
                    Data = loadedData;
                }
            }
            catch
            {
                Data = new AppData();
            }
        }

        // Saves data to the JSON file
        public static void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Data, options);
            File.WriteAllText(FilePath, json);
        }
    }
}
