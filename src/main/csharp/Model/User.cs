using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public List<Share> Shares { get; private set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User(string username, string password, string name)
        {
            Username = username;
            Password = password;
            Name = name;
            Shares = new List<Share>();
        }
    }
}