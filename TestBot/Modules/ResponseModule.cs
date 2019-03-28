using Discord.Commands;
using System.Threading.Tasks;

namespace TestBot.Modules
{
    public class ResponseModule : ModuleBase<SocketCommandContext>
    {
        [Command("aboutme")]
        public async Task AboutMe()
        {
            string aboutMeMessage = "Hi! I'm <BotName>! \n" +
                "I was created by Kaikat on Saturday, March 23, 2019.\n" +
                "I am a work in progress so if you have any new feature requests or ideas, let her know.";
            await ReplyAsync(aboutMeMessage);
        }

        [Command("help")]
        public async Task Help()
        {
            string helpMessage = "I look at any commands that start with an !\n" +
            	"Here are the current commands that I respond to:\n" +
            	"\taboutme\n" +
            	"\tcookie\n" +
            	"\tcookie <username>\n" +
            	"\thello\n" +
            	"\thi\n";

            await ReplyAsync(helpMessage);
        }

        /// <summary>
        /// Gives a cookie to the current user.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [Command("cookie")]
        public async Task GiveCookie()
        {
            await ReplyAsync($"Here {Context.User.Mention}, have a cookie :cookie:");
        }

        /// <summary>
        /// Gives a cookie to the tagged user.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        /// <param name="username">The user to give a cookie to.</param>
        [Command("cookie")]
        public async Task GiveCookie(string username)
        {
            await ReplyAsync($"Here {username}, have a cookie :cookie:");
        }

        [Command("hello")]
        public async Task HelloWorld()
        {
            await ReplyAsync($"Hello World!!!!");
        }

        [Command("hi")]
        public async Task Hi()
        {
            await ReplyAsync($"Hi {Context.User.Mention}");
        }

        [Command("redact")]
        public async Task Redact()
        {
            await ReplyAsync($"{Context.User.Mention} redacted or attempted to redact a comment!");
        }
    }
}
