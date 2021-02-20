using System;

namespace Instant.Client.Core.Models
{
    public class UserCredentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastOnline { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UserCredentials)
            {
                var otherUser = (UserCredentials) obj;
                return this.Login == otherUser.Login;
            }

            return false;
        }
    }
}