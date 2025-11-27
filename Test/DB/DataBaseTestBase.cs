using Dapper;
using Library.Infrastructure.Misc;
using System.IO;

namespace Test.DB
{
    public abstract class DatabaseTestBase
    {
        protected string ConnectionString { get; private set; } = null!;
        protected string DatabasePath { get; private set; } = null!;

        [SetUp]
        public void SetUpDatabase()
        {
            var baseDir = AppContext.BaseDirectory;
            var appData = Path.Combine(baseDir, "App_Data");

            if (!Directory.Exists(appData))
                throw new Exception("Directory does not exists");

            var initScriptPath = Path.Combine(appData, "init.sql");
            DatabasePath = Path.Combine(appData, "library.db");
            var scriptPath = Path.Combine(appData, "init.sql");

            if (File.Exists(DatabasePath))
                File.Delete(DatabasePath);

            ConnectionString = $"Data Source={DatabasePath};Version=3;";

            DataBase.BaseConnection = ConnectionString;
            var script = File.ReadAllText(scriptPath);
            using (var connection = DataBase.CreateConnection())
            {
                connection.Execute(script);
            }
        }

        [TearDown]
        public void TearDownDatabase()
        {
            if (File.Exists(DatabasePath))
                File.Delete(DatabasePath);
        }
    }
}
