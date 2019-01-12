using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lil_Dan.Event_Handlers
{
    public class LevelHandler : IEventHandler
    {
        private const int POLL_INTERVAL = 1000;
        //private const int POLL_INTERVAL = 1000 * 60 * 5;

        private static Dictionary<SocketUser, uint> deltaMessageCount = new Dictionary<SocketUser, uint>();

        public void Register()
        {
            EventManager.OnMessageReceived += OnMessageReceived;
        }
        private Task OnMessageReceived(SocketMessage message)
        {
            if(!deltaMessageCount.ContainsKey(message.Author))
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
                foreach (var pair in deltaMessageCount)
                {
                    uint currentCount = await Database.GetMesssageCount(pair.Key.Id);
                    await Database.SetMessageCount(pair.Key.Id, pair.Value + currentCount);
                }

                await Task.Delay(POLL_INTERVAL);
            }
        }
        private static void PollRole(SocketUser user)
        {

        }
    }
}
