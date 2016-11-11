namespace projektseminar_test
{
    class TagLoadingService: Service
    {
        public SqliteDatabaseConnection Connection { get; set; }

        protected override bool ServiceAction(object args)
        {
            var sql =
                "SELECT content FROM Tag WHERE business_object IN (SELECT DISTINCT business_object FROM Tag) GROUP BY content ORDER BY COUNT(*) DESC";
            var tagReader = Connection.Query(sql);

            while (tagReader.Read())
            {
                ReportProgress(0,1, new Tag{Content = tagReader["content"].ToString()});
            }
            return true;
        }
    }
}
