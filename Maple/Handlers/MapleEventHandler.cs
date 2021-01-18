using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Maple.Handlers
{
    public static class MapleEventHandler
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public static async Task OnReady()
        {
            Console.WriteLine("Maple has successfully logged in!");
            await HttpClient.PostAsync(Maple.MonitorWebhook, new FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    {"content", "Maple is now online!"}
                }
            ));
        }

        public static async Task OnShardReady(DiscordSocketClient shard)
        {
            Console.WriteLine($"Shard #{shard.ShardId} is online!");
            await HttpClient.PostAsync(Maple.MonitorWebhook, new FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    {"content", $"Shard #{shard.ShardId} is online!"}
                }
            ));
        }
    }
}