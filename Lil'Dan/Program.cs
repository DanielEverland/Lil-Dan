using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Lil_Dan.Event_Handlers;

namespace Lil_Dan
{
    class Program
    {
        public static void Main(string[] args)
        {
            string token = args[0];
            string server = args[1];
            string user = args[2];
            string database = args[3];
            string port = args[4];
            string password = args[5];

            Database.Start(server, user, database, port, password);
            Database.InsertProfile("119573797392220161", "AG", 546);
            //new Program().MainAsync(token).GetAwaiter().GetResult();
            Console.ReadLine();
        }
             

        public async Task MainAsync(string token)
        {
            DiscordSocketClient client = new DiscordSocketClient();

            client.Log += Log;
            
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            EventManager.RegisterEvents(client);
            EventHandlerRegister.RegisterEventHandlers();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
