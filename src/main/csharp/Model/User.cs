using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public List<Share> Shares { get; private set; }

        public string Name { get; private set; }

        public User(string name)
        {
            Name = name;
            Shares = new List<Share>();
        }
    }
}