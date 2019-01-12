using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace Lil_Dan
{
    public static class Database
    {
        private static MySqlConnection _connection;

        public static void Start(string server, string user, string database, string port, string password)
        {
            string connectionString = $"server={server};user={user};database={database};port={port};password={password}";
            _connection = new MySqlConnection(connectionString);
            _connection.StateChange += OnStateChanged;

            Console.WriteLine("Connecting to MySQL database...");
            
            _connection.Open();
        }
        private static void OnStateChanged(object sender, System.Data.StateChangeEventArgs e)
        {
            Console.WriteLine($"MySQL State: {e.CurrentState}");
        }
        private static void OnDatabaseMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            Console.WriteLine(args);
        }
    }
}
