using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKPhotoServer
    {
        public UploadServer response;

        public class UploadServer
        {
            [JsonProperty("upload_url")]
            public string upload_url;
            [JsonProperty("album_id")]
            public int album_id;
            [JsonProperty("user_id")]
            public int user_id;
            [JsonProperty("group_id")]
            public int group_id;
        }
    }
}
