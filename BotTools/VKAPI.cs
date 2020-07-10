using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MobieBotVK.BotTools.JsonStruct;
using System.Net.Http;
using System.Threading;
using MobieBotVK.BotTools.ImageTools;

namespace MobieBotVK.BotTools
{
    public static class VKAPI
    {
        public  static string Group_id;
        private static string Token;
        private static string LP_Key;
        public static string LP_Server;
        private static string LP_Ts;
        private static ResponseError error;
        private static Action<string> WriteMessage;
        private static string method_url = "https://api.vk.com/method/";
        private static Random rnd;

        public static void CreateVKAPI(string _Token, string _group_id, Action<string> func)
        {
            Token = _Token;
            Group_id = _group_id;
            WriteMessage = func;
            rnd = new Random();
            GetLongPollServer();
        }

        public static ResponseLongPollServer GetLongPollServer()
        {
            string answer = POST(method_url + "groups.getLongPollServer",
                "group_id=" + Group_id +
                "&lp_version=3" +
                "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            ResponseLongPollServer resp = JsonConvert.DeserializeObject<ResponseLongPollServer>(answer);
            LP_Key = resp.response.key;
            LP_Server = resp.response.server;
            LP_Ts = resp.response.ts;
            return resp;
        }

        public static string SendMessage(MessageSend message)
        {
            string tmp = ((message.User_ID == null) ? ("peer_id=" + message.Peer_ID) : ("user_id=" + message.User_ID)) +
                    "&message=" + Uri.EscapeDataString(message.MessageText) +
                    "&random_id=" + rnd.Next() +
                    "&group_id=" + Group_id +
                    "&keyboard=" + Uri.EscapeDataString(message.GetKeyBoardObject()) +
                    "&attachment=" + message.GetAttachs() +
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103";
            string answer = POST(method_url + "messages.send", tmp)
                    ;
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
           
            return answer;
        }

        public static VKPhotoServer GetMessagesUploadServer(int peer_id)
        {
            string answer = GET(method_url + "photos.getMessagesUploadServer",
                                "peer_id=" + peer_id +
                                "&lp_version=3" +
                                "&access_token=" + Token + 
                                "&v=5.103"); 
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if(error.error != null)
            {
                SendMessageError();
                return null;
            }
            VKPhotoServer resp = JsonConvert.DeserializeObject<VKPhotoServer>(answer);
            return resp;
        }

        public static VKSaveMessagesPhoto SaveMessagesPhoto(string photo, int server, string hash)
        {
            string answer = GET(method_url + "photos.saveMessagesPhoto",
                                "photo=" + photo +
                                "&server=" + server +
                                "&hash=" + hash +
                                "&lp_version=3" +
                                "&access_token=" + Token +
                                "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            VKSaveMessagesPhoto resp = JsonConvert.DeserializeObject<VKSaveMessagesPhoto>(answer);
            return resp;
        }

        public static VKPhotoFromServer LoadToServer(string url)
        {
            WebClient wc = new WebClient();

            byte[] tmpbt = wc.UploadFile(url, "POST", ImageCreator.TempNewFile);
            string tmpstr = Encoding.UTF8.GetString(tmpbt);
              

            VKPhotoFromServer vkpfs = JsonConvert.DeserializeObject<VKPhotoFromServer>(tmpstr);
            return vkpfs;
        }

        public static void DownLoadPhoto(string url)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(url, ImageCreator.TempDownFile);
        }

        public static VKPhotoObject GetPhoto(int peer_id)
        {
            VKPhotoServer vkps = GetMessagesUploadServer(peer_id);
            if (vkps == null) return null;
            VKPhotoFromServer vkpfs = LoadToServer(vkps.response.upload_url);
            if (vkpfs == null) return null;
            VKSaveMessagesPhoto vksmp = SaveMessagesPhoto(vkpfs.photo, vkpfs.server, vkpfs.hash);
            if (vksmp == null) return null;
            return vksmp.response[0];
        }

        public static VKChatInfo GetChatInfo(int peer_id)
        {
            string answer = GET(method_url + "messages.getConversationMembers",
                    "peer_id=" + peer_id +
                    "&group_id=" + Group_id +
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            return JsonConvert.DeserializeObject<VKChatInfo>(answer);
        }

        public static VKChatInfo.Profiles GetUserInfo(int user_id)
        {
            string answer = GET(method_url + "users.get",
                                "user_ids=" + user_id +
                                "&group_id=" + Group_id +
                                "&fields=photo_50" + 
                                "&lp_version=3" +
                                "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            VKUserInfo user = JsonConvert.DeserializeObject<VKUserInfo>(answer);
            if (user.response.Count > 0)
                return user.response[0];
            else
                return null;
        }

        public static VKChatAll GetChats(int count, int offset)
        {
            string answer = GET(method_url + "messages.getConversations",
                    "count=" + count +
                    "&offset=" + offset +
                    "&group_id" + Group_id +
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            return JsonConvert.DeserializeObject<VKChatAll>(answer);
        }

        public static string DeleteMessage(string messageid)
        {
            string answer = GET(method_url + "messages.delete",
                    "message_ids=" + messageid +
                    "&group_id=" + Group_id +
                    "&delete_for_all=1" + 
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103");
            return answer;
        }
        
        public static string DeleteChatUser(int chat_id, int user_id)
        {
            string answer = GET(method_url + "messages.removeChatUser",
                    "chat_id=" +(chat_id - 2000000000) +
                    "&user_id=" + user_id +
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            return answer;
        }

        public static VKMessages.Messages GetMessages(int peer_id,int offset, int count)
        {
            string answer = GET(method_url + "messages.getHistory",
                    "peer_id=" + peer_id +
                    "&offset=" + offset +
                    "&count="  + count +
                    "&group_id=" + Group_id +
                    "&lp_version=3" +
                    "&access_token=" + Token + "&v=5.103");
            error = JsonConvert.DeserializeObject<ResponseError>(answer);
            if (error.error != null)
            {
                SendMessageError();
                return null;
            }
            return JsonConvert.DeserializeObject<VKMessages.Messages>(answer);
        }

        public static List<ResponseLongPollServer.Update.Message> CheckServer()
        {
            try
            {
                string answer = GET(LP_Server,
                        "act=a_check" +
                        "&key=" + LP_Key +
                        "&ts=" + LP_Ts +
                        "&wait=25"
                        );
                error = JsonConvert.DeserializeObject<ResponseError>(answer);
                if (error.error != null)
                {
                    SendMessageError();
                    return null;
                }
                ResponseLongPollServer.ResultServer resp = JsonConvert.DeserializeObject<ResponseLongPollServer.ResultServer>(answer);
                LP_Ts = resp.ts;

                List<ResponseLongPollServer.Update.Message> list = new List<ResponseLongPollServer.Update.Message>();
                foreach (var m in resp.updates)
                    if (m._type == "message_new")
                        list.Add(m._object.message);
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static void SendMessageError()
        {
            WriteMessage("Error(" + error.error.error_code + "): " + error.error.error_msg);
            error = null;
        }

        private static string GET(string Url, string Data)
        {
            string input = Url + "?" + Data;
            WebRequest req = WebRequest.Create(input);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();         
            return Out;
        }
        private static string POST(string Url, string Data)
        {
            WebRequest req = WebRequest.Create(Url);
            req.Method = "POST";
            byte[] bytearray = Encoding.UTF8.GetBytes(Data);
            req.ContentLength = bytearray.Length;
            Stream strm = req.GetRequestStream();
            strm.Write(bytearray, 0, bytearray.Length);
            strm.Close();

            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        private class RespMesId
        {
            [JsonProperty("Response")]
            public string message_id;
        }
    }
}
