using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;

namespace terradbtag.Services
{
    class DataLoadingService: Service
    {
        public Repository Repository { get; set; }

        protected override bool ServiceAction(object args)
        {
            var query = args as ISearchQuery;
            if (query == null) return false;

            var tagFilter = "";
            if (query.SelectedTags.Count > 0)
            {
                var filterList = "'"+string.Join("', '", query.SelectedTags.Select(tag => tag.Content)) + "'";
                tagFilter = $"AND content IN ({filterList})";
            }

            var textFilterSql = "";
            if (!string.IsNullOrEmpty(query.TextQuery))
            {
                textFilterSql = $"AND (name LIKE '%{query.TextQuery}%' OR data LIKE '%{query.TextQuery}%')";
            }

            var sql =
                $"SELECT id FROM BusinessObject, Tag WHERE id = business_object {textFilterSql} {tagFilter} GROUP BY id ORDER BY COUNT(id) DESC LIMIT {query.ResultLimit}";

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
