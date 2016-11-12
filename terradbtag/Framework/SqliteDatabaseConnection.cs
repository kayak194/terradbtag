using System;
using System.Data;
using System.Data.SQLite;

namespace projektseminar_test.Framework
{
    class SqliteDatabaseConnection
    {
        private SQLiteConnection Connection { get; set; } = new SQLiteConnection();

        public bool IsConnected => Connection.State == ConnectionState.Open;

        public void Connect(string file)
        {
            var builder = new SQLiteConnectionStringBuilder { DataSource = file, BusyTimeout = 10000000 };
            Connection = new SQLiteConnection(builder.ToString());
            Connection.Open();
        }

        private TResult GenerateSqliteCommand<TResult>(string sql, Func<IDbCommand, TResult> action)
        {
            var command =  new SQLiteCommand
            {
                CommandText = sql,
                Connection = Connection
            };

            var result = action(command);

            command.Dispose();

            return result;
        }

        public int Execute(string sql)
        {
            return GenerateSqliteCommand(sql, command => command.ExecuteNonQuery());
        }

        public IDataReader Query(string sql)
        {
            try
            {
                return GenerateSqliteCommand(sql, command => command.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " Query: " + sql);
            }
        }

        public object Scalar(string sql)
        {
            return GenerateSqliteCommand(sql, command => command.ExecuteScalar());
        }

        public bool HasTable(string name)
        {
            return
                Convert.ToInt32(
                    Scalar($"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{name}';")) == 1;
        }

        public void Disconnect()
        {
            Connection.Close();
        }
    }
}
