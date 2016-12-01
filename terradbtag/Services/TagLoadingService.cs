using System.Diagnostics;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;
using terradbtag.SqlQueryFactory;

namespace terradbtag.Services
{
    class TagLoadingService: Service
    {
        public SqliteDatabaseConnection Connection { get; set; }

        protected override bool ServiceAction(object args)
        {
            var query = args as ISearchQuery;

            if (query == null) return false;

            var sql = SearchQuerySqlFactory.GetSql(query);
            var where = "";
            if (query.SelectedTags.Count > 0)
            {
                var filterList = "'"+string.Join("', '", query.SelectedTags.Select(tag => tag.Content)) + "'";

                where = $"WHERE content NOT IN ({filterList}) ";
            }

            sql +=
                $"SELECT content FROM Tag JOIN {SearchQuerySqlFactory.TableName} ON {SearchQuerySqlFactory.TableName}.id = Tag.business_object {where} GROUP BY content ORDER BY COUNT(content) DESC LIMIT {query.TagLimit};";
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
