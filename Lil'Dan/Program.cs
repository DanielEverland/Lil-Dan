using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Lil_Dan
{
    class Program
    {
        public static void Main(string[] args)
        {
            string token = args[0];
            new Program().MainAsync(token).GetAwaiter().GetResult();
        }
             

        public async Task MainAsync(string token)
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

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
