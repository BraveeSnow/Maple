using System;
using Discord.Commands;
using Discord.WebSocket;
using Maple.NHentai;
using Microsoft.Extensions.DependencyInjection;

namespace Maple.Services
{
    public class DependencyInjection
    {
        private readonly DiscordSocketConfig _mapleConfig;
        private readonly CommandServiceConfig _commandsConfig;

        public DependencyInjection(DiscordSocketConfig mapleConfig, CommandServiceConfig commandsConfig)
        {
            _mapleConfig = mapleConfig;
            _commandsConfig = commandsConfig;
        }

        public IServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(_mapleConfig))
                .AddSingleton(new CommandService(_commandsConfig))
                .AddSingleton<NHentaiClient>()
                .BuildServiceProvider();
        }
    }
}