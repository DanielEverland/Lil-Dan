using Discord.WebSocket;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lil_Dan
{
    public static class LevelRoles
    {
        public static IEnumerable<SocketRole> RoleObjects => roleObjects;

        private static List<ulong> roleIDs = new List<ulong>
        {
            // Level 1
            532656978757943296,
            // Level 5
            532657016565530675,
            // Level 10
            532657044491206659,
            // Level 20
            532657074379685898,
            // Level 50
            533521771802918913,
            // Level 100
            533521809933336626,
        };
        private static SocketRole[] roleObjects = new SocketRole[6];

        public static SocketRole GetRole(int index)
        {
            return roleObjects[index];
        }
        public static void CreateRoles(DiscordSocketClient client)
        {
            foreach (SocketGuild guild in client.Guilds)
            {
                foreach (SocketRole role in guild.Roles)
                {
                    if (roleIDs.Contains(role.Id))
                    {
                        int index = roleIDs.IndexOf(role.Id);
                        roleObjects[index] = role;
                    }
                }
            }

            // Ensure all roles have been assigned
            foreach (ulong roleID in roleIDs)
            {
                if(!roleObjects.Any(x => x.Id == roleID))
                {
                    throw new NullReferenceException($"No role could be found for {roleID}");
                }
            }
        }
    }
}
