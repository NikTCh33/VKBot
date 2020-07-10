using System;
using System.Collections.Generic;
using System.Text;
using MobieBotVK.BotTools.SessionControl;
using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.SessionControl.Sessions.User;
using Newtonsoft.Json;

namespace MobieBotVK.BotTools.UserTools
{
    public class UserDialog : DefaultDialog
    {
        [JsonProperty("User")]
        public AloneUser User;

        [JsonProperty("IsMember")]
        public bool IsMember;

        public override MessageSend ExecuteCommand(ExecCommand command, DefaultPerson User)
        {
            return SessionControl.RunCommand(command, User);
        }

        public UserDialog()
        {
            SessionControl = new SLoginUser(this); SessionType = SelectSession.LoginChat;          
        }

        public override bool SetSession(SelectSession session)
        {
            if (session == SelectSession.AloneUser)
            {
                SessionType = session;
                SessionControl = new SAloneUser(this);
                return true;
            }
            else if (session == SelectSession.LoginUser)
            {
                SessionType = session;
                SessionControl = new SLoginUser(this);
                return true;
            }
            else if (session == SelectSession.FormUser)
            {
                SessionType = session;
                SessionControl = new SFormUser(this);
                return true;
            }
            else if (session == SelectSession.AdminUser)
            {
                SessionType = session;
                SessionControl = new SAdminUser(this);
                return true;
            }
            return false;
        }

        public bool CheckSubscribe()
        {
            return true;//Недоработанный функционал
        }
    }
}
