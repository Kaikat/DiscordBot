namespace RavenBot
{
    /*
    public class MyBot
    {
        private DiscordSocketClient client;
        private CommandService commands;

        static void Main(string[] args) => new MyBot().Start().GetAwaiter().GetResult();

        public MyBot()
        {

        }

        // Start the bot
        public async Task Start()
        {
            IServiceCollection services;
            IServiceProvider provider;

            client = new DiscordSocketClient(
                new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                });

            commands = new CommandService(
                new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose
                }
            );

            //commands.CreateModuleAsync("hello", 

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<CommandHandler>();
            provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            provider.GetRequiredService<CommandHandler>();

            await client.LoginAsync(TokenType.Bot, 
                "");
            await client.StartAsync();
            //await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            client.Ready += () =>
            {
                Console.WriteLine("Bot is connected! YAY");
                return Task.CompletedTask;
            };




            await Task.Delay(-1);
        }
    }*/
}
