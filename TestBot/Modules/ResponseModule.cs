using Discord.Commands;
using Discord.Net;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace TestBot.Modules
{
    public class ResponseModule : ModuleBase<SocketCommandContext>
    {
        [Command("aboutme")]
        public async Task AboutMe()
        {
            string aboutMeMessage = "Hi! I'm Raven PVM! \n" +
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
            	"\thi\n" +
                "\tehb <rs_player-name>\n";

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

        [Command("ehb")]
        public async Task PlayerInfo()
        {
            await ReplyAsync($"Please provide the runescape player name to the Efficient Hours Bossed command.");
        }

        [Command("ehb")]
        public async Task PlayerInfo(string playername)
        {
            RunescapeHS player = new RunescapeHS(playername);
            if (player.GetBossDataTable().Count == 0)
            {
                await ReplyAsync($"Could not find a player with the name '{playername}'\n");
            }
            else
            {
                List<string> tableParts = player.GetBossDataTable();
                await ReplyAsync($"Hello {playername}\nEHB In Progress\n");

                /*
                var builder = new EmbedBuilder();

                builder.WithTitle("Ice Wizard Stats");
                builder.AddInlineField("Cost", "3");
                builder.AddInlineField("HP", "665");
                builder.AddInlineField("DPS", "42");
                builder.AddInlineField("Hit Speed", "1.5sec");
                builder.AddInlineField("SlowDown", "35%");
                builder.AddInlineField("AOE", "63");
                builder.WithThumbnailUrl("url");

                //builder.WithColor(Color.Red);
                await ReplyAsync("", false, builder.Build());*/

                for(int i = 0; i < tableParts.Count; i++)
                {
                    await ReplyAsync(tableParts[i]);
                }
            }
        }
    }
}
