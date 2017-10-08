using Plugin.MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicVibes.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Abstractions.EventArguments;
using Plugin.MediaManager.Abstractions;

namespace MusicVibes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPlayer : ContentPage
    {
        private NapsterModel Emotracks;
        public List<Track> RandomTracks;
        public int nextClickedCount;

        public MusicPlayer()
        {
            InitializeComponent();

        }
        private IMediaManager mediaManager;
        public MusicPlayer(NapsterModel emotracks)
        {
            InitializeComponent();
            Emotracks = emotracks;
            List<Track> tracks = Emotracks.Tracks.ToList();
            nextClickedCount = 0;
            var rnd = new Random();
            RandomTracks?.Clear();
            RandomTracks = tracks.OrderBy(i => rnd.Next()).ToList();
            mediaManager = CrossMediaManager.Current;
            mediaManager.MediaQueue.Clear();
            List<MediaFile> MediaFiles = new List<MediaFile>();
            foreach (var x in RandomTracks)                 MediaFiles.Add(new MediaFile(x.PreviewURL));

            mediaManager.Play(MediaFiles);
            mediaManager.MediaFileChanged += Current_MediaFileChanged;
            mediaManager.PlayingChanged += Current_PlayingChanged;
            mediaManager.MediaFinished += Current_MediaFinished;
        }

        private async void Current_MediaFinished(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFinishedEventArgs e)
        {
            await mediaManager?.PlayNext();
        }

        void Current_MediaFileChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFileChangedEventArgs e)         {
            var track = RandomTracks.Where(x => x.PreviewURL.Equals(e.File.Url)).FirstOrDefault();
            AlbumImage.Source = "http://direct.rhapsody.com/imageserver/v2/albums/" + track.AlbumId + "/images/600x600.jpg";
            SongName.Text = track.Name;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return true;
        }

        private void Current_PlayingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.PlayingChangedEventArgs e)
        {
            MyMusicSlider.Value = e.Position.Seconds;
        }
        bool isPlaying = true;


        private async void Stop_Clicked(object sender, EventArgs e)
        {
            await mediaManager?.Stop();
        }

        private async void Play_Clicked(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                await mediaManager?.Pause();
                Play.Source = "button_play";
                isPlaying = false;
            }
            else
            {
                await mediaManager?.Play();
                Play.Source = "button_pause";
                isPlaying = true;
            }
            //await CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            if (mediaManager.AudioPlayer.Position.Seconds < 10)
            {
                if (nextClickedCount >= 3)
                {

                    nextClickedCount = 0;
                    GlobalConstValue.gGenreIndex = (GlobalConstValue.gGenreIndex + 1) % GlobalConstValue.gGenreList.Count();
                    string newGenre = GlobalConstValue.gGenreList[GlobalConstValue.gGenreIndex];

                    var newTracks = await NapsterService.GetEmoTracks(newGenre);

                    await mediaManager?.Stop();

                    List<Track> tracks = newTracks.Tracks.ToList();

                    var rnd = new Random();
                    RandomTracks = tracks.OrderBy(i => rnd.Next()).ToList();
                    mediaManager = CrossMediaManager.Current;
                    List<MediaFile> MediaFiles = new List<MediaFile>();
                    foreach (var x in RandomTracks)
                        MediaFiles.Add(new MediaFile(x.PreviewURL));

                    await AzureServices.InsertSkip("2", GlobalConstValue.gMood, newGenre);

                    await mediaManager.Play(MediaFiles);

                }
                else
                {
                    ++nextClickedCount;
                    await mediaManager?.PlayNext();
                }

            }
            else
            {
                await mediaManager?.PlayNext();
            }

        }

        private async void Prev_Clicked(object sender, EventArgs e)
        {
            await mediaManager?.PlayPrevious();
        }
    }

}
