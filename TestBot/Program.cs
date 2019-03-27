using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TestBot.Services;

namespace TestBot
{
    class Program
    {
        private CommandService _commands;
        private DiscordSocketClient _client;

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        // Start the bot
        public async Task StartAsync()
        {
            IServiceCollection services;
            IServiceProvider provider;

            _commands = new CommandService(
                new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose
                }
            );
            _client = new DiscordSocketClient(
                new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                });

            // Configure services
            services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<StartupService>();

            provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);

            provider.GetRequiredService<LoggingService>();
            await provider.GetRequiredService<StartupService>().StartAsync();
            provider.GetRequiredService<CommandHandler>();

            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected! YAY");
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }
    }
}
