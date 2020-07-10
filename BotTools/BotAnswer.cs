using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobieBotVK.BotTools.JsonStruct;
using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.UserTools;

namespace MobieBotVK.BotTools
{
    public class BotAnswer
    {
        public BotSetting Setting;
        public string FileNameSetting = "Setting.json";

        public BotAnswer(string filename)
        {
            FileNameSetting = filename;
            string pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileNameSetting);
            if (File.Exists(pth))
            {
                string str = File.ReadAllText(pth);
                Setting = JsonConvert.DeserializeObject<BotSetting>(str);
                Setting.RecoverySetting();
            }
            else
            {
                Setting = new BotSetting();
                SaveSetting();
            }
            BotSetting.StaticBotSetting = Setting;
        }  

        public void SaveSetting()
        {
            string jsonfile = JsonConvert.SerializeObject(Setting);
            string pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileNameSetting);
            File.WriteAllText(pth, jsonfile);
        }

        public bool DeleteSetting()
        {
            string pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileNameSetting);
            try
            {
                File.Delete(pth);
                Setting = new BotSetting();
                SaveSetting();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MessageSend GetAnswer(ResponseLongPollServer.Update.Message mess)
        {
            MessageSend output = null;
            if (mess.peer_id > 0)
            {
                if (mess.peer_id - 2000000000 > 0)
                {
                    ChatDialog chat = Setting.CheckInitChat(mess.peer_id);
                    if (chat != null)
                    {
                        output = AnswerChat(chat, mess);
                        if (output != null) { output.Peer_ID = mess.peer_id; output.User_ID = null; }
                    }
                    else
                    {
                        output = Setting.InitChat(mess.peer_id);
                        if (output != null) { output.Peer_ID = mess.peer_id; output.User_ID = null; }
                    }
                }
                else
                {
                    UserDialog user = Setting.CheckInitUser(mess.peer_id);
                    if (user != null)
                    {
                        output = AnswerUser(user, mess);
                        if (output != null) { output.User_ID = mess.peer_id; output.Peer_ID = null; }
                    }
                    else
                    {
                        output = Setting.InitUser(mess.from_id);
                        if (output != null) { output.User_ID = mess.peer_id; output.Peer_ID = null; }            
                    }
                }
            }
            else return null;
            return output;
        }

        public MessageSend AnswerChat(ChatDialog chat, ResponseLongPollServer.Update.Message mess)
        {
            ChatUser user = chat.CheckInitUser(mess.from_id);
            MessageSend output;
            if (mess.action == null)
            {
                if (user != null)
                {
                    ExecCommand cmd = new ExecCommand(mess, false);
                    output = chat.ExecuteCommand(cmd, user);
                    chat.UpdateStatistics(user, mess.date);
                    return output;
                }
            }
            else if (chat.SessionType == SessionControl.SelectSession.AdminChat)
            {
                return EventAction(chat, user, mess);
            }
            
            return null;
        }

        public MessageSend AnswerUser(UserDialog userdialog, ResponseLongPollServer.Update.Message mess)
        {
            if(userdialog.CheckSubscribe())
            {
                ExecCommand cmd = new ExecCommand(mess, true);
                return userdialog.ExecuteCommand(cmd, userdialog.User);
            }
            MessageSend m = new MessageSend
            {
                MessageText = StaticSentence.UnSubscriber()
            };
            return m;
        }

        public MessageSend EventAction(ChatDialog chat, ChatUser user, ResponseLongPollServer.Update.Message mess)
        {
            ExecCommand cmd;
            string type = mess.action.type;
            int from_id = mess.from_id,
                user_id = mess.action.member_id;
            ChatUser uadmin = chat.GetUserForStr(chat.AdminID.ToString());
            if (type == "chat_invite_user" || type == "chat_invite_user_by_link")
            {
                cmd = new ExecCommand(new string[] { BotSetting.defaultbotname, "+участник", user_id.ToString() }, -1, "");
                return chat.ExecuteCommand(cmd, uadmin);
            }
            else if (type == "chat_kick_user")
            {
                if ((from_id == user_id) && chat.Kick_Leavers)
                {
                    cmd = new ExecCommand(new string[] { BotSetting.defaultbotname, "кик", user_id.ToString() }, -1, "");
                    return chat.ExecuteCommand(cmd, uadmin);
                }
                ChatUser u = chat.GetUserForStr(user_id.ToString());
                if (u != null)
                {
                    u.IsLeave = true;
                    cmd = new ExecCommand(new string[] { BotSetting.defaultbotname, "-жизнь", user_id.ToString() }, -1, "");
                    return chat.ExecuteCommand(cmd, uadmin);
                }          
            }
            return null;
        }

        /* Static sentence */

        public static class StaticSentence
        {
            public static string FirstInitChat()
            {
                return "Всем привет! Меня зовут \"Зайка\" и я бот🐰";
            }

            public static string ReInitChat()
            {
                return "Повторная инициализация беседы. Всем здравствуйте🐰";
            }

            public static string FirstInitUser()
            {
                return "Привет! Меня зовут \"Зайка\" и я бот🐰\nЯ вижу тебя впервые, не знаешь что написать? Напиши \"помощь\"";
            }

            public static string ReInitUser()
            {
                return "Привет! А я тебя помню🐰 Что нужно?";
            }

            public static string CommandNotFound(string command)
            {
                return "Команда \"" + command + "\" не найдена⛔";
            }

            public static string CommandMissing()
            {
                return "Некорректный запрос. Отсутствует команда⛔";
            }

            public static string HaventRight(string command, int right)
            {
                return "Для команды \"" + command + "\" требуется уровень доступа >= " + right.ToString() + "🥕";
            }

            public static string Status(string command, int right)
            {
                return "Для команды \"" + command + "\" требуется уровень доступа >= " + right.ToString();
            }

            public static string AddWarning(ChatUser u, int maxwarning)
            {
                return u.GetLink() + " получает предупреждение " + u.CountWarning + "/" + maxwarning + "⚠";
            }

            public static string MinusWarning(ChatUser u, int maxwarning)
            {
                return "C " + u.GetLink() + " сняты все предупреждения " + u.CountWarning + "/" + maxwarning + "⚠";
            }

            public static string SetImmune(ChatUser u, string act)
            {
                return u.GetLink() +  " " + act + " иммунитет💪🏻";
            }

            public static string ImmuneForCommand(ChatUser u, string command)
            {
                return u.GetLink() + " имеет иммунитет к команде \"" + command + "\"💪🏻";
            }

            public static string ChatUserNotFound(string command)
            {
                return "Команда \"" + command + "\" не выполнена. Участник не найден🔎";
            }

            public static string IncorrectParamCommand(string command)
            {
                return "Некорректные параметры для команды \"" + command + "\"❗";
            }

            public static string LastHealth(ChatUser u)
            {
                return u.GetLink() + " теряет последнюю жизнь и умирает навсегда😢";
            }

            public static string LoseHealth(ChatUser u, int maxhealth)
            {
                return u.GetLink() + " теряет жизнь " + u.Health + "/" + maxhealth + "💔";
            }

            public static string GiveHealth(ChatUser u, int maxhealth)
            {
                return u.GetLink() + " получает жизнь " + u.Health + "/" + maxhealth + "❤";
            }

            public static string RespectPlus()
            {
                return "➕Репутация повышена➕";
            }

            public static string RespectMinus()
            {
                return "➖Репутация понижена➖";
            }

            public static string HaventRespectPoints()
            {
                return "Недостаточно очков репутации⭐";
            }

            public static string SetBonusRespect(int val)
            {
                return "Значение бонусной репутации установлено - " + val + " ✅";
            }

            public static string UnSubscriber()
            {
                return "Простите, но вы не являетесь моим 🐰подписчиком🐰 и поэтому не можете вызывать команды";
            }

            public static string CommandWithReplyText(string command)
            {
                return "Команда \"" + command + "\"" + " принимает в качестве параметра текст сообщения Reply✉";
            }

            public static string NotTargetSelf(string command)
            {
                return "Нельзя использовать команду \"" + command + "\"" + " на самого себя☺";
            }

            public static string FailedSetSession()
            {
                return "Не удалось переключить сессию";
            }

            public static string NewLoginUser(AloneUser u)
            {
                return "Новая анкета от [id" + u.UserID + "|" + u.Name + " " + u.Surname + "]🐰";
            }

            public static string SetSessionForUser(AloneUser u, string session)
            {
                return "Пользователю [id" + u.UserID + "|" + u.Name + " " + u.Surname + "] установлена сессия " + session;
            }

            public static string HelpText()
            {
                return "Попробуй команду \"помощь\" чтобы я тебя поняла";
            }

            public static string HelpForLoginUser()
            {
                return "Вы не авторизованы. Для того, чтобы вступить к нам в клуб тебе нужно пройти" +
                " небольшой опрос. Приступим?";
            }

            public static string HaventLevel()
            {
                return "У вас недостаточный уровень🥕, чтобы выполнить эту операцию";
            }

            public static string SetLevelForUser(AloneUser u)
            {
                return "Пользователю [id" + u.UserID + "|" + u.Name + " " + u.Surname + "] устанавливается уровень доступа (" + u.Level + ")🥕";
            }

            public static string OverflowParam(int val)
            {
                return "Параметр не должен превышать " + val + " символов⛔";
            }

            public static string CantSetLevel()
            {
                return "Невозможно установить значение уровня, которое будет выше уровня вызывающего команду, или уровень не может быть отрицательным";
            }

            public static string SetLevel()
            {
                return "Значение уровня установлено ✅";
            }

            public static string NotFoundAttachment()
            {
                return "Отсутствует вложение";
            }
        }
    }

}
