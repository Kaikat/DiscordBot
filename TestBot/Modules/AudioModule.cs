using Discord;
using Discord.Commands;
using System.Threading.Tasks;
//using TestBot.Services;

namespace TestBot.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        //private readonly AudioService _service;

        //public AudioModule(AudioService service)
        //{
         //   _service = service;
        //}

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        /*[Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }*/

        /*[Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await ReplyError("You need to be in a voice channel, or pass one as an argument.");
                return;
            }

            audioclient = await channel.ConnectAsync();
        }*/

    }
}