using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terradbtag.Framework;

namespace terradbtag.Services
{
    class GraphExportService : Service
    {
        public SqliteDatabaseConnection Connection { get; set; }

        protected override bool ServiceAction(object args)
        {
            var file = new OpenFileService
            {
                AcceptNonExistingFiles = true,
                ForceNewFile = true
            };

            if (!file.OpenFile()) return false;

            var sb = new StringBuilder();

            var businessObjectDataReader = Connection.Query("SELECT id, name FROM BusinessObject");

            while (businessObjectDataReader.Read())
            {
                var id = businessObjectDataReader["id"];
                var name = businessObjectDataReader["name"];
                sb.AppendLine($"CREATE ({id}:BusinessObject {{id: '{id}', name: '{name}'}} )");
            }

            var tagDataReader = Connection.Query("SELECT DISTINCT content FROM Tag");

            while (tagDataReader.Read())
            {
                var content = tagDataReader["content"];
                sb.AppendLine($"CREATE ({content}:Tag {{content: '{content}'}})");
            }

            var relationsDataReader = Connection.Query("SELECT business_object, content FROM Tag");

            while (relationsDataReader.Read())
            {
                var business_object = relationsDataReader["business_object"];
                var content = relationsDataReader["content"];

                sb.AppendLine($"CREATE ({business_object})-->({content})");
            }

            File.WriteAllText(file.SelectedFile, sb.ToString());
            return true;
        }
    }
}
