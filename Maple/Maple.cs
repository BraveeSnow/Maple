using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Maple.Handlers;
using Maple.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Maple
{
    internal class Maple
    {
        public static readonly string MonitorWebhook = Environment.GetEnvironmentVariable("MONITOR_WH");

        private readonly DiscordSocketConfig _mapleConfig = new()
        {
            LogLevel = LogSeverity.Warning,
            TotalShards = 2
        };

        private readonly CommandServiceConfig _commandsConfig = new()
        {
            CaseSensitiveCommands = false,
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Warning
        };

        private static void Main(string[] args)
        {
            Console.WriteLine("Maple is now initializing...");
            new Maple().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            var services = new DependencyInjection(
                _mapleConfig,
                _commandsConfig
            ).GetServiceProvider();

            var maple = services.GetRequiredService<DiscordShardedClient>();
            var commands = services.GetRequiredService<CommandService>();

            // Event handling
            maple.Log += MapleEventHandler.Log;
            maple.LoggedIn += MapleEventHandler.OnReady;
            maple.ShardReady += MapleEventHandler.OnShardReady;
            commands.Log += MapleEventHandler.Log;

            var commandHandler = new CommandHandler(
                services,
                maple,
                commands
            );
            await commandHandler.StartCommandHandler();

            await maple.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
            await maple.StartAsync();
            await Task.Delay(-1);
        }
    }
}