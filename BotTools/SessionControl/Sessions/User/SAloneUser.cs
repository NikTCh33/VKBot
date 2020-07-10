using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.ImageTools;
using MobieBotVK.BotTools.JsonStruct;
using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.UserTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Sessions.User
{
    public class SAloneUser : ASession
    {
        public List<UserCommand> CommandList { get; set; }

        public UserDialog udialog;

        public SAloneUser(UserDialog _dialog)
        {
            udialog = _dialog; 
             CommandList = new List<UserCommand>();

            CommandList.Add(new UserCommand
            {
                CommandName = "помощь",
                AboutCommand = "помощь",
                RightValue = 1,
                CommandExecute = Help
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "-цвет",
                AboutCommand = "удаляет цвет с картинки",
                RightValue = 1,
                CommandExecute = IEffectDeleteColor
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "негатив",
                AboutCommand = "негатив",
                RightValue = 1,
                CommandExecute = IEffectNegative
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "-глубина",
                AboutCommand = "понижение глубины",
                RightValue = 1,
                CommandExecute = IEffectLowDeepColor
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "+цвет",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                CommandExecute = IEffectMaxColor
            });

            CommandList.Add(new UserCommand
            {
                CommandName = "+чб",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                CommandExecute = IEffectOnlyBlackWhite
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "чб",
                AboutCommand = "максимизирует цвет",
                RightValue = 1,
                CommandExecute = IEffectBlackWhite
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "яркость",
                AboutCommand = "повышение яркости",
                RightValue = 1,
                CommandExecute = IEffectBrightness
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "контраст",
                AboutCommand = "повышение контраста",
                RightValue = 1,
                CommandExecute = IEffectContrast
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "свап",
                AboutCommand = "меняет цвета местами",
                RightValue = 1,
                CommandExecute = IEffectSwapColor
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "цитата",
                AboutCommand = "цитата",
                RightValue = 1,
                CommandExecute = IEffectQuoteForUser
            });

            //Admin
            CommandList.Add(new UserCommand
            {
                CommandName = "перейти",
                AboutCommand = "Переход к выбранной сессии",
                RightValue = 9,
                CommandExecute = TransitionSession
            });
            CommandList.Add(new UserCommand
            {
                CommandName = "переходы",
                AboutCommand = "Показывает список переходов",
                RightValue = 9,
                CommandExecute = Transitions
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

        public MessageSend Help(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            output.MessageText = "Беспомощный ты мой! :)";
            return output;
        }

        public MessageSend IEffectDeleteColor(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();
            if(command.cmd.Count > 2)
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

        public MessageSend IEffectMaxColor(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectNegative(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectLowDeepColor(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectOnlyBlackWhite(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectBlackWhite(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectBrightness(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectContrast(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectSwapColor(ExecCommand command, AloneUser user)
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

        public MessageSend IEffectQuoteForUser(ExecCommand command, AloneUser user)
        {
            MessageSend output = new MessageSend();

                VKAPI.DownLoadPhoto(command.photo.GetHighQualityPhoto());
                ImageCreator.QuoteForUser("MyText", "");
                output.attachs.Add(new VKAttachment
                {
                    type = "photo",
                    photo = VKAPI.GetPhoto(user.UserID)
                });
                output.MessageText = "Готово!";
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
