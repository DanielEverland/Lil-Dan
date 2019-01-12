using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class MentionResponder : IEventHandler
    {
        private const string PLAY_WITH_ME_REGEX_STRING = @"(play with me)";
        
        private List<string> playWithMeResponds = new List<string>()
        {
            "I'll play with you all day, babydoll ( ͡º ͜ʖ ͡º)",
            "Only if you play with me first ( ͡º ͜ʖ ͡º)",
            "( ˘ ³˘)",
        };

        private Regex playWithMeRegex;

        public void Register()
        {
            playWithMeRegex = new Regex(PLAY_WITH_ME_REGEX_STRING, RegexOptions.IgnoreCase);

            EventManager.OnMessageReceived += MessageReceived;
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if(message.MentionedUsers.Any(x => x.Id == Program.User.Id))
            {
                if(playWithMeRegex.IsMatch(message.Content))
                {
                    await message.Channel.SendMessageAsync(playWithMeResponds.Random());
                }
            }
        }
    }    
}

