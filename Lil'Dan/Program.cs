using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Lil_Dan.Event_Handlers;

namespace Lil_Dan
{
    class Program
    {
        public static DiscordSocketClient Client { get; private set; }

        public static void Main(string[] args)
        {
            string token = args[0];
            string server = args[1];
            string user = args[2];
            string database = args[3];
            string port = args[4];
            string password = args[5];
            
            new Program().MainAsync(token).GetAwaiter().GetResult();
        }
             

        public async Task MainAsync(string token)
        {
            Client = new DiscordSocketClient();
            Client.Ready += Initalize;

            Client.Log += Log;
            
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
            
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private static Task Initalize()
        {
            LevelRoles.CreateRoles(Client);
            EventManager.RegisterEvents(Client);
            EventHandlerRegister.RegisterEventHandlers();

            return Task.CompletedTask;
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
