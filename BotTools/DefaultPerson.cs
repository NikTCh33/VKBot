using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MobieBotVK.BotTools
{
    public class DefaultPerson
    {
        [JsonProperty("UserID")]
        public int UserID;

        [JsonProperty("ShortID")]
        public string ShortID;

        [JsonProperty("Name")]
        public string Name;

        [JsonProperty("Surname")]
        public string Surname;

        public DefaultPerson(int ID, string shortid, string name, string surname)
        {
            UserID = ID;
            ShortID = shortid;
            Name = name;
            Surname = surname;
        }

        public DefaultPerson()
        {
        }
    }
}
