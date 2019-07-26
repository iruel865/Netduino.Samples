using System.IO;

namespace PlantMonitorGateway.IO
{
    public static class FileManager
    {
        public static void Initialize()
        {            
            File.Create(App.LOG_FILE_NAME);
        }

        public static string ReadFile()
        {            
            return File.ReadAllText(App.LOG_FILE_NAME);
        }

        public static void SaveLog(string jsonData)
        {
            File.WriteAllText(App.LOG_FILE_NAME, jsonData);
        }
    }
}