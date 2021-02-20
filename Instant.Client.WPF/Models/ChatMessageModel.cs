using System;
using Telerik.Windows.Controls.ConversationalUI;

namespace Instant.Client.WPF.Models
{
    public class ChatMessageModel
    {
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public Author TelerikAuthor { get; set; }
        public string Text { get; set; }
        public DateTime DateSent { get; set; }
    }
}
