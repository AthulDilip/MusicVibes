using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using MusicVibes.Model;

namespace MusicVibes
{
    public static class AzureServices
    {
        public static MobileServiceClient MobileService =
        new MobileServiceClient(
        "https://musicvibes.azurewebsites.net"
    );
        public static async Task<bool> InsertSkip(string UserId, string Emotion, string Genre)
        {
            try
            {
                Skip item = new Skip { userid = UserId, emotion = Emotion, genre = Genre };
                await MobileService.GetTable<Skip>().InsertAsync(item);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
