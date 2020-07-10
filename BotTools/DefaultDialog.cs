using MobieBotVK.BotTools.SessionControl;
using MobieBotVK.BotTools.SessionControl.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools
{
    public abstract class DefaultDialog
    {
        [JsonProperty("SessionType")]
        public SelectSession SessionType;
        [JsonIgnore]
        public ASession SessionControl;
        public abstract MessageSend ExecuteCommand(ExecCommand command, DefaultPerson User);
        public abstract bool SetSession(SelectSession session);
    }
}
