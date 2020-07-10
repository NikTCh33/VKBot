using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Commands
{
    public class UserCommand : ACommand
    {
        public Func<ExecCommand, AloneUser, MessageSend> CommandExecute { get; set; }
    }
}
