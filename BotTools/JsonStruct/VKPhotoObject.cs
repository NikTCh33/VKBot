using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKPhotoObject
    {
        [JsonProperty("id")]
        public long id { get; set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("user_id")]
        public int user_id { get; set; }

        [JsonProperty("sizes")]
        public Size[] Sizes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("has_tags")]
        public bool HasTags { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        public class Size
        {
            [JsonProperty("height")]
            public long Height { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("width")]
            public long Width { get; set; }
        }


        public string GetHighQualityPhoto()
        {
            return Sizes[Sizes.Length - 1].Url;
        }
    }
}
