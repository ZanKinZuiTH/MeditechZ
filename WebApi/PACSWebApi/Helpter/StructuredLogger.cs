using System;
using System.Configuration;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PACSWebApi.Helpter
{
    public static class StructuredLogger
    {
        private static readonly object _sync = new object();

        private static string GetLogDirectory()
        {
            var path = ConfigurationManager.AppSettings["StructuredLogPath"];
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            catch { }
            return path;
        }

        private static string GetLogFile()
        {
            var dir = GetLogDirectory();
            var file = Path.Combine(dir, $"pacswebapi-{DateTime.UtcNow:yyyyMMdd}.log");
            return file;
        }

        public static void Info(object payload)
        {
            Write("INFO", payload);
        }

        public static void Error(object payload)
        {
            Write("ERROR", payload);
        }

        private static void Write(string level, object payload)
        {
            try
            {
                var line = JsonConvert.SerializeObject(new
                {
                    level,
                    utc = DateTime.UtcNow,
                    payload
                });
                lock (_sync)
                {
                    File.AppendAllText(GetLogFile(), line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch { }
        }
    }
}


