using System.ComponentModel.DataAnnotations;

namespace GeekSpot.UI.Models
{
    public class User
    {
        public User()
        {

        }
        public User(int userId, string userName, string password)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
