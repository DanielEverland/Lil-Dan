using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class MmmmhhReplier : IEventHandler
    {
        private const string MMMMMHHHHH_TEXT = "( ͡º ͜ʖ ͡º)";

        public void Register()
        {
            EventManager.OnMessageReceived += MessageReceived;
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.Id == Program.User.Id)
                return;

            if (message.Content.Contains(MMMMMHHHHH_TEXT))
            {
                await message.Channel.SendMessageAsync("Mmmhhhh ( ͡º ͜ʖ ͡º)");
            }
        }
    }
}
