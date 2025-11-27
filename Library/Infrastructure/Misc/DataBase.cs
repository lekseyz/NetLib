using Dapper;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Library.Infrastructure.Misc
{
    public class DataBase
    {
        public static string BaseConnection = null;
        public static IDbConnection CreateConnection()
        {
            string connectionString;
            if (BaseConnection.IsNullOrWhiteSpace())
                connectionString = ConfigurationManager.ConnectionStrings["LibraryDb"].ConnectionString;
            else 
                connectionString = BaseConnection;

                var connection = new SQLiteConnection(connectionString);
            connection.Open();
            connection.Execute("PRAGMA foreign_keys = ON;");
            return connection;
        }

        public static void Init()
        {
            using (var connection = CreateConnection())
            {
                var path = HttpContext.Current.Server.MapPath("~/App_Data/init.sql");
                var script = File.ReadAllText(path);
                connection.Execute(script);
            }
        }
    }
}