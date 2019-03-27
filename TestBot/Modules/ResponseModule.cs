using Discord.Commands;
using System.Threading.Tasks;

namespace TestBot.Modules
{
    public class ResponseModule : ModuleBase<SocketCommandContext>
    {
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

    }
}
