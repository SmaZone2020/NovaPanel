using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace NovaPanel.Data
{
    public static class Setting
    {
        public static string DataBaseFile = "Data.dll";
        public static void Creat()
        {
            if (!System.IO.File.Exists(DataBaseFile))
            {
                SQLiteConnection.CreateFile(DataBaseFile);
            }
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataBaseFile};Version=3;"))
            {
                connection.Open();

                // Users
                string createUsersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Username TEXT PRIMARY KEY,
                        Password TEXT,
                        Mail TEXT,
                        RegDate DATE,
                        Level INT,
                        Record TEXT
                    );
                ";
                ExecuteNonQuery(connection, createUsersTableQuery);

                // Auth
                string createAuthTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Auth (
                        SessionCode TEXT PRIMARY KEY,
                        Username TEXT,
                        ValidTime DATE
                    );
                ";
                ExecuteNonQuery(connection, createAuthTableQuery);

                // Setting
                string createSettingTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Setting (
                        SafeMain TEXT,
                        Port SMALLINT,
                        ServerName TEXT,
                        WarningMail TEXT
                    );
                ";
                ExecuteNonQuery(connection, createSettingTableQuery);

                Console.WriteLine("Tables created successfully.");
            }
        }
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        private static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
}
