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

namespace MusicVibes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPlayer : ContentPage
    {
        private NapsterModel Emotracks;
        public List<Track> RandomTracks;
        public int trackIndex;

        public MusicPlayer()
        {
            InitializeComponent();

        }

        public MusicPlayer(NapsterModel emotracks)
        {
            InitializeComponent();

            Emotracks = emotracks;

            List<Track> tracks = Emotracks.Tracks.ToList();

            var rnd = new Random();
            RandomTracks = tracks.OrderBy(i => rnd.Next()).ToList();
            trackIndex = 0;

            List<MediaFile> MediaFiles = new List<MediaFile>();
            foreach (var x in RandomTracks)                 MediaFiles.Add(new MediaFile(x.PreviewURL, Plugin.MediaManager.Abstractions.Enums.MediaFileType.Audio, Plugin.MediaManager.Abstractions.Enums.ResourceAvailability.Remote));

            CrossMediaManager.Current.Play(MediaFiles);
            CrossMediaManager.Current.MediaFileChanged += Current_MediaFileChanged;
            CrossMediaManager.Current.PlayingChanged += Current_PlayingChanged;
            CrossMediaManager.Current.MediaFinished += Current_MediaFinished;
        }

        private async void Current_MediaFinished(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFinishedEventArgs e)
        {
            await CrossMediaManager.Current.PlayNext();
        }

        void Current_MediaFileChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFileChangedEventArgs e)         {
            var track = RandomTracks.Where(x => x.PreviewURL.Equals(e.File.Url)).FirstOrDefault();
            AlbumImage.Source = "http://direct.rhapsody.com/imageserver/v2/albums/"+track.AlbumId+"/images/600x600.jpg";
            SongName.Text = track.Name;
        }

        private void Current_PlayingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.PlayingChangedEventArgs e)
        {
            MyMusicSlider.Value = e.Position.Seconds;
        }
        bool isPlaying = true;
        private async void Stop_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Stop();
        }

        private async void Play_Clicked(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                await CrossMediaManager.Current.Pause();
                Play.Source = "button_play";
                isPlaying = false;
            }
            else
            {
                await CrossMediaManager.Current.Play();
                Play.Source = "button_pause";
                isPlaying = true;
            }
            //await CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.PlayNext();
        }
    }

}
