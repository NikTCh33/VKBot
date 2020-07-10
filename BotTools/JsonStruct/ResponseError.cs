using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class ResponseError
    {
        [JsonProperty("error")]
        public Error error { get; set; }

        public class RequestParam
        {
            [JsonProperty("key")]
            public string key { get; set; }
            [JsonProperty("value")]
            public string value { get; set; }
        }

        public class Error
        {
            [JsonProperty("error_code")]
            public int error_code { get; set; }
            [JsonProperty("error_msg")]
            public string error_msg { get; set; }
            [JsonProperty("request_params")]
            public List<RequestParam> request_params { get; set; }
        }
    }
}
