using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.ImageTools;
using MobieBotVK.BotTools.JsonStruct;
using MobieBotVK.BotTools.SessionControl.Commands;

namespace MobieBotVK.BotTools.SessionControl.Sessions.Chat
{
    public class SAdministrationChat : ASession
    {
        public List<ChatCommand> CommandList { get; set; }
        public ChatDialog Chat;
        public Random rnd = new Random();
        public SAdministrationChat(ChatDialog _Chat)
        {
            Chat = _Chat;
            CommandList = new List<ChatCommand>();
            CommandList.Add(new ChatCommand
            {
                CommandName = "пред",
                AboutCommand = "Выдает предупреждение выбранному участнику. Пример команды: \nЗайка пред [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = AddWarning
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-пред",
                AboutCommand = "Снимает все предупреждения с выбранного участника. Пример команды: \nЗайка -пред [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = MinusWarning
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "преды",
                AboutCommand = "Показывает всех участников, у которых есть предупреждения. Пример команды: \nЗайка преды",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = ShowAllWarning
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "иммун",
                AboutCommand = "Дает иммунитет выбранному участнику. Пример команды: \nЗайка иммун [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = AddImmune
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-иммун",
                AboutCommand = "Дает иммунитет выбранному участнику. Пример команды: \nЗайка иммун [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = MinusImmune
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "иммуны",
                AboutCommand = "Показывает всех участников, имеющих иммунитеты. Пример команды: \nЗайка иммуны",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = ShowAllImmune
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "участники",
                AboutCommand = "Показывает список зарегистрированных участников беседы. Пример команды: \nЗайка участники",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Statistics,
                CommandExecute = ShowAllUsers
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "приветствие",
                AboutCommand = "Устанавливает текст приветствия, которое будет показано приглашенному участнику. Пример команды: \nЗайка приветствие [ Текст приветствия ]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = SetHelloText
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "обновить",
                AboutCommand = "Обновляет список зарегистрированных участников. Если текущий список имеет участника, которого фактически нет в беседе, то эта команда удалит его из списка участников. Пример команды: \nЗайка обновить",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = UpdateChat
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "участник",
                AboutCommand = "Показывает информацию об участнике беседы. Пример команды: \nБез параметра покажет участника, который вызвал эту команду\nЗайка участник\nС параметром покажет участника указанного в параметре\nЗайка участник [фамилия | имя | никнейм | id | shortid]",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Statistics,
                CommandExecute = GetInfoUser
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "макспред",
                AboutCommand = "Устанавливает максимальное количество предупреждений, для получения кика. Пример команды: \nЗайка макспред [значение]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = SetMaxCountWarning
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "+участник",
                AboutCommand = "Регистрирует указанного участника в беседе. Пример команды: \nЗайка +участник [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = InitNewUser
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "кик",
                AboutCommand = "Исключает указанного участника. Пример команды: \nЗайка кик [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = KickUser
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "+",
                AboutCommand = "Участник тратит очко репутации и повышает репутацию выбранному участнику. Пример команды: \nЗайка + [фамилия | имя | никнейм | id | shortid]",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = PlusRespect
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-",
                AboutCommand = "Участник тратит очко репутации и понижает репутацию выбранному участнику. Пример команды: \nЗайка - [фамилия | имя | никнейм | id | shortid]",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = MinusRespect
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-жизнь",
                AboutCommand = "Отнимает одну жизнь у указаного участника. Пример команды: \nЗайка -жизнь [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = MinusHealth
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "+жизнь",
                AboutCommand = "Добавляет одну жизнь указаному участнику. Пример команды: \nЗайка +жизнь [фамилия | имя | никнейм | id | shortid]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = AddHealth
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "команда",
                AboutCommand = "Показывает список команд. Если есть параметр, показывает информацию о конкретной команде. Пример команды: \nЗайка команды [название команды]",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = ShowCommand
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "бонусрепутация",
                AboutCommand = "Устанавливает значение бонусной репутации. Пример команды: \nЗайка бонусрепутация [ значение ]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = SetBonusRespect
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "+имя",
                AboutCommand = "Добавляет новое имя для бота. Пример команды: \nЗайка новоеимя [ значение ]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = AddNewName
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-имя",
                AboutCommand = "Удаляет имя для бота. Пример команды: \nЗайка удалитьимя [ значение ]",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Managment,
                CommandExecute = DeleteName
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "имена",
                AboutCommand = "Показывает список имен. Пример команды: \nЗайка имена ",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Statistics,
                CommandExecute = ShowAllNames
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "чат",
                AboutCommand = "Показывает настройки данного чата. Пример команды: \nЗайка чат",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Statistics,
                CommandExecute = ShowSetting
            }); 
            CommandList.Add(new ChatCommand
            {
                CommandName = "стат",
                AboutCommand = "Показывает статистику за последнюю неделю. Пример команды: \nЗайка стат",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Statistics,
                CommandExecute = ShowStats
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "меню",
                AboutCommand = "Показывает меню с кнопками Пример команды: \nЗайка меню",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = ShowMenu
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "перейти",
                AboutCommand = "Переход к выбранной сессии",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = TransitionSession
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "застрелиться",
                AboutCommand = "Совершает суецид с шансом 1/6",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = ShotSelf
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "уровень",
                AboutCommand = "назначает уровень",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = SetLevel
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "киквышедших",
                AboutCommand = "назначает уровень",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = SetKickLeavers
            });

            //Эффекты
            CommandList.Add(new ChatCommand
            {
                CommandName = "-цвет",
                AboutCommand = "удаляет цвет с картинки",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectDeleteColor
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "негатив",
                AboutCommand = "негатив",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectNegative
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "-глубина",
                AboutCommand = "понижение глубины",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectLowDeepColor
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "+цвет",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                CommandExecute = IEffectMaxColor
            });

            CommandList.Add(new ChatCommand
            {
                CommandName = "+чб",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectOnlyBlackWhite
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "чб",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectBlackWhite
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "яркость",
                AboutCommand = "повышение яркости",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectBrightness
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "контраст",
                AboutCommand = "повышение контраста",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectContrast
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "свап",
                AboutCommand = "меняет цвета местами",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = IEffectSwapColor
            });
        }

