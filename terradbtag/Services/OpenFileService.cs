using System.IO;
using Microsoft.Win32;

namespace terradbtag.Services
{
    public class OpenFileService
    {
        public const string SqliteDatabaseFilter = "SQLite Datenbank | *.sqlite";
        public const string XmlFileFilter = "XML Datei | *.xml";

        public bool SelectedFileExists { get; private set; } = false;

        public string SelectedFile { get; private set; } = "";

        public bool ForceNewFile { get; set; } = false;

        public bool AcceptNonExistingFiles { get; set; } = false;

        public bool OpenFile(string filter = "Alle Dateien | *.*")
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = false,
                Filter = filter,
                CheckFileExists = !AcceptNonExistingFiles
            };
            if (ofd.ShowDialog() != true)
            {
                return false;
            }
            var file = ofd.FileName;
            SelectedFileExists = File.Exists(file);
            if (SelectedFileExists && ForceNewFile)
            {
                File.Delete(file);
            }
            SelectedFile = file;
            return true;
        }
    }
}