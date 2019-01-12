using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class SetRoleForNewMembers : IEventHandler
    {
        public void Register()
        {
            EventManager.OnUserJoined += OnUserJoined;
        }
        private async Task OnUserJoined(SocketGuildUser user)
        {
            await user.AddRoleAsync(LevelRoles.GetRole(0));
        }
    }
}
