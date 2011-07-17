using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class User
    {
        public List<Share> Shares { get; private set; }
        public List<User> Friends { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Editable(false)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public long IdFacebook { get; set; }

        public string TokenFacebook { get; set; }

        public User(string username, string password, string name)
        {
            Username = username;
            Password = password;
            Name = name;
            Shares = new List<Share>();
            Friends = new List<User>();
        }

        public User(long id, string token, string name)
        {
            IdFacebook = id;
            Username = id.ToString();
            Name = name;
            TokenFacebook = token;
            Shares = new List<Share>();
            Friends = new List<User>();
        }

        public override bool Equals(object other)
        {
            if (other is User)
            {
                User otherUser = other as User;
                return this.Username.Equals(otherUser.Username);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }
    }
}