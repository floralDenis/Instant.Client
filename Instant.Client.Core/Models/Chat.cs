using Instant.Client.Core.Models.Enums;
using System.Collections.Generic;

namespace Instant.Client.Core.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public ChatTypes ChatType { get; set; }
        public string Title { get; set; }
        public IList<string> MembersLogins { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Chat)
            {
                var otherChat = (Chat) obj;
                return this.ChatId == otherChat.ChatId;
            }

            return false;
        }
    }
}