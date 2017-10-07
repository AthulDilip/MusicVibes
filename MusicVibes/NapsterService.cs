using MusicVibes.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MusicVibes
{
    public class NapsterService
    {
        //http://api.napster.com/v2.2/playlists/mp.201666116/tracks?apikey=YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4&limit=2
        const string BaseUrl = "http://api.napster.com/v2.2/playlists/mp.201666116/tracks?";
        const string ApiKey = "YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4";

        async public static Task<NapsterModel> GetEmoTracks(string emotion)
        {
            
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync("http://api.napster.com/v2.2/playlists/"+emotion+"/tracks?apikey=YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4&limit=10");
                string NapsterJson = await response.Content.ReadAsStringAsync();
                NapsterModel napster = new NapsterModel();
                if (NapsterJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    napster = JsonConvert.DeserializeObject<NapsterModel>(NapsterJson);
                }
                //Binding listview with server response    
                return napster;
           
        }  
            
    }
}



   
    

    

