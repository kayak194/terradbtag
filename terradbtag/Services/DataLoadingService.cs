using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;
using terradbtag.SqlQueryFactory;

namespace terradbtag.Services
{
    class DataLoadingService : Service
    {
        public Repository Repository { get; set; }

        protected override bool ServiceAction(object args)
        {
            var query = args as ISearchQuery;
            if (query == null) return false;

            var sql = SearchQuerySqlFactory.GetSql(query);
            sql += $" SELECT id FROM {SearchQuerySqlFactory.TableName} LIMIT {query.ResultLimit};";

            var filterText = "";

            if (query.SelectedTags.Count > 0)
            {
                var tagText = "";
                var selectedTagsCount = query.SelectedTags.Count;
                if (selectedTagsCount == 1)
                {
                    tagText = query.SelectedTags[0].Text;
                }
                else
                {
                    var commaTags = query.SelectedTags.Take(selectedTagsCount - 1);
                    tagText += string.Join(", ", commaTags.Select(tag => tag.Text));
                    tagText += " und " + query.SelectedTags[selectedTagsCount - 1].Text;
                }
                filterText += $", die mit {tagText} getaggt wurden";
            }

            if (!string.IsNullOrEmpty(query.TextQuery))
            {
                if (filterText.Length != 0) filterText += " und ";
                else
                {
                    filterText += ", die ";
                }

                filterText += $"\"{query.TextQuery}\" enthalten";
            }

            Logger.Log($"Lade Figuren{filterText}.");

            Debug.WriteLine(sql);

            var result = Repository.Connection.Query(sql);

            ReportProgress(0, query.ResultLimit);

            var i = 0;
            while (result.Read())
            {
                ReportProgress(i++, query.ResultLimit, Repository.Find(result["id"].ToString()));
            }

            return true;
        }

    }
}