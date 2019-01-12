using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Lil_Dan.Event_Handlers;

namespace Lil_Dan
{
    class Program
    {
        public const string USERNAME = "Everland Games Bot";
        public const string DESCRIMINATOR = "2557";

        public static DiscordSocketClient Client { get; private set; }
        public static SocketUser User { get; private set; }

        public static void Main(string[] args)
        {
            string token = args[0];
            string server = args[1];
            string user = args[2];
            string database = args[3];
            string port = args[4];
            string password = args[5];
            
            try
            {
                Database.Start(server, user, database, port, password);
                new Program().MainAsync(token).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }
        
        public async Task MainAsync(string token)
        {
            Client = new DiscordSocketClient();
            Client.Ready += Initalize;

            Client.Log += Log;
            
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await LevelHandler.PollMessageDeltas();
            
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private static Task Initalize()
        {
            User = Client.GetUser(USERNAME, DESCRIMINATOR);
            
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
