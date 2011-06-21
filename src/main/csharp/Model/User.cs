using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class User
    {
        public List<Share> Shares { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Editable(false)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public User(string username, string password, string name)
        {
            Username = username;
            Password = password;
            Name = name;
            Shares = new List<Share>();
        }

        public override bool Equals(object other)
        {
            if(other is User)
            {
                User otherUser = other as User;
                return this.Username.Equals(otherUser.Username);
            }

            return false;
        }
    }
}