using System;
using System.Collections.ObjectModel;

namespace terradbtag.Framework
{
    static class Logger
    {
        public static ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();

        public static bool LogErrors { get; set; } = true;

        public static void Log(string msg)
        {
            Logs.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {msg}");
        }

        public static void LogError(string errMsg)
        {
            if(LogErrors) Log(errMsg);
        }

        public static void Clear()
        {
            Logs.Clear();
        }
    }
}
