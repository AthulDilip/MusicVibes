using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicVibes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakeSelfie : ContentPage
    {
        public TakeSelfie()
        {
            InitializeComponent();
        }
        MediaFile file;
        const string CheckEmotion = "Check my Emotion";
        const string TakePhoto = "Take a Photo";
        private void Shutter_Clicked(object sender, EventArgs e)
        {
            switch (Shutter.Text)
            {
                case CheckEmotion:
                    CheckMyEmotion();
                    break;
                case TakePhoto:
                    TakeMyPhoto();
                    break;
            }
        }
        async public void TakeMyPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            spinner.IsVisible = true;
            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                CompressionQuality = 80,
                CustomPhotoSize = 40

            });
            Shutter.Text = CheckEmotion;
            if (file == null)
                return;
            spinner.IsVisible = false;
            Shutter.Text = CheckEmotion;
            MyPhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

        }
        async public void CheckMyEmotion()
        {
            Shutter.Text = TakePhoto;
            if (file == null)
            {
                await DisplayAlert("Error", "No Image Taken", "Ok");
                return;
            }
            spinner.IsVisible = true;

            var result = await EmotionDetectService.UploadPhoto(file);
            spinner.IsVisible = false;
            if (result == null)
            {
                await DisplayAlert("Error", "Unable to send photo to the service", "Ok");
                return;
            }
            //Going to Display the result as alert, if multiple faces selecting the first major one from api
            var Face = result.FirstOrDefault();
            if (Face != null)
            {
                var emotion = "Anger: " + Face.Scores.Anger.ToString("0.0")
                        + "\n Contempt:" + Face.Scores.Contempt.ToString("0.0")
                        + "\n Disgust:" + Face.Scores.Disgust.ToString("0.0")
                        + "\n Fear:" + Face.Scores.Fear.ToString("0.0")
                        + "\n Happiness:" + Face.Scores.Happiness.ToString("0.0")
                        + "\n Neutral:" + Face.Scores.Neutral.ToString("0.0")
                        + "\n Sadness:" + Face.Scores.Sadness.ToString("0.0")
                        + "\n Surprise:" + Face.Scores.Surprise.ToString("0.0");

                Dictionary<string, double> mydic = new Dictionary<string, double>();
                mydic.Add("Anger", Face.Scores.Anger);
                mydic.Add("Contempt", Face.Scores.Contempt);
                mydic.Add("Disgust", Face.Scores.Disgust);
                mydic.Add("Fear", Face.Scores.Fear);
                mydic.Add("Happiness", Face.Scores.Happiness);
                mydic.Add("Neutral", Face.Scores.Neutral);
                mydic.Add("Sadness", Face.Scores.Sadness);
                mydic.Add("Surprise", Face.Scores.Surprise);
                double maxVal = mydic.Values.Max();
                var maxValkey = mydic.Where(x => x.Value.Equals(maxVal)).FirstOrDefault();
                string MaxEmotion = maxValkey.Key;

                string Mood = CategoryConstValues.Pop;
                if (MaxEmotion != null)
                {
                    if (MaxEmotion == "Anger")
                    {
                        Mood = CategoryConstValues.Rap;
                    }
                    else if (MaxEmotion == "Contempt")
                    {
                        Mood = CategoryConstValues.Rock;
                    }
                    else if (MaxEmotion == "Disgust")
                    {
                        Mood = CategoryConstValues.Peace;
                    }
                    else if (MaxEmotion == "Fear")
                    {
                        Mood = CategoryConstValues.Peace;
                    }
                    else if (MaxEmotion == "Happiness")
                    {
                        Mood = CategoryConstValues.Acoustics;
                    }
                    else if (MaxEmotion == "Neutral")
                    {
                        Mood = CategoryConstValues.Jazz;
                    }
                    else if (MaxEmotion == "Sadness")
                    {
                        Mood = CategoryConstValues.Slow;
                    }
                    else if (MaxEmotion == "Surprise")
                    {
                        Mood = CategoryConstValues.Fast;
                    }

                }

                await DisplayAlert("Results are out!", emotion, MaxEmotion);
                var Emotracks = await NapsterService.GetEmoTracks(Mood);
                await Navigation.PushAsync(new MusicPlayer(Emotracks));
            }
            else
            {
                await DisplayAlert("Oops", "No Face Detected!", "OK");
            }

        }
    }
}