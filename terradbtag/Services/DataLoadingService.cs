using System.Diagnostics;
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