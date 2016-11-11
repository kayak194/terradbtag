namespace projektseminar_test
{
    class DatabaseService
    {
        public SqliteDatabaseConnection Connection { get; set; }

        public void InitializeDatabase()
        {
            Connection.Execute(
                "CREATE TABLE IF NOT EXISTS BusinessObject(id TEXT PRIMARY KEY, name TEXT, data TEXT);" +
                "CREATE TABLE IF NOT EXISTS Tag(content TEXT, business_object TEXT);");
        }

        public void ResetDatabase()
        {
            Connection.Execute(
                "DELETE FROM BusinessObject;" +
                "DELETE FROM Tag;"
                );
        }
    }
}
