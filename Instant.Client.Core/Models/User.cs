using System;

namespace Instant.Client.Core.Models
{
    public class User
    {
        public string Login { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is User)
            {
                var otherUser = (User) obj;
                return this.Login == otherUser.Login;
            }

            return false;
        }
    }
}