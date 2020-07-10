using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MobieBotVK.BotTools.JsonStruct;
using MobieBotVK.BotTools.SessionControl;
using MobieBotVK.BotTools.SessionControl.Commands;
using MobieBotVK.BotTools.SessionControl.Sessions;
using MobieBotVK.BotTools.SessionControl.Sessions.Chat;

namespace MobieBotVK.BotTools.ChatTools
{
    public class ChatDialog : DefaultDialog
    {
        /* Static properties */
        [JsonProperty("Users")]
        public List<ChatUser> Users;
        [JsonProperty("AdminID")]
        public int AdminID;
        [JsonProperty("ChatID")]
        public int ChatID;
        [JsonProperty("ChatName")]
        public string ChatName = "chat_name";

        /* Dynamic properties */
        [JsonProperty("MaxWarning")]
        public int MaxWarning;

        [JsonProperty("MaxHealth")]
        public int MaxHealth;

        [JsonProperty("HelloText")]
        public string HelloText;

        [JsonProperty("BotNames")]
        public List<string> BotNames;

        [JsonProperty("Kick_Leavers")]
        public bool Kick_Leavers;

        [JsonProperty("BonusRespect")]
        public int BonusRespect;

        [JsonProperty("QuestForm")]
        public List<string> QuestForm;

        [JsonProperty("LinkInTortureChat")]
        public string LinkInTortureChat;
        [JsonProperty("LinkInAdminChat")]
        public string LinkInAdminChat;

        /* Global properties */

        [JsonProperty("IsSendInvite")]
        public bool IsSendInvite;

        public ChatDialog(int _id)
        {
            Users = new List<ChatUser>();
            BotNames = new List<string>(new string[] { BotSetting.defaultbotname });
            ChatID = _id;
            HelloText = "Добро пожаловать!";
            Kick_Leavers = false;
            MaxWarning = 2;
            BonusRespect = 3;
            MaxHealth = 3;
            SessionControl = new SLoginChat(this); SessionType = SelectSession.LoginChat;
            QuestForm = new List<string>();
            IsSendInvite = false;
        }

        public override MessageSend ExecuteCommand(ExecCommand command, DefaultPerson User)
        {
            return SessionControl.RunCommand(command, User);
        }

        public override bool SetSession(SelectSession session)
        {
            if (session == SelectSession.AdminChat)
            {
                SessionType = session;
                SessionControl = new SAdministrationChat(this);
                return true;
            }
            else if (session == SelectSession.LoginChat)
            {
                SessionType = session;
                SessionControl = new SLoginChat(this);
                return true;
            }
            else if (session == SelectSession.TortureChat)
            {
                SessionType = session;
                SessionControl = new STortureChat(this);
                return true;
            }
            return false;
        }

        public void SetQuestForm(string text)
        {
            QuestForm.Clear();
            QuestForm.AddRange(text.Split('\n'));
        }

        public string AddName(string botname)
        {
            BotNames.Add(botname);
            return "Новое имя \"" + botname + "\" добавлено";
        }

        public string DeleteName(string botname)
        {
            if (botname != BotSetting.defaultbotname)
            {
                BotNames.Remove(botname);
            }
            return "Имя \"" + botname + "\" удалено";
        }

        public void DeleteUser(ChatUser u)
        {
            Users.Remove(u);
        }

        public void DeleteUser(int user_id)
        {
            foreach (var u in Users)
                if (u.UserID == user_id)
                {
                    Users.Remove(u);
                    break;
                }
        }

        public ChatUser GetUserForStr(string input)
        {
            input = input.ToLower();
            try
            {
                int tmp_id = Convert.ToInt32(input);
                foreach (var u in Users)
                {
                    if ((u.UserID == tmp_id) ||
                        (u.Surname.ToLower() == input) ||
                        (u.Name.ToLower() == input) ||
                        (u.NickName.ToLower() == input) ||
                        (u.ShortID.ToLower() == input))
                        return u;
                }
            }
            catch
            {
                foreach (var u in Users)
                {
                    if ((u.Surname.ToLower() == input) ||
                        (u.Name.ToLower() == input) ||
                        (u.NickName.ToLower() == input) ||
                        (u.ShortID.ToLower() == input))
                        return u;
                }
            }
            return null;
        }

