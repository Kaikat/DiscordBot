using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace RavenBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RavenBot.Services.CommandHandler"/> class.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="commands">Commands.</param>
        /// <param name="provider">Provider.</param>
        public CommandHandler(DiscordSocketClient client,
            CommandService commands, IServiceProvider provider)
        {
            _client = client;
            _commands = commands;
            _provider = provider;
            client.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            SocketUserMessage message;
            message = socketMessage as SocketUserMessage;
            if (message == null)
            {
                Console.WriteLine("The message was null");
                return;
            }
            if (message.Author == _client.CurrentUser)
            {
                Console.WriteLine("The author was me");
                return;
            }

            SocketCommandContext context;
            int argPos = 0;
            context = new SocketCommandContext(_client, message);
            if (message.HasStringPrefix("!", ref argPos) || 
                message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                if (!string.Equals(message.Content.Substring(1, 1), "!"))
                {
                    IResult result;
                    Console.WriteLine(message.Content);
                    result = await _commands.ExecuteAsync(context, argPos, _provider);
                    if (!result.IsSuccess)
                    {
                        //EmbedBuilder embed;
                        //embed = new EmbedBuilder();
                        //embed.WithColor(Color.DarkRed);
                        //embed.AddField(":warning: An unexpected error occurred.", $"The command '{message.Content}' is not a registered command");

                        //await context.Channel.SendMessageAsync("", embed: embed.Build());
                    }
                }
            }
        }
    }
}











