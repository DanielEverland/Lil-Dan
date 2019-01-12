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
        private static MySqlConnection _connection;

        private const string QUERY_ASSIGN_VALUE = "UPDATE discordbot.user_profiles SET {0} = {1} WHERE (id = {2});";
        private const string QUERY_RETRIEVE_VALUE = "SELECT {0} FROM discordbot.user_profiles WHERE id = {1}";
        private const string QUERY_INSERT_PROFILE = "INSERT INTO discordbot.user_profiles (id, username, message_count) VALUES ({0}, \"{1}\", {2});";

        private const string VALUE_MESSAGE_COUNT = "message_count";

        public static async void Start(string server, string user, string database, string port, string password)
        {
            string connectionString = $"server={server};user={user};database={database};port={port};password={password}";
            _connection = new MySqlConnection(connectionString);
            _connection.StateChange += OnStateChanged;

            Console.WriteLine("Connecting to MySQL database...");
            
            await _connection.OpenAsync();
        }
        private static void OnStateChanged(object sender, System.Data.StateChangeEventArgs e)
        {
            Console.WriteLine($"MySQL State: {e.CurrentState}");
        }
        private static void OnDatabaseMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            Console.WriteLine(args);
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

        private static async Task<T> GetValue<T>(string valueName, object id)
        {
#if DEBUG
            Console.WriteLine($"Reading {valueName} WHERE ID: {id}");
#endif
            string query = string.Format(QUERY_RETRIEVE_VALUE, valueName, id);

            try
            {
                MySqlCommand command = new MySqlCommand(query, _connection);
                var value = await command.ExecuteScalarAsync();

                if (value == null)
                    return default(T);

                return (T)value;
            }
            catch (Exception e)
            {
                Console.WriteLine("---- QUERY FAILED ----");
                Console.WriteLine($"{query}");
                Console.WriteLine($"{e.Message}");
                throw;
            }            
        }
        private static async Task SetValue(string valueName, object id, object value)
        {
#if DEBUG
            Console.WriteLine($"Setting {valueName} to \"{value}\" WHERE ID: {id}");
#endif
            string query = string.Format(QUERY_ASSIGN_VALUE, valueName, value, id);

            try
            {
                MySqlCommand command = new MySqlCommand(query, _connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("---- UPDATE FAILED ----");
                Console.WriteLine($"{query}");
                Console.WriteLine($"{e.Message}");
                throw;
            }
        }
        private static async Task InsertData(object id, object username, object messageCount)
        {
#if DEBUG
            Console.WriteLine($"Inserting profile ({id}, {username}, {messageCount})");
#endif
            string query = string.Format(QUERY_INSERT_PROFILE, id, username, messageCount);

            try
            {
                MySqlCommand command = new MySqlCommand(query, _connection);
                int x = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("---- INSERTION FAILED ----");
                Console.WriteLine($"{query}");
                Console.WriteLine($"{e.Message}");
                throw;
            }            
        }
    }
}
