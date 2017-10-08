using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MusicVibes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Dictionary<string, double> mydic = new Dictionary<string, double>();
            mydic.Add("Anger", 0.2);
            mydic.Add("Contempt", 0);
            mydic.Add("Disgust", 0);
            mydic.Add("Fear", 0);
            mydic.Add("Happiness", 0);
            mydic.Add("Neutral", 0.5);
            mydic.Add("Sadness", 0);
            mydic.Add("Surprise", 0.1);

            var l = MappingService.getGenreFromMood(mydic);

            MainPage = new NavigationPage(new TakeSelfie())
            {
                BarBackgroundColor = Color.Black,
                
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
