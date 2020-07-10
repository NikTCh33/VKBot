using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKSaveMessagesPhoto
    {
        [JsonProperty("response")]
        public VKPhotoObject[] response;

    }
}