        public override MessageSend RunCommand(ExecCommand command, DefaultPerson u)
        {
            ChatUser User = (ChatUser)u; MessageSend output = new MessageSend();
            if ((command.ReplyID == -1) && !Chat.CheckBotName(command.cmd[0]))
                return null;

            if (command.cmd.Count > 1)
            {
                foreach (var cmditem in CommandList)
                    if (cmditem.CommandName == command.cmd[1])
                    {
                        if (User.Level >= cmditem.RightValue)
                        {
                            output = cmditem.CommandExecute(command, User);
                            return output;
                        }
                        else
                        {
                            output.MessageText = BotAnswer.StaticSentence.HaventRight(cmditem.CommandName, cmditem.RightValue);
                            return output;
                        }
                    }
                output.MessageText = BotAnswer.StaticSentence.CommandNotFound(command.cmd[1]);
            }
            else
                output.MessageText = BotAnswer.StaticSentence.CommandMissing();
            
            if (command.ReplyID != -1) return null;

            return output;
        }

        private MessageSend AddWarning(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (!u.Immune)
                        {
                            u.CountWarning++;
                            output.MessageText = BotAnswer.StaticSentence.AddWarning(u, Chat.MaxWarning);
                            if (u.CountWarning >= Chat.MaxWarning)
                            {
                                u.Health--; u.CountWarning = 0;
                                if (u.Health <= 0)
                                {
                                    u.Health = 0;
                                    Chat.DeleteUser(u);
                                    VKAPI.DeleteChatUser(Chat.ChatID, u.UserID);
                                    output.MessageText += "\n" + BotAnswer.StaticSentence.LastHealth(u);
                                }
                                else
                                    VKAPI.DeleteChatUser(Chat.ChatID, u.UserID);
                            }
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.ImmuneForCommand(u, command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend MinusWarning(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);

                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (!u.Immune)
                        {
                            u.CountWarning = 0;
                            output.MessageText = BotAnswer.StaticSentence.MinusWarning(u, Chat.MaxWarning);
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.ImmuneForCommand(u, command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }
               
        private MessageSend ShowAllWarning(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "⚠Предупреждения участников⚠"; string out_2 = "";
            foreach (var u in Chat.Users)
                if (u.CountWarning > 0)
                    out_2 += "\n" + u.GetLink() + " - " + u.CountWarning;
            if (out_2 == "")
                output.MessageText = "Ни один из участников не имеет предупрежднений🙂";
            output.MessageText = output.MessageText + out_2;
            return output;
        }

        private MessageSend AddImmune(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {

                        u.Immune = true;
                        output.MessageText = BotAnswer.StaticSentence.SetImmune(u, "получает");
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);
            }

            return output;
        }

        private MessageSend MinusImmune(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {

                        u.Immune = false;
                        output.MessageText = BotAnswer.StaticSentence.SetImmune(u, "теряет");
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);
            }
            return output;
        }

        private MessageSend ShowAllImmune(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "💪🏻Иммунитеты имеют💪🏻"; string out_2 = "";
            foreach (var u in Chat.Users)
                if (u.Immune)
                    out_2 += "\n" + u.GetLink();
            if (out_2 == "")
                output.MessageText = "Ни один из участников не имеет иммунитета🙂";
            output.MessageText = output.MessageText + out_2;
            return output;
        }
                
        private MessageSend ShowAllUsers(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Всего " + Chat.Users.Count + "👥"; string out_2 = "", out_3 = "";
            foreach (var u in Chat.Users)
                if (!u.IsLeave)
                    out_2 += "\n👤 " + u.GetLink();
                else
                    out_3 += "\n👤 " + u.GetLink();
            if(Chat.Users.Count > 0)
            {
                if (out_2 != "")
                    output.MessageText += "\n👤Активные участники👤" + out_2;
                if (out_3 != "")
                    output.MessageText += "\n👣Вышедшие участники👣" + out_3;
            }
            else
                output.MessageText = "В чате нет участников🙂";
            return output;
        }
               
        private MessageSend SetHelloText(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                Chat.HelloText = command.ReplyMessage;
                output.MessageText = "Новое приветствие установлено ✅";
            }
            else
                output.MessageText = "🙋‍♀Приветствие🙋‍♀\n" + Chat.HelloText;
            return output;
        }
               
        private MessageSend UpdateChat(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (Chat.ReInitChat() != "")
                output.MessageText = "Обновление участников прошло успешно ✅";
            else
                output.MessageText = "В ходе обновления возникли неполадки ⛔";
            return output;
        }
              
        private MessageSend GetInfoUser(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            ChatUser u = ((command.cmd.Count > 2) ? Chat.GetUserForStr(command.cmd[2]) : user);
            if (u != null)
                output.MessageText = u.GetInfo();
            else
                output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);
            return output;
        }
             
