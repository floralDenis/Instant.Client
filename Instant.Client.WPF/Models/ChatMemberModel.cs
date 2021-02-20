using Telerik.Windows.Controls.ConversationalUI;

namespace Instant.Client.WPF.Models
{
    public class ChatMemberModel
    {
        public string Login { get; set; }

        public Author TelerikAuthor { get; set; }

        public ChatMemberModel(
            string login)
        {
            this.Login = login;

            this.TelerikAuthor = new Author(this.Login);
        }
    }
}
