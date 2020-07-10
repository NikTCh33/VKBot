using MobieBotVK.BotTools.JsonStruct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Commands
{
    public class ExecCommand
    {
        public List<string> cmd;

        public int ReplyID;

        public string ReplyMessage;

        public VKPhotoObject photo;

        public ExecCommand(ResponseLongPollServer.Update.Message mess, bool IsAloneUser)
        {
            string[] _cmd = mess.text.Split(' ');
            cmd = new List<string>();
            if (mess.attachments != null && mess.attachments.Count > 0)
            {
                photo = mess.attachments[0].photo;
            }
            else
                photo = null;
            Payload payload = CheckPayload(mess.payload);
            if(payload != null)
            {
                cmd.Add(BotSetting.defaultbotname);
                cmd.AddRange(payload.command.Split(' '));
                ReplyID = -1;
                ReplyMessage = "";
            }
            else if (mess.reply_message != null)
            {
                cmd.Add(BotSetting.defaultbotname);
                cmd.AddRange(_cmd);
                ReplyID = mess.reply_message.from_id;
                ReplyMessage = mess.reply_message.text;
            }
            else
            {
                if (IsAloneUser) cmd.Add(BotSetting.defaultbotname);
                cmd.AddRange(_cmd);
                ReplyID = -1;
                ReplyMessage = "";
            }
            if (cmd.Count > 0) cmd[0] = cmd[0].ToLower();
            if (cmd.Count > 1) cmd[1] = cmd[1].ToLower();
        }

        public Payload CheckPayload(string json)
        {
            try
            {
                Payload p = JsonConvert.DeserializeObject<Payload>(json);
                return p;
            }
            catch
            {
                return null;
            }
        }

        public ExecCommand(string[] _cmd, int _ReplyID, string _ReplyMessage)
        {
            cmd = new List<string>();
            cmd.AddRange(_cmd);
            ReplyID = _ReplyID;
            ReplyMessage = _ReplyMessage;
        }
    }
}
