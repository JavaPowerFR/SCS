using System.Data;
using System.Data.SQLite;

namespace TcpSnifferOWN
{
    internal class DBInstance
    {
        private SQLiteConnection sqlite;

        public DBInstance(string path)
        {
            string cs = $"Data Source={path};Version=3;";
            sqlite = new SQLiteConnection(cs);
        }

        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt);
            }
            catch (SQLiteException ex) { }
            sqlite.Close();
            return dt;
        }
    }
}
