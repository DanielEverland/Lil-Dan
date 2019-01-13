using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
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
                new Program().MainAsync(token, server, user, database, port, password).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
                throw;
            }
        }
        
        public async Task MainAsync(string token, string server, string user, string database, string port, string password)
        {
            while (true)
            {
                Console.WriteLine("");
                Debug.Log("---- STARTING NEW INSTANCE -----");
                Console.WriteLine("");

                try
                {
                    Database.Start(server, user, database, port, password);
                    await RunBot(token);
                }
                catch (Exception e)
                {
                    Debug.Log("---- EXCEPTION THROWN -----");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("");
                }
                finally
                {
                    Database.Close();

                    Debug.Log("Closing client");
                    await Client.StopAsync();

                    Debug.Log("Waiting for connection to close...");
                    while (Client.ConnectionState != ConnectionState.Disconnected) { }
                    Debug.Log("Connection closed");
                }
            }
        }
        private async Task RunBot(string token)
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
            Debug.Log(msg.ToString());
            return Task.CompletedTask;
        }
        public static SocketGuildUser GetGuildUser(SocketUser user)
        {
            foreach (SocketGuild guild in Client.Guilds)
            {
                var value = guild.Users.FirstOrDefault(x => x.Id == user.Id);

                if (value != default(SocketGuildUser))
                    return value;
            }

            throw new System.NullReferenceException();
        }
    }
}
