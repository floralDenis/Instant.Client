using Instant.Client.WPF.Models;
using Telerik.Windows.Controls.ConversationalUI;

namespace Instant.Client.WPF.BusinessModels.Converters
{
    public class MessageConverter : IMessageConverter
    {
        public MessageBase ConvertItem(object item)
        {
            var messageModel = (ChatMessageModel) item;
            var textMessage = new TextMessage(messageModel.TelerikAuthor, messageModel.Text, messageModel.DateSent);
            return textMessage;
        }

        public object ConvertMessage(MessageBase message)
        {
            var textMessage = (TextMessage) message;
            var messageModel = new ChatMessageModel
            {
                TelerikAuthor = textMessage.Author,
                Text = textMessage.Text,
                DateSent = textMessage.CreationDate
            };

            return messageModel;
        }
    }
}
