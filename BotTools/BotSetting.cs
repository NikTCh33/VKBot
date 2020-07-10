using MobieBotVK.BotTools.ChatTools;
using MobieBotVK.BotTools.JsonStruct;
using MobieBotVK.BotTools.SessionControl;
using MobieBotVK.BotTools.SessionControl.Sessions.Chat;
using MobieBotVK.BotTools.SessionControl.Sessions.User;
using MobieBotVK.BotTools.UserTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools
{
    public class BotSetting
    {
        [JsonProperty("Chats")]
        public List<ChatDialog> Chats;

        [JsonProperty("UpdateComplete")]
        public bool UpdateComplete;

        [JsonProperty("Users")]
        public List<UserDialog> Users;

        [JsonProperty("QuestForm")]
        public List<string> QuestForm = new List<string>();

        [JsonProperty("AdminChat")]
        public ChatDialog AdminChat = null;

        [JsonProperty("AdminUser")]
        public UserDialog AdminUser = null;

        public BotSetting()
        {
            Chats = new List<ChatDialog>();
            Users = new List<UserDialog>();
            QuestForm = new List<string>();
            UpdateComplete = false;
        }

        public void RecoverySetting()
        {
            foreach(ChatDialog c in Chats)
            {
                c.SetSession(c.SessionType);
            }

            foreach (UserDialog u in Users)
            {
                u.SetSession(u.SessionType);
            }
            
        }

        public UserDialog AddNewAdmin(int user_id, int level)
        {
            foreach (var u in Users)
                if (u.User.UserID == user_id)
                {
                    u.User.Level = level;
                    return u;
                }

            VKChatInfo.Profiles userinfo = VKAPI.GetUserInfo(user_id);
            if (userinfo == null) return null;

            UserDialog udialog = new UserDialog();
            udialog.User = new AloneUser(userinfo.id, userinfo.screen_name, userinfo.first_name, userinfo.last_name);
            udialog.User.Level = level;
            Users.Add(udialog);
            return udialog;
        }

        public void SendNewInviteChat(ChatDialog chat)
        {
            if(AdminUser != null)
            {
                chat.IsSendInvite = true;
                MessageSend m = new MessageSend();
                m.MessageText = "Новая заявка от беседы:\n" +
                    "Название: " + chat.ChatName + "\n" +
                    "ID: " + chat.ChatID + "\n" +
                    "Админ: [id" + chat.AdminID + "| Админ]";
                m.IsInline = true;
                m.AddButtons("Принять заявку", ButtonBot.ButtonColor.primary, "заявка+ " + chat.ChatID);
                m.User_ID = AdminUser.User.UserID;
                VKAPI.SendMessage(m);
            }       
        }

        public void SendNewInviteUser(AloneUser user)
        {
            if (AdminChat != null)
            {
                MessageSend m = new MessageSend();
                m.MessageText = BotAnswer.StaticSentence.NewLoginUser(user);
                m.IsInline = true;
                m.AddButtons("Принять заявку", ButtonBot.ButtonColor.positive, "инвайт " + user.UserID);
                m.Peer_ID = AdminChat.ChatID;
                VKAPI.SendMessage(m);
            }
        }

        public static DateTime UnixTimeToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static long DateTimeToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }

        public ChatDialog CheckInitChat(int chatID)
        {
            for (int i = 0; i < Chats.Count; i++)
                if (Chats[i].ChatID == chatID)
                    return Chats[i];
            return null;
        }

        public UserDialog CheckInitUser(int userID)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].User.UserID == userID)
                    return Users[i];
            return null;
        }

        public MessageSend InitChat(int chat_id)
        {
            MessageSend output = new MessageSend();
            foreach (var cht in Chats)
                if (cht.ChatID == chat_id)
                {
                    output.MessageText = BotAnswer.StaticSentence.ReInitChat();
                    return output;
                }

            VKChatInfo info = VKAPI.GetChatInfo(chat_id);
            if (info == null) return null;

            ChatDialog chat = new ChatDialog(chat_id);
            ChatUser tmpU;
            foreach (var i in info.response.items)
            {
                if (i.member_id > 0)
                {
                    int inx = GetIndex(info.response.profiles, i.member_id);
                    if (info.response.profiles[inx].deactivated == null)
                    {
                        if (i.is_owner)
                        {
                            chat.AdminID = i.member_id;
                            tmpU = new ChatUser(chat, i.member_id,
                                                        info.response.profiles[inx].screen_name,
                                                        info.response.profiles[inx].first_name,
                                                        info.response.profiles[inx].last_name);
                            tmpU.Level = 10;
                            tmpU.Immune = true;
                            chat.Users.Add(tmpU);
                        }
                        else if (i.is_admin)
                        {
                            tmpU = new ChatUser(chat, i.member_id,
                                                        info.response.profiles[inx].screen_name,
                                                        info.response.profiles[inx].first_name,
                                                        info.response.profiles[inx].last_name);
                            tmpU.Level = 9;
                            tmpU.Immune = true;
                            chat.Users.Add(tmpU);
                        }
                        else
                        {
                            tmpU = new ChatUser(chat, i.member_id,
                                                        info.response.profiles[inx].screen_name,
                                                        info.response.profiles[inx].first_name,
                                                        info.response.profiles[inx].last_name);
                            tmpU.Level = 1;
                            chat.Users.Add(tmpU);
                        }
                    }
                }
            }
            Chats.Add(chat);
            output.MessageText = BotAnswer.StaticSentence.FirstInitChat();
            return output; 

            int GetIndex(List<VKChatInfo.Profiles> list, int id)
            {
                for (int i = 0; i < list.Count; i++)
                    if (id == list[i].id)
                        return i;
                return -1;
            }
        }

        public MessageSend InitUser(int user_id)
        {
            MessageSend output = new MessageSend();
            foreach (var u in Users)
                if (u.User.UserID == user_id)
                {
                    output.MessageText = BotAnswer.StaticSentence.ReInitUser();
                    return output;
                }

            VKChatInfo.Profiles userinfo = VKAPI.GetUserInfo(user_id);
            if (userinfo == null) return null;

            UserDialog udialog = new UserDialog();
            udialog.User = new AloneUser(userinfo.id, userinfo.screen_name, userinfo.first_name, userinfo.last_name);
            Users.Add(udialog);
            output.MessageText = BotAnswer.StaticSentence.FirstInitUser();
            return output;
        }

        public void SetInviteForm(string text)
        {
            QuestForm.Clear();
            QuestForm.AddRange(text.Split('\n'));
        }

        public void SendMessageForLevel(string message, params int[] level)
        {
            MessageSend m = new MessageSend();
            m.MessageText = message;
            m.Peer_ID = null;

            foreach(UserDialog u in Users)
            {
                foreach(int L in level)
                    if(u.User.Level == L)
                    {
                        m.User_ID = u.User.UserID;
                        VKAPI.SendMessage(m);
                        break;
                    }
            }
        }

        public static BotSetting StaticBotSetting;
        public static string defaultbotname = "зайка";
    }
}
