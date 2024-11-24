using System;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace NovaPanel.Data.Auth
{
    public class CookieCheck
    {
        public static bool IsSessionValid(HttpContext context)
        {
            foreach (var cks in context.Request.Cookies)
            {
                if (cks.Key == "sessions")
                {
                    return CheckSessionValidity(cks.Value);
                }
            }
            return false;
        }

        private static bool CheckSessionValidity(string sessionCode)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={Setting.DataBaseFile};Version=3;"))
            {
                connection.Open();

                string query = @"
                    SELECT ValidTime 
                    FROM Auth 
                    WHERE SessionCode = @SessionCode;
                ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionCode", sessionCode);

                    object result = command.ExecuteScalar();
                    if (result != null && DateTime.TryParse(result.ToString(), out DateTime validTime))
                    {
                        return validTime > DateTime.Now;
                    }
                }
            }
            return false;
        }

        public static string GenerateSessionCode(string username, string password)
        {
            DateTime validTime = DateTime.Now.AddHours(2);
            string rawString = $"{username}{password}{validTime:yyyyMMddHHmmss}";
            string sessionCode = ComputeMd5Hash(rawString);
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={Setting.DataBaseFile};Version=3;"))
            {
                connection.Open();

                string query = @"
                    INSERT INTO Auth (SessionCode, Username, ValidTime) 
                    VALUES (@sessionCode, @username, @validTime);
                ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sessionCode", sessionCode);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@validTime", validTime);

                    int rowsAffected = command.ExecuteNonQuery();
                    return sessionCode;
                }
            }
        }

        // 计算 MD5 摘要
        private static string ComputeMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
