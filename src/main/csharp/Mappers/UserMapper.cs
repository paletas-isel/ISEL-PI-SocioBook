using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Model;

namespace Mappers
{
    public class UserMapper
    {
        private readonly LinkedList<User> _allUsers = new LinkedList<User>();
        
        private UserMapper()
        {

        }

        private volatile static UserMapper _singletonInstance;
        public static UserMapper Singleton
        {
            get
            {
                if (_singletonInstance == null)
                {
                    Interlocked.CompareExchange(ref _singletonInstance, new UserMapper(), null);
                }
                return _singletonInstance;

            }
        }

        public void Add(User share)
        {
            lock (_singletonInstance)
            {
                _allUsers.AddFirst(share);
            }
        }

        public void Remove(User share)
        {
            lock(_singletonInstance)
            {
                _allUsers.Remove(share);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _allUsers;
        }

        public User Get(string name)
        {
            User user = _allUsers.Where(p => p.Name.Equals(name)).SingleOrDefault();
            if(user == null)
            {
                user = new User(name);
                _allUsers.AddLast(user);
            }
            return user;
        }
    } 
}