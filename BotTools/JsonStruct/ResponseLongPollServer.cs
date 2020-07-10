using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class ResponseLongPollServer
    {
        public ParamLongPollServer response { get; set; }

        public class ParamLongPollServer
        {
            public string key { get; set; }
            public string server { get; set; }
            public string ts { get; set; }
        }



        public class Update
        {
            [JsonProperty("type")]
            public string _type { get; set; }
            [JsonProperty("object")]
            public GettedObject _object { get; set; }
            [JsonProperty("group_id")]
            public int group_id { get; set; }

            public class GettedObject
            {
                [JsonProperty("message")]
                public Message message;
                [JsonProperty("client_info")]
                public object client_info;
            }

            public class Message
            {
                [JsonProperty("date")]
                public int date { get; set; }
                [JsonProperty("from_id")]
                public int from_id { get; set; }
                [JsonProperty("id")]
                public int id { get; set; }
                [JsonProperty("out")]
                public int _out { get; set; }
                [JsonProperty("peer_id")]
                public int peer_id { get; set; }
                [JsonProperty("chat_id")]
                public int chat_id { get; set; }
                [JsonProperty("text")]
                public string text { get; set; }
                [JsonProperty("conversation_message_id")]
                public int conversation_message_id { get; set; }
                [JsonProperty("fwd_messages")]
                public List<object> fwd_messages { get; set; }
                [JsonProperty("important")]
                public bool important { get; set; }
                [JsonProperty("random_id")]
                public int random_id { get; set; }
                [JsonProperty("attachments")]
                public List<VKAttachment> attachments { get; set; }
                [JsonProperty("is_hidden")]
                public bool is_hidden { get; set; }
                [JsonProperty("reply_message")]
                public Message reply_message { get; set; }
                [JsonProperty("payload")]
                public string payload { get; set; }

                [JsonProperty("action")]
                public MessageAction action;

                public class MessageAction
                {
                    [JsonProperty("type")]
                    public string type;
                    public int member_id;
                }
            }
        }
        public class ResultServer
        {
            [JsonProperty("ts")]
            public string ts { get; set; }
            [JsonProperty("updates")]
            public List<Update> updates { get; set; }
        }
    }
}
