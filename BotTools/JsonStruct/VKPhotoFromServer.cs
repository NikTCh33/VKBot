using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKPhotoFromServer
    {
        [JsonProperty("server")]
        public int server;
        [JsonProperty("photo")]
        public string photo;
        [JsonProperty("hash")]
        public string hash;
    }
}
