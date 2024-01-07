using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSpot.Classes
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IsAdmin { get; set; }

        public User(string nickname, string email, string password, int isAdmin)
        {
            Nickname = nickname;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }

        public User()
        {

        }
    }
}
