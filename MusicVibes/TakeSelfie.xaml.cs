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
        private async void Shutter_Clicked(object sender, EventArgs e)
        {
            await TakeMyPhoto();
            await CheckMyEmotion();
        }
        async public Task TakeMyPhoto()
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
            if (file == null)
                return;
            spinner.IsVisible = false;
            instruction.IsVisible = false;
            Loading.IsVisible = true;
            //MyPhoto.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    return stream;
            //});

        }
        async public Task CheckMyEmotion()
        {
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

                List<string> genreList = MappingService.getGenreFromMood(mydic);

                GlobalConstValue.gGenreList = genreList;
                GlobalConstValue.gGenreIndex = 0;

                // await DisplayAlert("Your mood", emotion, "OK");
                var Emotracks = await NapsterService.GetEmoTracks(genreList.FirstOrDefault());
                await Navigation.PushAsync(new MusicPlayer(Emotracks));
            }
            else
            {
                await DisplayAlert("Oops", "No Face Detected!", "OK");
            }
        }
    }
}