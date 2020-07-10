using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKChatInfo
    {
        [JsonProperty("response")]
        public Response response { get; set; }

        public class Response
        {
            [JsonProperty("count")]
            public int count;
            [JsonProperty("items")]
            public List<Items> items;
            [JsonProperty("profiles")]
            public List<Profiles> profiles;
            [JsonProperty("groups")]
            public List<Groups> groups;
        }

        public class Items
        {
            [JsonProperty("member_id")]
            public int member_id;
            [JsonProperty("invited_by")]
            public int invited_by;
            [JsonProperty("join_date")]
            public int join_date;
            [JsonProperty("is_admin")]
            public bool is_admin;
            [JsonProperty("is_owner")]
            public bool is_owner;
        }
        public class Profiles
        {
            [JsonProperty("id")]
            public int id;
            [JsonProperty("first_name")]
            public string first_name;
            [JsonProperty("last_name")]
            public string last_name;
            [JsonProperty("screen_name")]
            public string screen_name;
            [JsonProperty("deactivated")]
            public string deactivated;
            [JsonProperty("online")]
            public int online;
            [JsonProperty("photo_50")]
            public string photo_50;
        }
        public class Groups
        {
            [JsonProperty("id")]
            public int id;
        }
    }
}
