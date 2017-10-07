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
            CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
            CrossMediaManager.Current.PlayingChanged += Current_PlayingChanged;
        }

        private void Current_PlayingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.PlayingChangedEventArgs e)
        {
            MyMusicSlider.Value = e.Position.Seconds;
            Debug.WriteLine("The position is " + e.Position.Seconds );
        }

        private async void Stop_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Stop();
        }

        private async void Play_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Stop();
            trackIndex = (trackIndex + 1) % 10;
            await CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
