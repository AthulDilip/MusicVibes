using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicVibes.Model
{
    public partial class Face
    {
        [JsonProperty("faceRectangle")]
        public FaceRectangle FaceRectangle { get; set; }

        [JsonProperty("scores")]
        public Scores Scores { get; set; }
    }

    public partial class FaceRectangle
    {
        [JsonProperty("left")]
        public long Left { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("top")]
        public long Top { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class Scores
    {
        [JsonProperty("fear")]
        public double Fear { get; set; }

        [JsonProperty("contempt")]
        public double Contempt { get; set; }

        [JsonProperty("anger")]
        public double Anger { get; set; }

        [JsonProperty("disgust")]
        public double Disgust { get; set; }

        [JsonProperty("neutral")]
        public double Neutral { get; set; }

        [JsonProperty("happiness")]
        public double Happiness { get; set; }

        [JsonProperty("sadness")]
        public double Sadness { get; set; }

        [JsonProperty("surprise")]
        public double Surprise { get; set; }
    }

    public partial class Faces
    {
        public static Face[] FromJson(string json) => JsonConvert.DeserializeObject<Face[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Face[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
