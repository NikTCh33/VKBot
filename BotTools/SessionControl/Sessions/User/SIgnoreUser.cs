using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Sessions.User
{
    public class SIgnoreUser : ASession
    {
        public List<UserCommand> CommandList { get; set; }

        public UserDialog udialog;

        public SIgnoreUser(UserDialog _dialog)
        {
            udialog = _dialog;
            CommandList = new List<UserCommand>();
        }

        public override MessageSend RunCommand(ExecCommand command, DefaultPerson u)
        {
            return null;
        }


    }
}