        public string GetHello(ChatUser u)
        {
            return "Здравствуй, [id" + u.UserID + "|" + u.Name + "]!\n" + HelloText;
        }

        public string GetAgainHello(ChatUser u)
        {
            return "И снова здравствуй!\n" + u.GetInfo();
        }

        public string GetStatsAllMessage()
        {
            Users = SortList(Users);
            string output = ""; int cnt = 0, inx = 1;
            foreach (var u in Users)
            {
                output += "\n" + (inx++) + ".👤" + u.Name + " " + u.Surname + ": " + u.WeekCountMessage;
                cnt += u.WeekCountMessage;
            }
            output = "📈Статистика за последнюю неделю📈\n📩Всего сообщений: " + cnt + output;
            return output;
        }
        
        public string ReInitChat()
        {
            VKChatInfo info = VKAPI.GetChatInfo(ChatID);
            if (info == null) return "";

            List<ChatUser> new_users = new List<ChatUser>();

            foreach (var i in info.response.items)
            {
                if (i.member_id > 0)
                {
                    int inx = GetIndex(info.response.profiles, i.member_id);
                    if (info.response.profiles[inx].deactivated == null)
                    {
                        ChatUser olduser = GetUserForStr(i.member_id.ToString());
                        if (i.is_owner)
                        {
                            AdminID = i.member_id;
                            ChatUser tmp;
                            if (olduser != null)
                                tmp = new ChatUser(olduser, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            else
                                tmp = new ChatUser(this, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            tmp.Immune = true;
                            tmp.Level = 10;
                            new_users.Add(tmp);
                        }
                        else if (i.is_admin)
                        {
                            ChatUser tmp;
                            if (olduser != null)
                                tmp = new ChatUser(olduser, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            else
                                tmp = new ChatUser(this, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            tmp.Immune = true;
                            tmp.Level = 9;
                            new_users.Add(tmp);
                        }
                        else
                        {
                            ChatUser tmp;
                            if (olduser != null)
                                tmp = new ChatUser(olduser, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            else
                                tmp = new ChatUser(this, i.member_id, info.response.profiles[inx].screen_name, info.response.profiles[inx].first_name, info.response.profiles[inx].last_name);
                            tmp.Immune = false;
                            tmp.Level = 1;
                            new_users.Add(tmp);
                        }
                    }
                }
            }
            Users = new_users;
            return "Чат обновлён";

            int GetIndex(List<VKChatInfo.Profiles> list, int id)
            {
                for (int i = 0; i < list.Count; i++)
                    if (id == list[i].id)
                        return i;
                return -1;
            }
        }
        
        public bool CheckBotName(string botname)
        {
            foreach (var n in BotNames)
            {
                if (n == botname)
                    return true;
            }
            return false;
        }

        public ChatUser AddUserInChat(string us_id)
        {
            int user_id;
            try
            {
                user_id = Convert.ToInt32(us_id);
            }
            catch
            {
                return null;
            }
            VKChatInfo info = VKAPI.GetChatInfo(ChatID);
            if (info == null) return null;

            foreach (var i in info.response.profiles)
            {
                if ((i.id == user_id) && (i.deactivated == null))
                {
                    ChatUser u = new ChatUser(this, i.id, i.screen_name, i.first_name, i.last_name);
                    Users.Add(u);
                    return u;
                }
            }
            return null;
        }

        public ChatUser CheckInitUser(int user_id)
        {
            foreach (var u in this.Users)
                if (u.UserID == user_id)
                    return u;
            return null;
        }

        public void UpdateStatistics(ChatUser user, long mesdate)
        {
            user.WeekCountMessage++;
            user.DateLastMessage = mesdate;
        }

        private List<ChatUser> SortList(List<ChatUser> list)
        {
            ChatUser t;
            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < list.Count - 1; j++)//лень сделать сортировку побыстрее :(
                    if (list[j + 1].WeekCountMessage > list[j].WeekCountMessage)
                    {
                        t = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = t;
                    }
            return list;
        }

    }
}
