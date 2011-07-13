using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Model;

namespace Mappers
{
    public class FriendsMapper
    {

        private FriendsMapper(){}

        private volatile static FriendsMapper _singletonInstance;
        public static FriendsMapper Singleton
        {
            get
            {
                if (_singletonInstance == null)
                {
                    Interlocked.CompareExchange(ref _singletonInstance, new FriendsMapper(), null);
                }
                return _singletonInstance;

            }
        }

        public bool Add(User user, User friend)
        {
            if (user == null || friend == null)
                return false;

            lock (_singletonInstance)
            {
                if (!user.Friends.Contains(friend))
                {
                    user.Friends.Add(friend);
                    friend.Friends.Add(user);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(User user, User friend)
        {
            if (user == null || friend == null)
                return false;

            lock (_singletonInstance)
            {
                if (user.Friends.Contains(friend))
                {
                    user.Friends.Remove(friend);
                    friend.Friends.Remove(user);
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<User> GetAll(User user)
        {
            return user.Friends;
        }
    }
}
