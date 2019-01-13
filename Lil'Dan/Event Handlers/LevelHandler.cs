using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class LevelHandler : IEventHandler
    {
        private const int POLL_INTERVAL = 1000 * 60 * 5;

        private static Dictionary<SocketUser, uint> deltaMessageCount = new Dictionary<SocketUser, uint>();

        public void Register()
        {
            EventManager.OnMessageReceived += OnMessageReceived;
        }
        private Task OnMessageReceived(SocketMessage message)
        {
            if (!deltaMessageCount.ContainsKey(message.Author))
            {
                deltaMessageCount.Add(message.Author, 0);
            }

            deltaMessageCount[message.Author]++;

            return Task.CompletedTask;
        }

        public static async Task PollMessageDeltas()
        {
            while (true)
            {
                foreach (SocketUser user in new List<SocketUser>(deltaMessageCount.Keys))
                {
                    uint delta = deltaMessageCount[user];

                    if (!await Database.HasProfile(user.Id))
                    {
                        await Database.InsertProfile(user.Id, user.Username, 0);
                    }

                    uint currentCount = await Database.GetMesssageCount(user.Id);
                    uint newCount = currentCount + delta;
                    await Database.SetMessageCount(user.Id, newCount);

                    PollRole(user, newCount);
                }
                deltaMessageCount.Clear();

                await Task.Delay(POLL_INTERVAL);
            }
        }
        private static void PollRole(SocketUser user, uint messageCount)
        {
            int currentIndex = LevelRoles.GetRoleIndexFromUser(user);
            int potentialIndex = LevelRoles.GetRoleIndexFromMessageCount(messageCount);
            
            if(currentIndex != potentialIndex)
            {
                LevelRoles.RemoveAllLevelRoles(user);

                SocketRole role = LevelRoles.GetRoleFromIndex(potentialIndex);

#if DEBUG
                Console.WriteLine($"Adding {role.Name} to {user.Username}");
#endif

                SocketGuildUser guildUser = Program.GetGuildUser(user);
                guildUser.AddRoleAsync(role);
            }
        }
    }
}
