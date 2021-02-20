using System;

namespace Instant.Client.Core.Models
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public string SenderLogin { get; set; }
        public string Text { get; set; }
        public DateTime DateSent { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ChatMessage)
            {
                var otherChat = (ChatMessage)obj;
                return this.MessageId == otherChat.MessageId 
                    && this.ChatId == otherChat.ChatId
                    && this.SenderLogin == SenderLogin;
            }

            return false;
        }
    }
}