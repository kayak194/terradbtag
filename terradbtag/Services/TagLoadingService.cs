using System.Diagnostics;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;

namespace terradbtag.Services
{
    class TagLoadingService: Service
    {
        public SqliteDatabaseConnection Connection { get; set; }

        protected override bool ServiceAction(object args)
        {
            var query = args as ISearchQuery;

            if (query == null) return false;

            var where = "";
            if (query.SelectedTags.Count > 0)
            {
                var filterList = "'"+string.Join("', '", query.SelectedTags.Select(tag => tag.Content)) + "'";
                where = $"JOIN (SELECT business_object as bo FROM Tag WHERE content IN ({filterList}) GROUP BY bo HAVING COUNT(business_object) = {query.SelectedTags.Count}) as t2 ON business_object = bo  WHERE content NOT IN ({filterList}) ";
            }

            var sql =
                $"SELECT content FROM Tag {where} GROUP BY content ORDER BY COUNT(content) DESC LIMIT {query.TagLimit}";
            var tagReader = Connection.Query(sql);
            Debug.WriteLine(sql);
            var i = 0;
            ReportProgress(i,query.TagLimit);
            while (tagReader.Read())
            {
                ReportProgress(i++,query.TagLimit, new Tag{Content = tagReader["content"].ToString()});
            }

            return true;
        }
    }
}
