using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKUserInfo
    {

        [JsonProperty("response")]
        public List<VKChatInfo.Profiles> response;    
    }
}
