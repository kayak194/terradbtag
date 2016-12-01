using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;
using terradbtag.SqlQueryFactory;
using WordCloudCalculator.Contract;
using WordCloudCalculator.Contract.Visualization;
using WordCloudCalculator.Contract.Word;
using WordCloudCalculator.ExtractingWordCloudCalculator;
using WordCloudCalculator.WordCloudCalculator;

namespace terradbtag.Services
{
    class TagLoadingService: Service
    {
        public SqliteDatabaseConnection Connection { get; set; }

        public static Size GetTextMetrics(string text, double size) => new Size(text.Length, 1);

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
                $"SELECT content, count(content) as weight FROM Tag JOIN {SearchQuerySqlFactory.TableName} ON {SearchQuerySqlFactory.TableName}.id = Tag.business_object {where} GROUP BY content ORDER BY COUNT(content) DESC LIMIT {query.TagLimit};";
            var tagReader = Connection.Query(sql);

            var weightedList = new List<WeightedWord>();

            Debug.WriteLine(sql);
            var i = 0;
            ReportProgress(i,query.TagLimit);
            while (tagReader.Read())
            {
                //ReportProgress(i++,query.TagLimit, new Tag{Content = tagReader["content"].ToString()});
                weightedList.Add(new WeightedWord() { Text = tagReader["content"].ToString(), Weight = tagReader.GetInt32(1) });
            }

            var weightedExtractingWordCloudCalculator = new ExtractingWordCloudCalculator<SimpleAppearenceCalculationMethod>();

            var appearenaceArgs = new WordCloudAppearenceArguments()
            {
               // PanelSize = new Size(width, height), //TODO: Where do we get Width and Height
                FontSizeRange = new Range(0.0, 15.0),
                OpacityRange = new Range(0.5, 1.0),
                WordMargin = Margin.None,
                WordSizeCalculator = GetTextMetrics
            };

            var ret = weightedExtractingWordCloudCalculator.Calculate(appearenaceArgs, weightedList, row => new WeightedWord { Text = row.Text, Weight = row.Weight });

            ReportProgress(100, 100, ret);        

            return true;
        }
    }
}
