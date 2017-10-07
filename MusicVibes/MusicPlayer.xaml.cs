using Plugin.MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MusicVibes.Model;

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
            ++trackIndex;
            await CrossMediaManager.Current.Play(RandomTracks[trackIndex].PreviewURL);
        }
    }

}
