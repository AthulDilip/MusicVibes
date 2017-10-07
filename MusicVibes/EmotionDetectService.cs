using MusicVibes.Model;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MusicVibes
{
    public class EmotionDetectService
    {
        public static string BaseUrl = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/";

        /// <summary>
        /// The API key for using Microsoft Cognitive Service Emotion API (Can be obtained from Azure)
        /// </summary>
        public static string ApiKey = "75c19e35e1534bdea882335c003ed868";

        /// <summary>
        /// Uploads the photo and get response from Microsoft Cognitive Service
        /// </summary>
        /// <returns>The photo.</returns>
        /// <param name="file">File.</param>
        async public static Task<Face[]> UploadPhoto(MediaFile file)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Add("ocp-apim-subscription-key", ApiKey);
                    StreamContent streamContent = new StreamContent(file.GetStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var result = await client.PostAsync("recognize", streamContent);
                    if (!result.IsSuccessStatusCode)
                        return null;
                    else return Faces.FromJson(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                //keeping it simple
                //if you want handle each exception and show it in UI
                return null;
            }
        }
    }
}