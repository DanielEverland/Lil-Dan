using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;

namespace Lil_Dan
{
    public static class Database
    {
        private static MySqlConnection connection;

        private const string QUERY_ASSIGN_VALUE = "UPDATE discordbot.user_profiles SET {0} = {1} WHERE (id = {2});";
        private const string QUERY_RETRIEVE_VALUE = "SELECT {0} FROM discordbot.user_profiles WHERE id = {1}";
        private const string QUERY_INSERT_PROFILE = "INSERT INTO discordbot.user_profiles (id, username, message_count) VALUES ({0}, \"{1}\", {2});";

        private const string VALUE_MESSAGE_COUNT = "message_count";
        private const string VALUE_ID = "id";

        public static async void Start(string server, string user, string database, string port, string password)
        {
            string connectionString = $"server={server};user={user};database={database};port={port};password={password}";
            connection = new MySqlConnection(connectionString);
            connection.StateChange += OnStateChanged;

            Debug.Log("Connecting to MySQL database...");
            
            await connection.OpenAsync();
        }
        public static async void Close()
        {
            Debug.Log("Closing connection to MySQL database...");

            await connection.CloseAsync();
        }
        private static void OnStateChanged(object sender, System.Data.StateChangeEventArgs e)
        {
            Debug.Log($"MySQL State: {e.CurrentState}");
        }
        private static void OnDatabaseMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            Debug.Log(args);
        }
        
        public static async Task<uint> GetMesssageCount(object id)
        {
            return await GetValue<uint>(VALUE_MESSAGE_COUNT, id);
        }
        public static async Task SetMessageCount(object id, uint value)
        {
            await SetValue(VALUE_MESSAGE_COUNT, id, value);
        }
        public static async Task InsertProfile(object id, string username, uint messageCount)
        {
            await InsertData(id, username, messageCount);
        }

        public static async Task<bool> HasProfile(object id)
        {
            string query = string.Format(QUERY_RETRIEVE_VALUE, VALUE_ID, id);
            Debug.Log($"{query}");

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                var value = await command.ExecuteScalarAsync();
                Debug.Log($"    Value: {value}");

                return value != null;
            }
            catch (Exception e)
            {
                Debug.Log("---- QUERY FAILED ----");
                Debug.Log($"{query}");
                Debug.Log($"{e.Message}");
                throw;
            }
        }
        private static async Task<T> GetValue<T>(string valueName, object id)
        {
            string query = string.Format(QUERY_RETRIEVE_VALUE, valueName, id);
            Debug.Log($"{query}");

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                var value = await command.ExecuteScalarAsync();
                Debug.Log($"    Value: {value}");

                if (value == null)
                    return default(T);

                return (T)value;
            }
            catch (Exception e)
            {
                Debug.Log("---- QUERY FAILED ----");
                Debug.Log($"{query}");
                Debug.Log($"{e.Message}");
                throw;
            }            
        }
        private static async Task SetValue(string valueName, object id, object value)
        {
            string query = string.Format(QUERY_ASSIGN_VALUE, valueName, value, id);
            Debug.Log($"{query}");

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Debug.Log("---- UPDATE FAILED ----");
                Debug.Log($"{query}");
                Debug.Log($"{e.Message}");
                throw;
            }
        }
        private static async Task InsertData(object id, object username, object messageCount)
        {
            string query = string.Format(QUERY_INSERT_PROFILE, id, username, messageCount);
            Debug.Log($"{query}");

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                int x = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Debug.Log("---- INSERTION FAILED ----");
                Debug.Log($"{query}");
                Debug.Log($"{e.Message}");
                throw;
            }            
        }
    }
}
