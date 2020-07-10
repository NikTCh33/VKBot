using System;
using System.Collections.Generic;
using System.Text;
using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.SessionControl.Commands;

namespace MobieBotVK.BotTools.SessionControl.Sessions.Chat
{
    public class STortureChat : ASession
    {

        public List<ChatCommand> CommandList { get; set; }
        public ChatDialog Chat;
        public STortureChat(ChatDialog _Chat)
        {
            Chat = _Chat;
            CommandList = new List<ChatCommand>();
            CommandList.Add(new ChatCommand
            {
                CommandName = "опрос",
                AboutCommand = "Начинает новый опрос",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = BeginForm
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "следвопрос",
                AboutCommand = "Переходит к следующему вопросу",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = NextQuest
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "вопросы",
                AboutCommand = "Выводит список вопросов",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = AllQuest
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "новыевопросы",
                AboutCommand = "Устанавливает новые вопросы в качестве сообщения Reply",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = SetQuests
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "меню",
                AboutCommand = "Показывает меню",
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
                CommandName = "инвайт",
                AboutCommand = "Отправить ссылку приглашения в беседу",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = SendInvite
            });

            CommandList.Add(new ChatCommand
            {
                CommandName = "ссылкапытки",
                AboutCommand = "Отправить ссылку приглашения в беседу",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = GetLinkInviteTorture
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "ссылкаосн",
                AboutCommand = "Отправить ссылку приглашения в беседу",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = GetLinkInviteAdmin
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

        private int IndexQuest = 1;
        private bool IsBegin = false;

        public MessageSend BeginForm(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            IndexQuest = 1; IsBegin = true;
            output.MessageText = "Начнём опрос:\n" + IndexQuest + ") ";
            if (IndexQuest > 0 && IndexQuest < Chat.QuestForm.Count)
            {
                output.MessageText += Chat.QuestForm[IndexQuest - 1];
                output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
                output.IsInline = true;
            }
            else
                output.MessageText = "Отсутствует база вопросов";

            return output;
        }

        public MessageSend NextQuest(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            IndexQuest++;
            if (IsBegin)
            {
                if (IndexQuest == Chat.QuestForm.Count)
                {
                    output.MessageText = IndexQuest + ") " + Chat.QuestForm[IndexQuest - 1];
                    output.AddButtons("Принять участника", ButtonBot.ButtonColor.positive, "ссылкаосн");
                    output.AddButtons("Закончить опрос", ButtonBot.ButtonColor.negative, "следвопрос");
                    output.IsInline = true;
                }
                else if (IndexQuest < Chat.QuestForm.Count)
                {
                    output.MessageText = IndexQuest + ") " + Chat.QuestForm[IndexQuest - 1];
                    output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
                    output.IsInline = true;
                }
                else
                {
                    output.MessageText = "Опрос окончен";
                    IsBegin = false;
                }
            }
            else
            {
                output.MessageText = "Хотите начать опрос?";
                output.AddButtons("Начать опрос", ButtonBot.ButtonColor.positive, "опрос");
                output.IsInline = true;
            }

            return output;
        }

        public MessageSend SetQuests(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                Chat.SetQuestForm(command.ReplyMessage);
                output.IsInline = true;
                output.MessageText = "Новые вопросы установлены";
                output.AddButtons("Начать опрос", ButtonBot.ButtonColor.primary, "опрос");
                output.AddButtons("Список вопросов", ButtonBot.ButtonColor.primary, "вопросы"); 
            }
            else
                output.MessageText = BotAnswer.StaticSentence.CommandWithReplyText(command.cmd[1]);
            return output;
        }

        public MessageSend AllQuest(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (Chat.QuestForm.Count > 0)
            {
                output.MessageText = "Вопросы:\n";
                for (int i = 0; i < Chat.QuestForm.Count; i++)
                    output.MessageText += "\n" + (i + 1) + ") " + Chat.QuestForm[i];
            }
            else
                output.MessageText = "Вопросы отсутствуют";
            return output;
        }

        public MessageSend SendInvite(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if(command.cmd.Count > 2)
            {
                MessageSend m = new MessageSend();
                m.MessageText = "🐰Ваша заявка одобрена :)🐰\n Приглашаем вас на беседу с администрацией\n" + Chat.LinkInTortureChat;
                try
                {
                    m.User_ID = Convert.ToInt32(command.cmd[2]);
                    if (VKAPI.SendMessage(m) != null)
                    {
                        output.MessageText = "Приглашение успешно выслано ✅";
                    }
                    else
                        output.MessageText = "⛔Возникла ошибка при отправке сообщения. Либо неверное ID получателя, либо получатель запретил доступ к сообщениям.";
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

        private MessageSend ShowMenu(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.IsInline = true;
            output.MessageText = "Меню:\n";
            output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
            output.AddButtons("Начать опрос", ButtonBot.ButtonColor.primary, "опрос");
            output.AddButtons("Список вопросов", ButtonBot.ButtonColor.primary, "вопросы");
            output.AddButtons("Режим пытки", ButtonBot.ButtonColor.secondary, "перейти 1");
            output.AddButtons("Режим администрирования", ButtonBot.ButtonColor.secondary, "перейти 2");
            return output;
        }

        private MessageSend GetLinkInviteTorture(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                Chat.LinkInTortureChat = command.ReplyMessage;
                output.MessageText = "Новая ссылка установлена ✅";
            }
            else
                output.MessageText = "🙋‍Ссылка🙋‍♀\n" + Chat.LinkInTortureChat;
            return output;
        }

        private MessageSend GetLinkInviteAdmin(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (command.ReplyID != -1)
            {
                Chat.LinkInAdminChat = command.ReplyMessage;
                output.MessageText = "Новая ссылка установлена ✅";
            }
            else
                output.MessageText = "🙋‍Ссылка🙋‍♀\n" + Chat.LinkInAdminChat;
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
    }
}
