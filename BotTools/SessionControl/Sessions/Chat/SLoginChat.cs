using System;
using System.Collections.Generic;
using System.Text;
using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.SessionControl.Commands;

namespace MobieBotVK.BotTools.SessionControl.Sessions.Chat
{
    class SLoginChat : ASession
    {
        public List<ChatCommand> CommandList { get; set; }

        public ChatDialog Chat;

        public SLoginChat(ChatDialog _Chat)
        {
            Chat = _Chat;
            CommandList = new List<ChatCommand>();

            CommandList.Add(new ChatCommand
            {
                CommandName = "заявка",
                AboutCommand = "Отправляет заявку на включение бота\nПример команды: \nЗайка заявка",
                RightValue = 9,
                Group = ChatCommand.CommandGroup.Moderation,
                CommandExecute = RequestRegistration
            });
            CommandList.Add(new ChatCommand
            {
                CommandName = "помощь",
                AboutCommand = "Отправляет заявку на включение бота\nПример команды: \nЗайка заявка",
                RightValue = 1,
                Group = ChatCommand.CommandGroup.Interaction,
                CommandExecute = Help
            });
        }

        public override MessageSend RunCommand(ExecCommand command, DefaultPerson u)
        {
            ChatUser User = (ChatUser)u; MessageSend output = new MessageSend();
            if (!Chat.CheckBotName(command.cmd[0]))
                return null;

            output.MessageText = "Если не знаете как со мной начать общаться, воспользуйтесь командой \"зайка помощь\"\n";
            output.AddButtons("Помощь", ButtonBot.ButtonColor.primary, "помощь");
            output.IsInline = true;
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
            }
            else
                output.MessageText = BotAnswer.StaticSentence.CommandMissing();

            return output;
        }

        /* Command */

        private MessageSend RequestRegistration(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            if (!Chat.IsSendInvite)
            {
                BotSetting.StaticBotSetting.SendNewInviteChat(Chat);
                output.MessageText = "Заявка отправлена";
            }
            else output.MessageText = "Заявка уже отправлена, ожидайте ее обработки";
            return output;
        }

        private MessageSend Help(ExecCommand command, ChatUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Итак, для того чтобы я заработала, нужно оправить заявку командой \"зайка заявка\" и ждать ее одобрения.";
            output.AddButtons("Отправить заявку", ButtonBot.ButtonColor.positive, "заявка");
            output.AddButtons("Помощь", ButtonBot.ButtonColor.primary, "помощь");
            output.IsInline = true;
            return output;
        }
    }
}
