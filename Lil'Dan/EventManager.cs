using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan
{
    public static class EventManager
    {
        public static event Func<SocketMessage, Task> OnMessageReceived;

        public static void RegisterEvents(DiscordSocketClient client)
        {
            client.MessageReceived += MessageReceived;
        }
        private static async Task MessageReceived(SocketMessage message)
        {
            await OnMessageReceived.Invoke(message);
        }
    }
}
