using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.ServiceModel.WebSockets;
using Mappers;

namespace ChatServer
{
    public class ChatService : WebSocketsService
    {
        private ChatMapper _chatMapper = ChatMapper.Singleton;
        private UserMapper _userMapper = UserMapper.Singleton;
        private static readonly ChatParticipants Participants = new ChatParticipants();

        public override void OnOpen()
        {
#if DEBUG
            Console.WriteLine("Joining chat!");
#endif
            Participants.AddParticipant(this);
            Console.WriteLine(HttpRequestHeaders[HttpRequestHeader.Cookie]);
        }

        public override void OnMessage(string value)
        {
            int sepIdx;
            string user = value.Substring(0, sepIdx = value.IndexOf('|'));
            string token = value.Substring(sepIdx);

            
        }

        protected override void OnClose(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine("Leaving chat!");
#endif
            ChatMapper mapper = ChatMapper.Singleton;
            
            //mapper.RemoveOnlineUser()
        }

        protected override void OnError(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine("An error ocurred!");
#endif
            OnClose(sender, e);
        }
    }
}