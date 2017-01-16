using System;
using System.Collections.Generic;
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

            var sqlBase = SearchQuerySqlFactory.GetSql(query);
            var where = "";
            if (query.SelectedTags.Count > 0)
            {
                var filterList = "'"+string.Join("', '", query.SelectedTags.Select(tag => tag.Text)) + "'";

                where = $"WHERE content NOT IN ({filterList}) ";
            }

            var sql = sqlBase +
                $"SELECT content, count(content) as weight FROM Tag JOIN {SearchQuerySqlFactory.TableName} ON {SearchQuerySqlFactory.TableName}.id = Tag.business_object {where} GROUP BY content ORDER BY COUNT(content) DESC LIMIT {query.TagLimit};";
            var tagReader = Connection.Query(sql);

                var relatedObjectsReader =
                    Connection.Query(sqlBase + $"SELECT name FROM {SearchQuerySqlFactory.TableName}");

                var relatedObjects = new List<string>();

                while (relatedObjectsReader.Read())
                {
                    relatedObjects.Add(relatedObjectsReader["name"].ToString());
                }
                relatedObjectsReader.Close();


            if (relatedObjects.Count > 0)
            {
                string relatedObjectsText;

                if (relatedObjects.Count == 1)
                {
                    relatedObjectsText = relatedObjects[0];
                }
                else
                {
                    var commaRelatedObjects = relatedObjects.Take(relatedObjects.Count - 1);
                    relatedObjectsText = string.Join(", ", commaRelatedObjects) + " und " +
                                         relatedObjects[relatedObjects.Count - 1];
                }

                Logger.Log($"Lade Tags, die mit {relatedObjectsText} in Beziehung stehen.");
            }

            Debug.WriteLine(sql);
            var i = 0;
            ReportProgress(i,query.TagLimit);
            Logger.Log("Zeige die ersten 5 Tags mit Gewichten:");
            while (tagReader.Read())
            {
                var tag = new Tag
                {
                    Text = tagReader["content"].ToString(),
                    Weight = Convert.ToInt32(tagReader["weight"])
                };

                if (i < 5)
                    Logger.Log($"[{tag.Weight:00}] {tag.Text}");

                ReportProgress(i++,query.TagLimit, tag);
            }
            tagReader.Close();

            return true;
        }
    }
}
