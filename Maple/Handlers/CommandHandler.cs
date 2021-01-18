using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Maple.Handlers
{
    public class CommandHandler
    {
        private readonly IServiceProvider _services;
        private readonly DiscordShardedClient _maple;
        private readonly CommandService _commands;

        public CommandHandler(IServiceProvider services, DiscordShardedClient maple, CommandService commands)
        {
            _services = services;
            _maple = maple;
            _commands = commands;
        }

        public async Task StartCommandHandler()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _maple.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage userMsg)) return;

            var argPos = 0;

            if (!(userMsg.HasStringPrefix("..", ref argPos) ||
                  userMsg.HasMentionPrefix(_maple.CurrentUser, ref argPos)) ||
                userMsg.Author.IsBot) return;

            var ctx = new ShardedCommandContext(_maple, userMsg);
            await _commands.ExecuteAsync(ctx, argPos, _services);
        }
    }
}