using MobieBotVK.BotTools.ChatTools;
using System;
using System.Collections.Generic;
using System.Text;


namespace MobieBotVK.BotTools.SessionControl.Commands
{
    public class ChatCommand : ACommand
    {
        public enum CommandGroup
        {
            Moderation = 1,
            Managment,
            Interaction,
            Statistics
        }

        public CommandGroup Group { get; set; }
        public Func<ExecCommand, ChatUser, MessageSend> CommandExecute { get; set; }
    }
}
