using System.Linq;
using terradbtag.Models;

namespace terradbtag.SqlQueryFactory
{
    public static class SearchQuerySqlFactory
    {
        public static string TableName => "FilteredBusinessObjects";

        public static string GetSql(ISearchQuery searchQuery)
        {
            var tagFilter = string.Empty;
            var selectedTagCount = searchQuery.SelectedTags.Count;
            if (selectedTagCount > 0)
            {
                for (var i = 0; i < selectedTagCount; i++)
                {
                    tagFilter +=
                        $"JOIN Tag AS tag{i} ON BusinessObject.id = tag{i}.business_object AND tag{i}.content = '{searchQuery.SelectedTags[i].Text}' ";
                }
            }

            var textFilterSql = "";
            if (!string.IsNullOrEmpty(searchQuery.TextQuery))
            {
                textFilterSql =
                    $"WHERE name LIKE '%{searchQuery.TextQuery}%'";
            }

            var sql =
                $"SELECT id, name FROM BusinessObject {tagFilter} {textFilterSql}";

            var commonTableExpression = $"WITH {TableName}(id, name) AS ({sql})";

            return commonTableExpression;
        }
    }
}