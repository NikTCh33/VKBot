using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MobieBotVK.BotTools.JsonStruct;

namespace MobieBotVK.BotTools.ChatTools
{
    public class ChatUser : DefaultPerson
    {
        [JsonProperty("NickName")]
        public string NickName;

        [JsonProperty("Level")]
        public int Level;

        [JsonProperty("CountWarning")]
        public int CountWarning;

        [JsonProperty("Immune")]
        public bool Immune;

        [JsonProperty("Health")]
        public int Health;

        [JsonProperty("RespectPlus")]
        public int RespectPlus;

        [JsonProperty("RespectMinus")]
        public int RespectMinus;

        [JsonProperty("DateInvited")]
        public long DateInvited;

        [JsonProperty("DateLastMessage")]
        public long DateLastMessage;

        [JsonProperty("WeekCountMessage")]
        public int WeekCountMessage;

        [JsonProperty("RespectPoints")]
        public int RespectPoints;

        [JsonProperty("IsLeave")]
        public bool IsLeave;

        public ChatUser() : base()
        {

        }

        public ChatUser(ChatDialog chat, int ID, string shortid, string name, string surname) : base(ID, shortid, name, surname)
        {
            NickName = name;
            Level = 1;
            CountWarning = 0;
            Immune = false;
            Health = chat.MaxHealth;
            RespectPlus = 0;
            RespectMinus = 0;
            DateInvited = BotSetting.DateTimeToUnixTime(DateTime.Now);
            DateLastMessage = 0;
            WeekCountMessage = 0;
            IsLeave = false;
            RespectPoints = 3;
        }

        public ChatUser(ChatUser u, int ID, string shortid, string name, string surname) : base(ID, shortid, name, surname)
        {
            NickName = name;
            Level = u.Level;
            CountWarning = u.CountWarning;
            Immune = u.Immune;
            Health = u.Health;
            RespectPlus = u.RespectPlus;
            RespectMinus = u.RespectMinus;
            DateInvited = u.DateInvited;
            DateLastMessage = u.DateLastMessage;
            WeekCountMessage = u.WeekCountMessage;
            IsLeave = false;
            RespectPoints = u.RespectPoints;
        }

        public string GetLink()
        {
            return "[id" + UserID + "|" + Name + " " + Surname + "]";
        }

        public string GetInfo()
        {
            return "👤Участник: " + Name + " " + Surname + ":\n" +
                "📝Никнейм: " + NickName + "\n" +
                "🥕Уровень доступа: " + Level + "\n" +
                "⚠Предупреждения: " + CountWarning + "\n" +
                 "🌝Репутация: ➕" + RespectPlus + "|➖" + RespectMinus + "\n" +
                 "⭐Очки репутации: " + RespectPoints + "\n" +
                 "❤Жизни: " + Health + "\n" +
                "🍫Дата приглашения: " + BotSetting.UnixTimeToDateTime(DateInvited).ToShortDateString() + "\n" +
                "🍭Дата последнего сообщения: " + BotSetting.UnixTimeToDateTime(DateLastMessage).ToShortDateString() + "\n" +
                "✉Сообщений за неделю: " + WeekCountMessage + "\n" +
                "💪🏻Иммунитет: " + ((Immune) ? "Есть" : "Нет");
        }
    }
}
