using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKMessages
    {
        [JsonProperty("response")]
        public Response response { get; set; }

        public class Response
        {
            [JsonProperty("history")]
            public long[][] history { get; set; }
            [JsonProperty("messages")]
            public Messages messages { get; set; }
            [JsonProperty("profiles")]
            public Profiles[] profiles { get; set; }
            [JsonProperty("new_pts")]
            public string new_pts { get; set; }
        }

        public class Messages
        {
            [JsonProperty("count")]
            public string count { get; set; }
            [JsonProperty("items")]
            public Items[] items { get; set; }

            public class Items
            {
                [JsonProperty("date")]
                public string date { get; set; }
                [JsonProperty("from_id")]
                public int from_id { get; set; }
                [JsonProperty("id")]
                public int id { get; set; }
                [JsonProperty("out")]
                public string _out { get; set; }
                [JsonProperty("peer_id")]
                public int peer_id { get; set; }
                [JsonProperty("conversation_message_id")]
                public string conversation_message_id { get; set; }
                [JsonProperty("fwd_messages")]
                public Items[] fwd_messages { get; set; }
                [JsonProperty("important")]
                public string important { get; set; }
                [JsonProperty("read_state")]
                public string read_state { get; set; }
                [JsonProperty("text")]
                public string text { get; set; }
                [JsonProperty("is_hidden")]
                public string is_hidden { get; set; }
                [JsonProperty("random_id")]
                public string random_id { get; set; }
            }
        }
        public class Profiles
        {
            [JsonProperty("id")]
            public string id { get; set; }
            [JsonProperty("first_name")]
            public string first_name { get; set; }
            [JsonProperty("last_name")]
            public string last_name { get; set; }
            [JsonProperty("sex")]
            public string sex { get; set; } //секас
            [JsonProperty("screen_name")]
            public string screen_name { get; set; }
            [JsonProperty("photo")]
            public string photo { get; set; }
            [JsonProperty("photo_medium_rec")]
            public string photo_medium_rec { get; set; }
            [JsonProperty("online")]
            public string online { get; set; }
        }
    }
}
