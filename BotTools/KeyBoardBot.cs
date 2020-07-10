using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools
{
    public class KeyBoardBot
    {
        [JsonProperty("one_time")]
        public bool? one_time;
        [JsonProperty("buttons")]
        public List<List<ButtonBot>> buttons;
        [JsonProperty("inline")]
        public bool? inline;
        public KeyBoardBot()
        {
            //one_time = true;
            buttons = new List<List<ButtonBot>>();
        }
    }

    public class ButtonBot
    {
        [JsonProperty("action")]
        public ButtonAction action;
        [JsonProperty("color")]
        public string color;

        public enum ButtonColor
        {
            primary = 0,
            secondary,
            negative,
            positive
        }
    }

    public class ButtonAction
    {
        [JsonProperty("type")]
        public string type;
        [JsonProperty("label")]
        public string label;
        [JsonProperty("payload")]
        public string payload;
    }

    public class Payload
    {
        [JsonProperty("command")]
        public string command;
    }
}
