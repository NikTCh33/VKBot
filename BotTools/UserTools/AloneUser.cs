using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.UserTools
{
    public class AloneUser : DefaultPerson
    {
        [JsonProperty("Level")]
        public int Level;

        public AloneUser(int ID, string shortid,string name, string surname) : base(ID, shortid, name, surname)
        {
            Level = 1;
        }

    }
}
