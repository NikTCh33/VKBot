using MobieBotVK.BotTools.JsonStruct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools
{
    public class MessageSend
    {
        public string MessageText;
        public int? Peer_ID;
        public int? User_ID;
        public KeyBoardBot KeyBoard;
        public bool IsInline
        {
            set
            {
                if(value)
                {
                    KeyBoard.inline = true;
                    KeyBoard.one_time = null;
                }
                else
                {
                    KeyBoard.inline = null;
                    KeyBoard.one_time = true;
                }
            }
        }
        public List<VKAttachment> attachs;

        public bool IsOne_time
        {
            set
            {
                KeyBoard.inline = null;
                KeyBoard.one_time = value;
            }
        }

        public MessageSend()
        {
            Peer_ID = null;
            User_ID = null;
            attachs = new List<VKAttachment>();
            KeyBoard = new KeyBoardBot();
        }

        public string GetKeyBoardObject()
        {
            string outp = JsonConvert.SerializeObject(KeyBoard);
            return outp;
        }

        public string GetAttachs()
        {
            string output = "";
            foreach(VKAttachment a in attachs)
            {
                if(a.type == "photo")
                {
                    output = a.type + a.photo.OwnerId + "_" + a.photo.id;
                    break;
                }
            }
            return output;
        }

        public void AddButtons(string Text, ButtonBot.ButtonColor Color, string _command)
        {
            ButtonBot btn = new ButtonBot
            {
                color = GetColor(Color),
                action = new ButtonAction
                {
                    label = Text,
                    type = "text",
                    payload = JsonConvert.SerializeObject( new Payload { command = _command })
                }
            };
            KeyBoard.buttons.Add(new List<ButtonBot>() { btn });
        }

        private string GetColor(ButtonBot.ButtonColor c)
        {
            if (c == ButtonBot.ButtonColor.negative)
                return "negative";
            else if (c == ButtonBot.ButtonColor.positive)
                return "positive";
            else if (c == ButtonBot.ButtonColor.primary)
                return "primary";
            else
                return "secondary";
        }
    }
}
