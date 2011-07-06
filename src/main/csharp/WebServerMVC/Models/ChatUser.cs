using SuperWebSocket;

namespace WebServerMVC.Models
{
    public class ChatUser
    {
        public ChatUser(string username, WebSocketSession session)
        {
            Username = username;
            Session = session;
        }

        public WebSocketSession Session { get; private set; }

        public string Username { get; private set; }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != typeof(ChatUser)) return false;
            return Equals((ChatUser)o);
        }

        public bool Equals(ChatUser other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Session, Session) && Equals(other.Username, Username);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Session != null ? Session.GetHashCode() : 0)*397) ^ (Username != null ? Username.GetHashCode() : 0);
            }
        }
    }
}