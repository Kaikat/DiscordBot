using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace RavenBot.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        
        public StartupService(DiscordSocketClient discord,
            CommandService commands)
        {
            _discord = discord;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            Func<Task> connected;
            Func<Task> loggedIn;
            Func<Task> clientReady;

            connected = () =>
            {
                Console.WriteLine("----");
                Console.WriteLine("Gateway connected");
                Console.WriteLine("----");
                return Task.CompletedTask;
            };
            loggedIn = () =>
            {
                Console.WriteLine("----");
                Console.WriteLine("Logged in");
                Console.WriteLine("----");
                return Task.CompletedTask;
            };

            clientReady = () =>
            {
                Assembly assembly;
                FileVersionInfo fileVersionInfo;
                string version;

                Console.WriteLine("----");
                Console.WriteLine("Client Ready");
                Console.WriteLine("----");

                assembly = Assembly.GetExecutingAssembly();
                fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                version = fileVersionInfo.FileVersion;

                Console.WriteLine($"Version {version}");
                Console.WriteLine("----");
                return Task.CompletedTask;
            };

            _discord.Connected += connected;
            _discord.LoggedIn += loggedIn;
            _discord.Ready += clientReady;

            string tokenFile = Directory.GetCurrentDirectory() + "/Data/BotToken.txt";
            string discordToken = File.ReadAllText(tokenFile).Trim();
            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }
    }
}

























