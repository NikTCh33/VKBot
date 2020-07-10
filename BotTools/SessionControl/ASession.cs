using System;
using System.Collections.Generic;
using System.Text;
using MobieBotVK.BotTools.SessionControl.Commands;

namespace MobieBotVK.BotTools.SessionControl
{
    public enum SelectSession
    {
        LoginChat = 1,
        IgnoreUser,
        LoginUser,
        FormUser,
        AdminChat,
        AdminUser,
        AloneUser,
        TortureChat
    }

    public abstract class ASession
    {

        public ASession()
        {

        }

        public abstract MessageSend RunCommand(ExecCommand cmd, DefaultPerson u);

        public static string SessionString(SelectSession session)
        {
            if (session == SelectSession.AdminChat)
                return "Администрирование чата";
            else if (session == SelectSession.AloneUser)
                return "Обычный юзер";
            else if (session == SelectSession.FormUser)
                return "Опрос юзера";
            else if (session == SelectSession.IgnoreUser)
                return "Игнор юзера";
            else if (session == SelectSession.LoginChat)
                return "Логин чата";
            else if (session == SelectSession.LoginUser)
                return "Логин юзера";
            else if (session == SelectSession.AdminUser)
                return "Админ юзера";
            else if (session == SelectSession.TortureChat)
                return "Пытка в чате";
            else
                return "Не удалось найти сессию";
        }
    }
}
