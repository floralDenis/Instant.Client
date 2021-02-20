using Instant.Client.Core.Models.Enums;

namespace Instant.Client.Core.Models
{
    public class ChatPermission
    {
        public int ChatId { get; set; }
        public ChatPermissionTypes PermissionType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ChatPermission)
            {
                var otherChat = (ChatPermission)obj;
                return this.ChatId == otherChat.ChatId;
            }

            return false;
        }
    }
}