using System.Data.SQLite;

namespace Quiz.Model.Database
{
    internal class DBConnection
    {
        private static DBConnection? _instance = null;
        public static DBConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DBConnection();
                return _instance;
            }
        }

        private SQLiteConnection? _connection = null;
        public SQLiteConnection getConnection(string databasePath)
        {
            if (_connection != null)
                _connection.Dispose();
            _connection = new SQLiteConnection($"Data Source={databasePath}");
            return _connection;
        }

        private DBConnection() { }
    }
}
