using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Sessions.User
{
    class SFormUser : ASession
    {
        public List<UserCommand> CommandList { get; set; }

        public UserDialog udialog;

        public SFormUser(UserDialog _dialog)
        {
            udialog = _dialog;
            CommandList = new List<UserCommand>();

            CommandList.Add(new UserCommand
            {
                CommandName = "опрос",
                AboutCommand = "Запускает сессию опроса",
                RightValue = 1,
                CommandExecute = AgainForm
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "следвопрос",
                AboutCommand = "Переход в следующему вопросу опроса",
                RightValue = 1,
                CommandExecute = NextQuest
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "завершить",
                AboutCommand = "Завершает текущую сессию и возвращается к предыдущей",
                RightValue = 1,
                CommandExecute = FinishSession
            });

            CommandList.Add(new UserCommand
            {
                CommandName = "помощь",
                AboutCommand = "помощь",
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
            AloneUser User = (AloneUser)u; MessageSend output = null;
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

        private int IndexQuest = 1;
        private bool IsBegin = false;

        public MessageSend AgainForm(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            IndexQuest = 1; IsBegin = true;
            output.MessageText = "Начнем опрос:\n" + IndexQuest + ") ";
            if (IndexQuest > 0 && IndexQuest < BotSetting.StaticBotSetting.QuestForm.Count)
            {
                output.MessageText += BotSetting.StaticBotSetting.QuestForm[IndexQuest-1];
                output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
                output.AddButtons("Начать заново", ButtonBot.ButtonColor.negative, "опрос");
                output.AddButtons("Завершить", ButtonBot.ButtonColor.secondary, "завершить");
                output.IsOne_time = false;
            }
            else
                output.MessageText = "Опрос отсутствует, обратитесь к администратору";

            return output;
        }

        public MessageSend NextQuest(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            IndexQuest++;
            if (IsBegin)
            {
                if (IndexQuest == BotSetting.StaticBotSetting.QuestForm.Count)
                {
                    output.MessageText = IndexQuest + ") " + BotSetting.StaticBotSetting.QuestForm[IndexQuest - 1];
                    output.AddButtons("Отправить ответы🐰", ButtonBot.ButtonColor.positive, "следвопрос");
                    output.AddButtons("Начать заново", ButtonBot.ButtonColor.negative, "опрос");
                    output.AddButtons("Завершить", ButtonBot.ButtonColor.secondary, "завершить");
                    output.IsInline = false;
                }
                else if (IndexQuest < BotSetting.StaticBotSetting.QuestForm.Count)
                {
                    output.MessageText = IndexQuest + ") " + BotSetting.StaticBotSetting.QuestForm[IndexQuest - 1];
                    output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
                    output.AddButtons("Начать заново", ButtonBot.ButtonColor.negative, "опрос");
                    output.AddButtons("Завершить", ButtonBot.ButtonColor.secondary, "завершить");
                    output.IsOne_time = false;
                }
                else
                {
                    BotSetting.StaticBotSetting.SendNewInviteUser(user);
                    output.MessageText = "Спасибо за ответы, вы закончили опрос. Дождитесь когда администратор проанализирует ваши ответы и свяжется с вами!";
                    udialog.SetSession(SelectSession.LoginUser);
                    IsBegin = false;
                }
            }
            else
            {
                output.MessageText = "Опрос не начат. Хотите начать опрос?";
                output.AddButtons("Начать опрос", ButtonBot.ButtonColor.primary, "опрос");
                output.AddButtons("Завершить", ButtonBot.ButtonColor.secondary, "завершить");
                output.IsOne_time = false;
            }
            return output;
        }

        public MessageSend FinishSession(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if (udialog.SetSession(SelectSession.LoginUser))
            {
                output.MessageText = "Опрос остановлен.\nБуду ждать когда ты захочешь начать опрос";
                output.AddButtons("Приступим!", ButtonBot.ButtonColor.primary, "начать");
                output.IsOne_time = true;
            }
            else
                output.MessageText = "Не удалось остановить процедуру опроса. Обратитесь к администратору";
            return output;
        }

        public MessageSend Help(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Выберите действие:";
            if (IsBegin)
                output.AddButtons("Следующий вопрос", ButtonBot.ButtonColor.positive, "следвопрос");
            output.AddButtons("Начать опрос", ButtonBot.ButtonColor.primary, "опрос");
            output.AddButtons("Прервать опрос", ButtonBot.ButtonColor.primary, "завершить");
            output.IsOne_time = true;
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
