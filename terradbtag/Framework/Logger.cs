using System;
using System.Collections.ObjectModel;

namespace terradbtag.Framework
{
    static class Logger
    {
        public static ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();

        public static void Log(string msg)
        {
            Logs.Add($"{DateTime.Now:yyyy-MM-dd hh:mm:ss} {msg}");
        }

        public static void Clear()
        {
            Logs.Clear();
        }
    }
}
