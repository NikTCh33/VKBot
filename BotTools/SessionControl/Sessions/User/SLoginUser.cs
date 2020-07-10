using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Sessions.User
{
    public class SLoginUser : ASession
    {
        public List<UserCommand> CommandList { get; set; }

        public UserDialog udialog;

        public SLoginUser(UserDialog _dialog)
        {
            udialog = _dialog;
            CommandList = new List<UserCommand>();

            CommandList.Add(new UserCommand
            {
                CommandName = "начать",
                AboutCommand = "Запускает сессию опроса",
                RightValue = 1,
                CommandExecute = StartForm
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "помощь",
                AboutCommand = "Помогает пользователю понять что куда нажимать",
                RightValue = 1,
                CommandExecute = Help
            });

            //Admin
            CommandList.Add(new UserCommand
            {
                CommandName = "переходы",
                AboutCommand = "Показывает список переходов",
                RightValue = 9,
                CommandExecute = Transitions
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
                CommandName = "статус",
                AboutCommand = "Статус",
                RightValue = 9,
                CommandExecute = Status
            });
        }

        public override MessageSend RunCommand(ExecCommand command, DefaultPerson u)
        {
            AloneUser User = (AloneUser)u; MessageSend output = new MessageSend();
            output.MessageText = BotAnswer.StaticSentence.HelpText();
            output.IsOne_time = true;
            output.AddButtons("Помощь", ButtonBot.ButtonColor.primary, "помощь");
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

        public MessageSend StartForm(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (udialog.SetSession(SelectSession.FormUser))
            {
                output.MessageText = "Сейчас вам предстоит пройти опрос для вступления в конференцию " +
                    "\"\".\nОтвечайте честно и развернуто на каждый вопрос. Не бойтесь! Перед началом " +
                    " опроса найдите спокойное место, где никто не будет отвлекать, отвечайте неторопливо и вдумчиво.\n" +
                    "P.S. Если не хотите отвечать на вопрос, то поставьте \"-\"\n" +
                    "Чтобы начать, нажмите соответствующую кнопку или введите команду \"опрос\"";
                output.IsOne_time = false;
                output.AddButtons("Начать опрос", ButtonBot.ButtonColor.primary, "опрос");
                output.AddButtons("Я передумал(а)", ButtonBot.ButtonColor.secondary, "завершить");
            }
            else
                output.MessageText = "Я пока не готова :(. Обратись к администратору.";

            return output;
        }

        public MessageSend Help(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = BotAnswer.StaticSentence.HelpForLoginUser();
            output.IsOne_time = true;
            output.AddButtons("Приступим!", ButtonBot.ButtonColor.primary, "начать");
            return output;
        }


        /* Admin commands */

        /* Global */
        public MessageSend Transitions(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Переходы:\n";
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
