using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public List<Share> Shares { get; private set; }

        public string Name { get; private set; }

        public string Password { get; private set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
            Shares = new List<Share>();
        }
    }
}