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
        private static List<uint> requiredMessages = new List<uint>()
        {
            // Level 1
            0,
            // Level 5
            25,
            // Level 10
            100,
            // Level 20
            400,
            // Level 50
            2500,
            // Level 100
            10000,
        };
        private static SocketRole[] roleObjects = new SocketRole[6];
        
        public static int GetRoleIndexFromMessageCount(uint messageCount)
        {
            for (int i = requiredMessages.Count - 1; i >= 0; i--)
            {
                if(messageCount >= requiredMessages[i])
                {
                    return i;
                }
            }

            return 0;
        }
        public static int GetRoleIndexFromUser(SocketUser user)
        {
            SocketGuildUser guildUser = Program.GetGuildUser(user);

            foreach (SocketRole role in guildUser.Roles)
            {
                if(roleIDs.Contains(role.Id))
                {
                    return roleIDs.IndexOf(role.Id);
                }
            }

            return -1;
        }
        public static SocketRole GetRoleFromIndex(int index)
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
                if (!roleObjects.Any(x => x.Id == roleID))
                {
                    throw new NullReferenceException($"No role could be found for {roleID}");
                }
            }
        }
        public static void RemoveAllLevelRoles(SocketUser user)
        {
            SocketGuildUser guildUser = Program.GetGuildUser(user);

            foreach (SocketRole role in guildUser.Roles)
            {
                if (roleIDs.Contains(role.Id))
                {
                    guildUser.RemoveRoleAsync(role);
                }
            }
        }
    }
}
