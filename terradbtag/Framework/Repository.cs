using System;
using System.Collections.Generic;
using System.Linq;
using terradbtag.Models;

namespace terradbtag.Framework
{
    class Repository
    {
        public SqliteDatabaseConnection Connection { get; set; }

        public void Create(BusinessObject obj)
        {
            Connection.Execute($"INSERT INTO BusinessObject VALUES (\"{obj.Id}\", \"{obj.Name}\", \"{obj.Data}\");");
            foreach (var tag in obj.Tags)
            {
                AddTag(obj,tag);
            }
        }

        public void Update(BusinessObject obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        private void AddTag(BusinessObject obj, Tag tag)
        {
            if(string.IsNullOrEmpty(tag.Text)) return;
            Connection.Execute($"INSERT INTO Tag VALUES (\"{tag.Text}\", \"{obj.Id}\");");
        }

        private void DeleteTags(string businessObjectId)
        {
            Connection.Execute($"DELETE FROM Tag WHERE business_object = \"{businessObjectId}\";");
        }

        public bool Exists(string id)
        {
            return Convert.ToInt32(Connection.Scalar($"SELECT COUNT(id) FROM BusinessObject WHERE id = \"{id}\";").ToString()) > 0;
        }

        public List<BusinessObject> FindMany(IEnumerable<string> ids)
        {
            return ids.Select(Find).ToList();
        }

        public List<string> FindAll()
        {
            var query = Connection.Query("SELECT id FROM BusinessObject;");
            var list = new List<string>();
            while (query.Read())
            {
                list.Add(query["id"].ToString());
            }
            return list;
        }

        public BusinessObject Find(string id)
        {
            var query = Connection.Query($"SELECT * FROM BusinessObject WHERE id = \"{id}\";");
            query.Read();
            var obj = new BusinessObject()
            {
                Id = query["id"].ToString(),
                Name = query["name"].ToString(),
                Data = query["data"].ToString()
            };

            query = Connection.Query($"SELECT * FROM Tag WHERE business_object = \"{id}\";");
            while (query.Read())
            {
                obj.AddTag(new Tag
                {
                    Text = query["content"].ToString()
                });
            }
            return obj;
        }

        public void Delete(string id)
        {
            DeleteTags(id);
            Connection.Execute($"DELETE FROM BusinessObject WHERE id = \"{id}\";");
        }
    }
}
