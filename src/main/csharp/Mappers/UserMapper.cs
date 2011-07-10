using System;
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
                if (_allUsers.Select(p => p.Username).Contains(share.Username))
                    throw new InvalidOperationException("User already exists!");
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

        public User Get(string userName)
        {
            User user = _allUsers.Where(p => p.Username.Equals(name)).SingleOrDefault();
            
            return user;
        }
    } 
}