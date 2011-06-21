using System.Collections.Generic;

namespace ChatServer
{
    public class ChatParticipants
    {
        private volatile List<ChatService> _participants = new List<ChatService>();

        public void AddParticipant(ChatService participant)
        {
            lock(this)
            {
                _participants.Add(participant);
            }
        }

        public void RemoveParticipant(ChatService participant)
        {
            lock (this)
            {
                _participants.Remove(participant);
            }
        }
    }
}