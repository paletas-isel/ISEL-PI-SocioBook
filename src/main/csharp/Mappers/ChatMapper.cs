using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Model;

namespace Mappers
{
    public class ChatMapper
    {
        private LinkedList<User> _onlineUsers = new LinkedList<User>();

        private ChatMapper()
        {

        }

        private volatile static ChatMapper _singletonInstance;
        public static ChatMapper Singleton
        {
            get
            {
                if (_singletonInstance == null)
                {
                    Interlocked.CompareExchange(ref _singletonInstance, new ChatMapper(), null);
                }
                return _singletonInstance;
            }
        }

        public bool AddOnlineUser(User user)
        {
            if (_onlineUsers.Contains(user))
                return false;

            _onlineUsers.AddLast(user);
            return true;
        }

        public bool RemoveOnlineUser(User user)
        {
            if (!_onlineUsers.Contains(user))
                return false;

            _onlineUsers.Remove(user);
            return true;
        }

        public IEnumerable<User> GetOnlineUsers()
        {
            return _onlineUsers;
        }
    }
}
