using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace TestBot.Services
{
    public class SongService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private IVoiceChannel _voiceChannel;
        private IMessageChannel _messageChannel;
        private BufferBlock<IPlayable> _songQueue;

        public SongService(DiscordSocketClient client, CommandService commands)
        {
            _discord = client;
            _commands = commands;
            _songQueue = new BufferBlock<IPlayable>();
        }





        public AudioPlaybackService AudioPlaybackService { get; set; }

        public IPlayable NowPlaying { get; private set; }

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
            AudioPlaybackService.StopCurrentOperation();
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
                        await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);
                    }

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while playing song: {e}");
                }
            }
        }


        
    }
}