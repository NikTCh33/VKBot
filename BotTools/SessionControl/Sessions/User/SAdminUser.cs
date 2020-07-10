using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Sessions.User
{
    class SAdminUser : ASession
    {
        public List<UserCommand> CommandList { get; set; }

        public UserDialog udialog;

        public SAdminUser(UserDialog _dialog)
        {
            udialog = _dialog;
            CommandList = new List<UserCommand>();

            CommandList.Add(new UserCommand
            {
                CommandName = "сессия=",
                AboutCommand = "устанавливает сессию выбранному пользователю",
                RightValue = 9,
                CommandExecute = SetSessionForUser
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "перейти",
                AboutCommand = "Переход к выбранной сессии",
                RightValue = 9,
                CommandExecute = TransitionSession
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "чаты",
                AboutCommand = "Показывает все чаты",
                RightValue = 10,
                CommandExecute = ShowAllChats
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "юзеры",
                AboutCommand = "Показывает всех юзеров",
                RightValue = 10,
                CommandExecute = ShowAllUsers
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "уровень=",
                AboutCommand = "Показывает всех юзеров",
                RightValue = 9,
                CommandExecute = SetLevelForUser
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "статус",
                AboutCommand = "Статус",
                RightValue = 9,
                CommandExecute = Status
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "вопросы",
                AboutCommand = "Показывает список текущих вопросов",
                RightValue = 9,
                CommandExecute = ShowQuests
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "новыевопросы",
                AboutCommand = "Устанавливает вопросы",
                RightValue = 9,
                CommandExecute = SetQuests
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "заявка+",
                AboutCommand = "Одобрение заявки беседы",
                RightValue = 10,
                CommandExecute = AcceptInviteChat
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "админчат",
                AboutCommand = "Устанавливает чат админов",
                RightValue = 10,
                CommandExecute = SetAdminChat
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "серверинфо",
                AboutCommand = "показывает информацию о сервере",
                RightValue = 10,
                CommandExecute = GetServerInfo
            });
        }

        public override MessageSend RunCommand(ExecCommand command, DefaultPerson u)
        {
            AloneUser User = (AloneUser)u; MessageSend output = new MessageSend();
            output = AdminPanel();
            foreach (var cmditem in CommandList)
            {
                if (cmditem.CommandName == command.cmd[1])
                {
                    if (User.Level >= cmditem.RightValue)
                        output = cmditem.CommandExecute(command, User);
                    break;
                }
            }
            return output;
        }

        /* Admin commands */

        public MessageSend SetQuests(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                BotSetting.StaticBotSetting.SetInviteForm(command.ReplyMessage);
                output.MessageText = "Новые вопросы установлены";
                output.IsOne_time = true;
                output.AddButtons("Список вопросов", ButtonBot.ButtonColor.primary, "вопросы");
            }
            else
                output.MessageText = BotAnswer.StaticSentence.CommandWithReplyText(command.cmd[1]);
            return output;
        }

        public MessageSend ShowQuests(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Вопросы:";
            for (int i = 0; i < BotSetting.StaticBotSetting.QuestForm.Count; i++)
                output.MessageText += "\n" + (i + 1) + ")" + BotSetting.StaticBotSetting.QuestForm[i];
            output.IsOne_time = true;
            output.AddButtons("Помощь", ButtonBot.ButtonColor.primary, "помощь");
            return output;
        }

        public MessageSend ShowAllChats(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Все имеющиеся чаты:";
            foreach (ChatDialog chat in BotSetting.StaticBotSetting.Chats)
            {
                output.MessageText += "\n👥\"" + chat.ChatName + "\"👥\n" +
                                      "ID: " + chat.ChatID + "\n" +
                                      "Admin: " + "[id" + chat.AdminID + "|Админ]\n" +
                                      "Участников: " + chat.Users.Count + "\n" +
                                      "Session: " + ASession.SessionString(chat.SessionType) + "\n\n";
            }
            return output;
        }

        public MessageSend ShowAllUsers(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Все имеющиеся пользователи:";
            foreach (UserDialog ud in BotSetting.StaticBotSetting.Users)
            {
                output.MessageText += "\n👤[id" + ud.User.UserID + "|" + ud.User.Name + " " + ud.User.Surname + "]\n" +
                                      "Level: " + ud.User.Level + "\n" +
                                      "Session: " + ASession.SessionString(ud.SessionType) + "\n\n";
            }
            return output;
        }

        public MessageSend AcceptInviteChat(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                try
                {
                    ChatDialog chat = BotSetting.StaticBotSetting.CheckInitChat(Convert.ToInt32(command.cmd[2]));
                    if (chat != null)
                    {
                        chat.SetSession(SelectSession.AdminChat);
                        MessageSend m = new MessageSend();
                        m.Peer_ID = chat.ChatID;
                        m.MessageText = "Ваша заявка одобрена!";
                        VKAPI.SendMessage(m);
                        output.MessageText = "Заявка принята";
                    }
                    else
                        output.MessageText = "Ошибка. Беседа не найдена";
                }
                catch
                {
                    output.MessageText = "Возникла ошибка :(";
                }
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            return output;
        }

        public MessageSend SetLevelForUser(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 3)
            {
                try
                {
                    UserDialog ud = BotSetting.StaticBotSetting.CheckInitUser(Convert.ToInt32(command.cmd[2]));
                    if (ud != null)
                    {
                        int val = Convert.ToInt32(command.cmd[3]);
                        if ((val < user.Level) && (user.Level >= ud.User.Level))
                        {
                            ud.User.Level = val;
                            output.MessageText = BotAnswer.StaticSentence.SetLevelForUser(ud.User);
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.HaventLevel();
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[2]);
                }
                catch
                {
                    output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend BroadCastMessage(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                BotSetting.StaticBotSetting.SetInviteForm(command.ReplyMessage);
                output.MessageText = "Новые вопросы установлены";
                output.IsOne_time = true;
                output.AddButtons("Список вопросов", ButtonBot.ButtonColor.primary, "вопросы");
            }
            else
                output.MessageText = BotAnswer.StaticSentence.CommandWithReplyText(command.cmd[1]);
            return output;
        }

        /*
         * 0 - SIgnoreUser
         * 1 - SLoginUser
         * 2 - SFormUser
         * 3 - SAloneUser
         * 4 - SAdminUser
         */

        public MessageSend SetSessionForUser(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 3)
            {
                try
                {
                    UserDialog ud = BotSetting.StaticBotSetting.CheckInitUser(Convert.ToInt32(command.cmd[2]));
                    if (ud != null)
                    {
                        int index = Convert.ToInt32(command.cmd[3]);
                        if (index == 0)
                        {
                            ud.SetSession(SelectSession.IgnoreUser);
                            output.MessageText = BotAnswer.StaticSentence.SetSessionForUser(ud.User, "игнор");
                        }
                        else if (index == 1)
                        {
                            ud.SetSession(SelectSession.LoginUser);
                            output.MessageText = BotAnswer.StaticSentence.SetSessionForUser(ud.User, "авторизация");
                        }
                        else if (index == 2)
                        {
                            ud.SetSession(SelectSession.FormUser);
                            output.MessageText = BotAnswer.StaticSentence.SetSessionForUser(ud.User, "опрос");
                        }
                        else if (index == 3)
                        {
                            ud.SetSession(SelectSession.AloneUser);
                            output.MessageText = BotAnswer.StaticSentence.SetSessionForUser(ud.User, "обычный");
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);
                }
                catch
                {
                    output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend SetAdminChat(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                try
                {
                    ChatDialog chd = BotSetting.StaticBotSetting.CheckInitChat(Convert.ToInt32(command.cmd[2]));
                    if (chd != null)
                    {
                        BotSetting.StaticBotSetting.AdminChat = chd;
                        MessageSend m = new MessageSend();
                        m.MessageText = "Этот чат инициализирован как чат администрации";
                        m.Peer_ID = chd.ChatID;
                        VKAPI.SendMessage(m);
                        output.MessageText = "Успешно!";
                    }
                    else
                        output.MessageText = "Чат не найден. Операция не выполнена";
                }
                catch
                {
                    output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend GetServerInfo(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Информация о состоянии сервера:\n" +
                "Кол-во ошибок: " + MainPage.Errors.Count + "\n" +
                "Кол-во переподключений: " + MainPage.CountReconnect + "\n" +
                "Кол-во отправленных сообщений: " + MainPage.CountMessages;
            return output;
        }

        /* Global */

        public MessageSend AdminPanel()
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Панель администратора:\n";
            output.AddButtons("Показать юзеров", ButtonBot.ButtonColor.primary, "юзеры");
            output.AddButtons("Показать чаты", ButtonBot.ButtonColor.primary, "чаты");
            output.AddButtons("Состояние сервера", ButtonBot.ButtonColor.primary, "серверинфо");
            output.AddButtons("Переход логин", ButtonBot.ButtonColor.secondary, "перейти 1");
            output.AddButtons("Переход опрос", ButtonBot.ButtonColor.secondary, "перейти 2");
            output.AddButtons("Переход обычный", ButtonBot.ButtonColor.secondary, "перейти 3");
            output.AddButtons("Переход админ", ButtonBot.ButtonColor.secondary, "перейти 4");

            output.IsOne_time = true;
            return output;
        }

        public MessageSend TransitionSession(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.cmd[2] == "3")
                {
                    if (udialog.SetSession(SelectSession.AloneUser))
                        output.MessageText = "Вы перешли в режим обычного диалога";
                    else
                        output.MessageText = "Не удалось перейти в режим обычного диалога";
                }
                else if (command.cmd[2] == "2")
                {
                    if (udialog.SetSession(SelectSession.FormUser))
                        output.MessageText = "Вы перешли в режим опроса пользователя";
                    else
                        output.MessageText = "Не удалось перейти в режим опроса пользователя";
                }
                else if (command.cmd[2] == "1")
                {
                    if (udialog.SetSession(SelectSession.LoginUser))
                        output.MessageText = "Вы перешли в режим авторизации пользователя";
                    else
                        output.MessageText = "Не удалось перейти в режим авторизации пользователя";
                }
                else if (command.cmd[2] == "4")
                {
                    if (udialog.SetSession(SelectSession.AdminUser))
                        output.MessageText = "Вы перешли в режим администрации";
                    else
                        output.MessageText = "Не удалось перейти в режим авторизации пользователя";
                }
                else
                    output.MessageText = "Не удалось осуществить переход в режим \"" + command.cmd[2] + "\"";
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[0]);

            return output;
        }

        public MessageSend Status(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Сессия: " + ASession.SessionString(udialog.SessionType) + "\n" +
                "Уровень: " + user.Level;
            return output;
        }
    }
}
