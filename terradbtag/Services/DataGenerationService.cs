using System;
using System.Collections.Generic;
using System.Linq;
using terradbtag.Framework;
using terradbtag.Models;

namespace terradbtag.Services
{
    class DataGenerationService : Service
    {
        private class Entity
        {
            public Entity(string tableName, string prefix, string typeTag, string[] dataProperties, string[] tagProperties)
            {
                TableName = tableName;
                Prefix = prefix;
                TypeTag = typeTag;
                DataProperties = dataProperties;
                TagProperties = tagProperties;
            }

            public string TableName { get; }
            public string Prefix { get; }
            public string TypeTag { get; }
            public string[] DataProperties { get; }
            public string[] TagProperties { get; }
        }

        public Repository Repository { get; set; }

        private SqliteDatabaseConnection Connection { get; } = new SqliteDatabaseConnection();

        private void CreateBusinessObject(string id, string name, string data, string[] tags)
        {
            var obj = new BusinessObject
            {
                Id = id,
                Name = name,
                Data = data,
                Tags = tags.Select(s => new Tag { Content = s }).ToList()
            };

            Repository.Create(obj);
        }

        private void ExtractData(Entity entity)
        {
            var name = $"{entity.Prefix}_NAME";
            var columns = new List<string> {name};
            if(entity.DataProperties != null) columns.AddRange(entity.DataProperties);
            if(entity.TagProperties != null) columns.AddRange(entity.TagProperties);

            if (entity.TableName == "STADT")
            {
                columns.Add("L_ID");
                columns.Add("LT_ID");
            }
          
            var sql = $"SELECT {string.Join(", ", columns)} FROM {entity.TableName};";

            var reader = Connection.Query(sql);
           
            var i = 1;
            while (reader.Read())
            {
                var dataString = entity.DataProperties == null ? "" : entity.DataProperties.Aggregate("", (current, s) => string.IsNullOrEmpty(reader[s].ToString()) ? "" : current + $"{s}: {reader[s]}" + Environment.NewLine);
                var tagsList = new List<string> { entity.TypeTag, reader[name].ToString() };
                if (entity.TagProperties != null)
                {
                    tagsList.AddRange(entity.TagProperties.Select(s => reader[s].ToString()).ToList());
                }

                if (entity.TableName == "STADT")
                {
                    var lidVal = reader["L_ID"];
                    var ltidVal = reader["LT_ID"];
                    var nameVal = reader[name];
                    var liegtAnSql = $"SELECT F_NAME, S_NAME, M_NAME FROM LIEGT_AN WHERE L_ID = '{lidVal}' AND LT_ID = '{ltidVal}' AND ST_NAME = \"{nameVal}\"";
                    var liegtAnReader = Connection.Query(liegtAnSql);
                    while (liegtAnReader.Read())
                    {
                        var fluss = liegtAnReader["F_NAME"].ToString();
                        var see = liegtAnReader["S_NAME"].ToString();
                        var meer = liegtAnReader["M_NAME"].ToString();

                        if (fluss != "")
                        {
                            tagsList.Add(fluss);
                            tagsList.Add("fluss");
                        }

                        if (see != "")
                        {
                            tagsList.Add(see);
                            tagsList.Add("see");
                        }

                        if (meer != "")
                        {
                            tagsList.Add(meer);
                            tagsList.Add("meer");
                        }
                    }


                    var liegtInSql = $"SELECT L_NAME FROM LAND WHERE L_ID = '{lidVal}'";
                    var liegtInVal = Connection.Scalar(liegtInSql);
                    if(liegtInVal != null)
                        tagsList.Add(liegtInVal.ToString());
                }

                var geoTableName = $"GEO_{entity.TableName}";
                if (Connection.HasTable(geoTableName))
                {
                    var geoSql = $"SELECT L_NAME, LT_NAME FROM {geoTableName}, LANDTEIL, LAND WHERE {name}=\"{reader[name]}\" AND LAND.L_ID = {geoTableName}.L_ID AND LANDTEIL.L_ID = {geoTableName}.L_ID AND LANDTEIL.LT_ID = {geoTableName}.LT_ID;";
                    var geoReader = Connection.Query(geoSql);
                    while (geoReader.Read())
                    {
                        tagsList.Add(geoReader["L_NAME"].ToString());
                        tagsList.Add(geoReader["LT_NAME"].ToString());
                    }
                }

                CreateBusinessObject(GetId(entity.Prefix, i++), ConvertToName(reader[name].ToString()), dataString, tagsList.Distinct().Select(ConvertToTag).ToArray());
            }
        }

        private static string ConvertToName(string raw)
        {
            return raw.Replace("_", " ");
        }

        private static string ConvertToTag(string raw)
        {
            string[] invalidChars = {"_", "-", "'", ".", "/"};
            return invalidChars.Aggregate(raw, (current, invalidChar) => current.Replace(invalidChar, ""));
        }

        private static string GetId(string prefix, int i)
        {
            return $"{prefix}{i:000000000}";
        }

        protected override bool ServiceAction(object args)
        {
            var entityList = new List<Entity>
            {
                new Entity("BERG", "B", "berg", new[] {"HOEHE", "JAHR"}, new[] {"GEBIRGE"}),
                new Entity("EBENE", "E", "ebene", new[] {"HOEHE", "FLAECHE"}, null),
                new Entity("FLUSS", "F", "fluss", new[] {"LAENGE"}, new[] {"MEER", "SEE", "FLUSS"}),
                new Entity("INSEL", "I", "insel", new[] {"FLAECHE"}, new[] {"INSELGRUPPE"}),
                new Entity("LAND", "L", "land", new[] {"EINWOHNER", "FLAECHE", "HAUPTSTADT"}, null),
                new Entity("LANDTEIL", "LT", "landteil", new[] {"EINWOHNER", "HAUPTSTADT"}, null),
                new Entity("MEER", "M", "meer", new[] {"TIEFE"}, null),
                new Entity("SEE", "S", "see", new[] {"TIEFE", "FLAECHE"}, null),
                new Entity("STADT", "ST", "stadt", new[] {"EINWOHNER"}, null),
                new Entity("WUESTE", "W", "wueste", new[] {"FLAECHE"}, new[] {"WUESTENART"}),
            };


            var opener = new OpenFileService();

            if (!opener.OpenFile(OpenFileService.SqliteDatabaseFilter)) return false;

            Connection.Connect(opener.SelectedFile);

            var typeCount = entityList.Count;
            var i = 1;

            ReportProgress(0, typeCount);
            foreach (var entity in entityList)
            {
                ExtractData(entity);
                ReportProgress(i++, typeCount);
            }

            Connection.Disconnect();
            return true;
        }
    }
}