        private MessageSend SetMaxCountWarning(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                int tmp;

                try
                {
                    tmp = Convert.ToInt32(command.cmd[2]);
                    if (tmp > 0)
                    {
                        Chat.MaxWarning = tmp;
                        output.MessageText = "Максимальное количество предупреждений установлено - " + Chat.MaxWarning + " ✅";
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
                catch
                {
                    output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }
                
        private MessageSend InitNewUser(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                ChatUser u = Chat.GetUserForStr(command.cmd[2]);
                if (u == null)
                {
                    u = Chat.AddUserInChat(command.cmd[2]);
                    if (u != null)
                        output.MessageText = Chat.GetHello(u);
                    else
                        output.MessageText = "Не удалось добавить нового участника⛔";
                }
                else
                {
                    if(u.IsLeave)
                    {
                        u.IsLeave = false;
                        output.MessageText = Chat.GetAgainHello(u);
                    }
                    else
                    {
                        Chat.DeleteUser(u);
                        u = Chat.AddUserInChat(command.cmd[2]);
                        if (u != null)
                            output.MessageText = Chat.GetHello(u) + "\n(Этот участник был переинициализирован)";
                        else
                            output.MessageText = "Не удалось добавить нового участника⛔";
                    }
                }               
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        private MessageSend SetLevel(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 3)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        try
                        {
                            int lvl = Convert.ToInt32(command.cmd[3]);
                            if ((lvl <= user.Level) && lvl >= 0)
                            {
                                u.Level = lvl;
                                output.MessageText = BotAnswer.StaticSentence.SetLevel();
                            }
                            else output.MessageText = BotAnswer.StaticSentence.CantSetLevel();
                        }
                        catch
                        {
                            output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                        }
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend MinusHealth(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (!u.Immune)
                        {
                            u.Health--;
                            if (u.Health <= 0)
                            {
                                u.Health = 0;
                                Chat.DeleteUser(u);
                                VKAPI.DeleteChatUser(Chat.ChatID, u.UserID);
                                output.MessageText = BotAnswer.StaticSentence.LastHealth(u);
                            }
                            else output.MessageText = BotAnswer.StaticSentence.LoseHealth(u, Chat.MaxHealth);

                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.ImmuneForCommand(u, command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend AddHealth(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (!u.Immune)
                        {
                            u.Health++;
                            if (u.Health > Chat.MaxHealth)
                                u.Health = Chat.MaxHealth;
                            output.MessageText = BotAnswer.StaticSentence.GiveHealth(u, Chat.MaxHealth);
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.ImmuneForCommand(u, command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend KickUser(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (!u.Immune)
                        {
                            u.Health--; VKAPI.DeleteChatUser(Chat.ChatID, u.UserID);
                            u.IsLeave = true;
                            if (u.Health <= 0)
                            {
                                u.Health = 0;
                                Chat.DeleteUser(u);
                                output.MessageText = BotAnswer.StaticSentence.LastHealth(u);
                            }
                            else output.MessageText = BotAnswer.StaticSentence.LoseHealth(u, Chat.MaxHealth);
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.ImmuneForCommand(u, command.cmd[1]);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend PlusRespect(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);
                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (user.RespectPoints - 1 >= 0)
                        {
                            user.RespectPoints--;
                            u.RespectPlus++;
                            output.MessageText = BotAnswer.StaticSentence.RespectPlus();
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.HaventRespectPoints();
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);
            }
            return output;
        }

        private MessageSend MinusRespect(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend(); string targetUser = "";
            if (command.ReplyID != -1)
                targetUser = command.ReplyID.ToString();
            else if (command.cmd.Count > 2)
                targetUser = command.cmd[2];
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);

            if (targetUser != "")
            {
                ChatUser u = Chat.GetUserForStr(targetUser);

                if (u != null)
                {
                    if (user.UserID != u.UserID)
                    {
                        if (user.RespectPoints - 1 >= 0)
                        {
                            user.RespectPoints--;
                            u.RespectMinus++;
                            output.MessageText = BotAnswer.StaticSentence.RespectMinus();
                        }
                        else
                            output.MessageText = BotAnswer.StaticSentence.HaventRespectPoints();
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.NotTargetSelf(command.cmd[1]);
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.ChatUserNotFound(command.cmd[1]);

            }
            return output;
        }

        private MessageSend SetKickLeavers(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (Chat.Kick_Leavers)
                output.MessageText = "Теперь участники не будут кикнуты если они покинут беседу";
            else
                output.MessageText = "Теперь участники будут кикнуты если они покинут беседу";
            Chat.Kick_Leavers = !Chat.Kick_Leavers;
            return output;
        }

        private MessageSend SetBonusRespect(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                try
                {
                    int tmp = Convert.ToInt32(command.cmd[2]);
                    if(tmp > 0)
                    {
                        Chat.BonusRespect = tmp;
                        output.MessageText = BotAnswer.StaticSentence.SetBonusRespect(Chat.BonusRespect);
                    }
                    else
                        output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
                catch
                {
                    output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
                }
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }
              
        private MessageSend ShowCommand(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                foreach (var c in CommandList)
                    if (c.CommandName == command.cmd[2])
                    {
                        output.MessageText = "Команда \"" + c.CommandName + "\":\n" + "Уровень доступа: " + c.RightValue + "\nОписание: " + c.AboutCommand;
                        break;
                    }
                output.MessageText = "Команда \"" + command.cmd[2] + "\" не найдена";
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }
              
        private MessageSend AddNewName(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                output.MessageText = Chat.AddName(command.cmd[2].ToLower());
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }
               
        private MessageSend DeleteName(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                output.MessageText = Chat.DeleteName(command.cmd[2].ToLower());
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }
               
        private MessageSend ShowAllNames(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "💬Список имён, на которые откликается бот💬";
            foreach (var s in Chat.BotNames)
            {
                output.MessageText += "\n🐰" + s;
            }
            return output;
        }
              
        private MessageSend ShowSetting(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            ChatUser admin = Chat.GetUserForStr(Chat.AdminID.ToString());
            output.MessageText = "🔧Информация о настройках чата🔧\n" +
                "Создатель: [id" + admin.UserID + "|" + admin.Name + " " + admin.Surname + "]\n" +
                "Кол-во участников: " + Chat.Users.Count + "\n" +
                "Мах предупреждений: " + Chat.MaxWarning + "\n" +
                "Мах жизней: " + Chat.MaxHealth + "\n" +
                "Исключать вышедших: " + (Chat.Kick_Leavers ? "Да" : "Нет") + "\n" +
                "Бонусная репутация: " + Chat.BonusRespect;
            return output;
        }
          
        private MessageSend ShowStats(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = Chat.GetStatsAllMessage();
            return output;
        }

        private MessageSend StartTorture(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (Chat.SetSession(SelectSession.TortureChat))
                output.MessageText = "Включен режим опроса";
            else
                output.MessageText = BotAnswer.StaticSentence.FailedSetSession();
            return output;
        }

        private MessageSend ShowMenu(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.IsInline = true;
            output.MessageText = "📚Меню📚\n";
            output.AddButtons("📈Статистика", ButtonBot.ButtonColor.primary, "стат");
            output.AddButtons("🐰Режим пытки", ButtonBot.ButtonColor.secondary, "перейти 1");
            output.AddButtons("😎Режим администрирования", ButtonBot.ButtonColor.secondary, "перейти 2");
            return output;
        }

        public MessageSend TransitionSession(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.cmd[2] == "1")
                {
                    if (Chat.SetSession(SelectSession.TortureChat))
                        output.MessageText = "Вы перешли в режим пытки";
                    else
                        output.MessageText = "Не удалось перейти в режим пытки";
                }
                else if (command.cmd[2] == "2")
                {
                    if (Chat.SetSession(SelectSession.AdminChat))
                        output.MessageText = "Вы перешли в режим администрирования чата";
                    else
                        output.MessageText = "Не удалось перейти в режим администрирования чата";
                }
                else
                    output.MessageText = "Не удалось осуществить переход в режим \"" + command.cmd[2] + "\"";
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[0]);

            return output;
        }

        public MessageSend ShotSelf(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            Thread suicide = new Thread(SuicideProcess);
            if (!user.Immune && user.Level < 9)
            {
                output = null;
                suicide.Start();
            }
            else
                output.MessageText = "Вы имеете иммунитет и не можете застрелиться";
            return output;
            void SuicideProcess()
            {
                MessageSend m = new MessageSend();
                m.Peer_ID = Chat.ChatID;
                m.MessageText = "Заряжаю револьвер🔫..."; VKAPI.SendMessage(m);
                Thread.Sleep(3000);
                m.MessageText = "Последнее слово?"; VKAPI.SendMessage(m);
                Thread.Sleep(5000);
                if(rnd.Next(1,7) == 1)
                {
                    m.MessageText = "Револьвер выстрелил💥 и " + user.GetLink() + " погибает😭"; VKAPI.SendMessage(m);
                    user.Health--; VKAPI.DeleteChatUser(Chat.ChatID, user.UserID);
                    user.IsLeave = true;
                    if (user.Health <= 0)
                    {
                        user.Health = 0;
                        Chat.DeleteUser(user);
                        m.MessageText = BotAnswer.StaticSentence.LastHealth(user);
                    }
                    else m.MessageText = BotAnswer.StaticSentence.LoseHealth(user, Chat.MaxHealth);
                    VKAPI.SendMessage(m);
                }
                else
                {
                    m.MessageText = "Револьвер не выстрелил\n" + user.GetLink() + " тебе очень повезло. Не делай больше так😅"; VKAPI.SendMessage(m);
                }               
            }
        }

        public MessageSend SetNickName(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.cmd[2].Length < 25)
                {
                    user.NickName = command.cmd[2];
                    output.MessageText = "Никнейм установлен ✅";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.OverflowParam(25);
            }
            else output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        //эффекты

        public MessageSend IEffectDeleteColor(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.photo != null)
                {

                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    ImageCreator.DeleteColor(command.cmd.GetRange(2, command.cmd.Count - 2).ToArray());
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend IEffectMaxColor(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.photo != null)
                {

                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    ImageCreator.MaxColor(command.cmd.GetRange(2, command.cmd.Count - 2).ToArray());
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend IEffectNegative(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();

            if (command.photo != null)
            {
                VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                ImageCreator.Negative();
                output.attachs.Add(new VKAttachment
                {
                    type = "photo",
                    photo = VKAPI.GetPhoto(user.UserID)
                });
                output.MessageText = "Готово!";
            }
            else
                output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            return output;
        }

        public MessageSend IEffectLowDeepColor(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.photo != null)
                {
                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    int x;
                    try
                    {
                        x = Convert.ToInt32(command.cmd[2]);
                    }
                    catch
                    {
                        output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
                        return output;
                    }
                    ImageCreator.LowDeepColor(x);
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend IEffectOnlyBlackWhite(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();

            if (command.photo != null)
            {
                VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                ImageCreator.OnlyBlackWhite();
                output.attachs.Add(new VKAttachment
                {
                    type = "photo",
                    photo = VKAPI.GetPhoto(user.UserID)
                });
                output.MessageText = "Готово!";
            }
            else
                output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            return output;
        }

        public MessageSend IEffectBlackWhite(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();

            if (command.photo != null)
            {
                VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                ImageCreator.BlackWhite();
                output.attachs.Add(new VKAttachment
                {
                    type = "photo",
                    photo = VKAPI.GetPhoto(user.UserID)
                });
                output.MessageText = "Готово!";
            }
            else
                output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            return output;
        }

        public MessageSend IEffectBrightness(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.photo != null)
                {
                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    int x;
                    try
                    {
                        x = Convert.ToInt32(command.cmd[2]);
                    }
                    catch
                    {
                        output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
                        return output;
                    }
                    ImageCreator.Brightness(x);
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend IEffectContrast(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 2)
            {
                if (command.photo != null)
                {
                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    int x;
                    try
                    {
                        x = Convert.ToInt32(command.cmd[2]);
                    }
                    catch
                    {
                        output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
                        return output;
                    }
                    ImageCreator.Contrast(x);
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

        public MessageSend IEffectSwapColor(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.cmd.Count > 3)
            {
                if (command.photo != null)
                {

                    VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                    ImageCreator.SwapColor(command.cmd[2], command.cmd[3]);
                    output.attachs.Add(new VKAttachment
                    {
                        type = "photo",
                        photo = VKAPI.GetPhoto(user.UserID)
                    });
                    output.MessageText = "Готово!";
                }
                else
                    output.MessageText = BotAnswer.StaticSentence.NotFoundAttachment();
            }
            else
                output.MessageText = BotAnswer.StaticSentence.IncorrectParamCommand(command.cmd[1]);
            return output;
        }

    }
}
