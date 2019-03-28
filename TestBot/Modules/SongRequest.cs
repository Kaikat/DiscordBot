using Discord.Commands;
using TestBot.Services;
using TestBot.Services.YouTube;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Discord;

namespace TestBot.Modules
{
    public class SongRequest : ModuleBase
    {
        /*public YouTubeDownloadService YoutubeDownloadService
        {
            get { return youtubeDownloadService; }
            set { youtubeDownloadService = value;  }
        }
        private YouTubeDownloadService youtubeDownloadService;*/
        // public SongService SongService { get; set; }


        /* public SongService SongService
       {
           get { return songService; }
           set { songService = value; }
       }*/
        //private SongService songService;
        //_songQueue = new BufferBlock<IPlayable>();

        private IVoiceChannel _voiceChannel;
        private IMessageChannel _messageChannel;
        private BufferBlock<IPlayable> _songQueue = new BufferBlock<IPlayable>();



        [Alias("sq", "request", "play")]
        [Command("songrequest", RunMode = RunMode.Async)]
        [Summary("Requests a song to be played")]
        public async Task Request([Remainder, Summary("URL of the video to play")] string url)
        {
            //await ReplyAsync("From AudioModule 1");
            await Speedrun(url, 48);
        }

        [Alias("test")]
        [Command("soundtest", RunMode = RunMode.Async)]
        [Summary("Performs a sound test")]
        public async Task SoundTest()
        {
            //await ReplyAsync("From AudioModule 2");
            await Request("https://www.youtube.com/watch?v=i1GOn7EIbLg");
        }

        [Command("speedrun", RunMode = RunMode.Async)]
        [Summary("Performs a sound test")]
        public async Task Speedrun(string url, int speedModifier)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid song URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to download {url}");



                var video = await DownloadVideo(url);





                await downloadAnnouncement.DeleteAsync();

                if (video == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to queue song, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                video.Requester = Context.User.Mention;
                video.Speed = speedModifier;

                await ReplyAsync($"{Context.User.Mention} queued **{video.Title}** | `{TimeSpan.FromSeconds(video.Duration)}` | {url}");

                //songService.Queue(video); //////////
                //Queue(video);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing song requet: {e}");
            }
        }

        [Command("stream", RunMode = RunMode.Async)]
        [Summary("Streams a livestream URL")]
        public async Task Stream(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to open {url}");
                var stream = await GetLivestreamData(url);
                await downloadAnnouncement.DeleteAsync();

                if (stream == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to open live stream, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                stream.Requester = Context.User.Mention;
                stream.Url = url;

                Console.WriteLine("Attempting to stream {@Stream}", stream);

                await ReplyAsync($"{Context.User.Mention} queued **{stream.Title}** | `{stream.DurationString}` | {url}");

                //songService.Queue(stream); ///////
                //Queue(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing song requet: {e}");
            }
        }

        /*
    [Command("clear")]
    [Summary("Clears all songs in queue")]
    public async Task ClearQueue()
    {
        //SongService.Clear();
            Clear();
        await ReplyAsync("Queue cleared");
    }

    [Alias("next", "nextsong")]
    [Command("skip")]
    [Summary("Skips current song")]
    public async Task SkipSong()
    {
       // SongService.Next();
            Next();
        await ReplyAsync("Skipped song");
    }

    [Alias("np", "currentsong", "songname", "song")]
    [Command("nowplaying")]
    [Summary("Prints current playing song")]
    public async Task NowPlaying()
    {
        //if (SongService.NowPlaying == null)
        if(NowPlaying == null)
        {
            await ReplyAsync($"{Context.User.Mention} current queue is empty");
        }
        else
        {
            await ReplyAsync($"{Context.User.Mention} now playing `{SongService.NowPlaying.Title}` requested by {SongService.NowPlaying.Requester}");
        }
    }*/










        // YouTubeDownloadService Code here:
        public async Task<DownloadedVideo> DownloadVideo(string url)
        {
            var filename = Guid.NewGuid();

            var youtubeDl = StartYoutubeDl(
                $"-o Songs/{filename}.mp3 --restrict-filenames --extract-audio --no-overwrites --print-json --audio-format mp3 " +
                url);

            if (youtubeDl == null)
            {
                Console.WriteLine("Error: Unable to start process");
                return null;
            }

            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<DownloadedVideo>(jsonOutput);
        }

        public async Task<StreamMetadata> GetLivestreamData(string url)
        {
            var youtubeDl = StartYoutubeDl("--print-json --skip-download " + url);
            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<StreamMetadata>(jsonOutput);
        }

        private static Process StartYoutubeDl(string arguments)
        {
            var youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "youtube-dl",
                Arguments = arguments
            };

            Console.WriteLine($"Starting youtube-dl with arguments: {youtubeDlStartupInfo.Arguments}");
            return Process.Start(youtubeDlStartupInfo);
        }



















        /*
        //public AudioPlaybackService AudioPlaybackService { get; set; }

        //public IPlayable NowPlaying { get; private set; }

        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this._voiceChannel = voiceChannel;
            ProcessQueue();
        }

        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            this._messageChannel = messageChannel;
        }

        public void Next()
        {
            //AudioPlaybackService.StopCurrentOperation();
            Console.WriteLine("NEXT - implement me");
        }

        public IList<IPlayable> Clear()
        {
            _songQueue.TryReceiveAll(out var skippedSongs);

            Console.WriteLine($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public void Queue(IPlayable video)
        {
            _songQueue.Post(video);
        }

        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Console.WriteLine("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    await _messageChannel?.SendMessageAsync($"Now playing **{NowPlaying.Title}** | `{NowPlaying.DurationString}` | requested by {NowPlaying.Requester} | {NowPlaying.Url}");

                    Console.WriteLine("Connecting to voice channel");
                    using (var audioClient = await _voiceChannel.ConnectAsync())
                    {
                        Console.WriteLine("Connected!");
                        //await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);
                        await Task.CompletedTask;
                    }

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while playing song: {e}");
                }
            }
        }*/
    }

}
























//using Discord;
//using Discord.Commands;
//using System.Threading.Tasks;
//using TestBot.Services;

//namespace TestBot.Modules
//{
//public class AudioModule : ModuleBase<ICommandContext>
//{
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

//  }
//}