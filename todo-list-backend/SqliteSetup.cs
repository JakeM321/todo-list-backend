using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend
{
    public class SqliteSetup
    {
        public const string dbFile = @"bin/app.db";
        public static string ConnectionString
        {
            get
            {
                return String.Format(@"Data Source = {0}", dbFile);
            }
        }
        public static void RunSetup()
        {
            if (!File.Exists(dbFile))
            {
                Console.WriteLine("Creating DB");
                using (var conn = new SqliteConnection(ConnectionString))
                {
                    conn.Open();
                    var text = File.ReadAllText("sqliteMigration.sql");
                    using (var cmd = new SqliteCommand(text, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("DB already exists");
            }
        }
    }
}
