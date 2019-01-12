using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class PongReplier : IEventHandler
    {
        public void Register()
        {
            EventManager.OnMessageReceived += MessageReceived;
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
        }
    }
}
