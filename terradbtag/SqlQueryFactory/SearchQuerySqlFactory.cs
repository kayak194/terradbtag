using System.Linq;
using terradbtag.Models;

namespace terradbtag.SqlQueryFactory
{
    public static class SearchQuerySqlFactory
    {
        public static string TableName => "FilteredBusinessObjects";
        public static string GetSql(ISearchQuery searchQuery)
        {
            var tagFilter = "";
            var countConstraint = "";
            var selectedTagCount = searchQuery.SelectedTags.Count;
            if (selectedTagCount > 0)
            {
                var filterList = $"'{string.Join("', '", searchQuery.SelectedTags.Select(tag => tag.Content))}'";
                tagFilter = $"AND content IN ({filterList})";
                countConstraint = $"HAVING COUNT(id) = {selectedTagCount}";
            }

            var textFilterSql = "";
            if (!string.IsNullOrEmpty(searchQuery.TextQuery))
            {
                textFilterSql = $"AND (name LIKE '%{searchQuery.TextQuery}%' OR data LIKE '%{searchQuery.TextQuery}%')";
            }

            var sql =
                $"SELECT id, name FROM BusinessObject, Tag WHERE id = business_object {textFilterSql} {tagFilter} GROUP BY id {countConstraint} ORDER BY COUNT(id) DESC";

            var commonTableExpression = $"WITH {TableName}(id, name) AS ({sql})";

            return commonTableExpression;
        }
    }
}