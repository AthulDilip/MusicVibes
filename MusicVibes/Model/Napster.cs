using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicVibes.Model
{
    public partial class NapsterModel
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("tracks")]
        public Track[] Tracks { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("returnedCount")]
        public long ReturnedCount { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }
    }

    public partial class Track
    {
        [JsonProperty("formats")]
        public Format[] Formats { get; set; }

        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [JsonProperty("albumName")]
        public string AlbumName { get; set; }

        [JsonProperty("albumId")]
        public string AlbumId { get; set; }

        [JsonProperty("artistId")]
        public string ArtistId { get; set; }

        [JsonProperty("contributors")]
        public Contributors Contributors { get; set; }

        [JsonProperty("blurbs")]
        public object[] Blurbs { get; set; }

        [JsonProperty("disc")]
        public long Disc { get; set; }

        [JsonProperty("isExplicit")]
        public bool IsExplicit { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("isrc")]
        public string Isrc { get; set; }

        [JsonProperty("isStreamable")]
        public bool IsStreamable { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("previewURL")]
        public string PreviewURL { get; set; }

        [JsonProperty("playbackSeconds")]
        public long PlaybackSeconds { get; set; }

        [JsonProperty("shortcut")]
        public string Shortcut { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Format
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bitrate")]
        public long Bitrate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Contributors
    {
        [JsonProperty("primaryArtist")]
        public string PrimaryArtist { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("artists")]
        public Albums Artists { get; set; }

        [JsonProperty("albums")]
        public Albums Albums { get; set; }

        [JsonProperty("genres")]
        public Albums Genres { get; set; }

        [JsonProperty("tags")]
        public Albums Tags { get; set; }
    }

    public partial class Albums
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("ids")]
        public string[] Ids { get; set; }
    }

    public partial class NapsterModel
    {
        public static NapsterModel FromJson(string json) => JsonConvert.DeserializeObject<NapsterModel>(json, ConverterNapster.Settings);
    }

    public static class SerializeNapster
    {
        public static string ToJson(this NapsterModel self) => JsonConvert.SerializeObject(self, ConverterNapster.Settings);
    }

    public class ConverterNapster
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
