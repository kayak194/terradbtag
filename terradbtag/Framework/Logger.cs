using System.Collections.ObjectModel;

namespace terradbtag.Framework
{
    static class Logger
    {
        public static ObservableCollection<string> Logs { get; private set; } = new ObservableCollection<string>();

        public static void Log(string msg)
        {
            Logs.Add(msg);
        }

        public static void Clear()
        {
            Logs.Clear();
        }
    }
}
