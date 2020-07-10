using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobieBotVK.BotTools.JsonStruct
{
    public class VKChatAll
    {
        public Response response;
        public class Response
        {
            public int count;
            public List<Items> items;
            public int unread_count;
        }

        public class Items
        {
            public ChatObject conversation;
        }

        public class ChatObject
        {
            public Peer peer;

            public class Peer
            {
                public int id;
                public string type;
                public int local_id;
            }
        }

    }
}
