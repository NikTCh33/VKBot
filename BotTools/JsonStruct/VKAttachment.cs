using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKAttachment
    {
        [JsonProperty("type")]
        public string type;
        [JsonProperty("photo")]
        public VKPhotoObject photo;
    }
}
