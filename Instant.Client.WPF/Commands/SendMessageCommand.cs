using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.ConversationalUI;

namespace Instant.Client.WPF.Commands
{
    public class SendMessageCommand : ICommand
    {
        public MainViewModel MainViewModel { private get; set; }

        private readonly IServerChatService serverChatService;

        public SendMessageCommand(IServerChatService serverChatService)
        {
            this.serverChatService = serverChatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.serverChatService != null
                && this.MainViewModel != null;

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Failed to send message");
                }

                var eventParameter = (SendMessageEventArgs) parameter;
                var domainChatMessage = ConvertTelerikMessageToDomain(eventParameter.Message);
                this.serverChatService.SendMessage(domainChatMessage);

                eventParameter.Handled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Can not send message!",
                    "Authorization Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private ChatMessage ConvertTelerikMessageToDomain(MessageBase message)
        {
            ChatMessage domainChatMessage;

            switch (message.MessageType)
            {
                case MessageType.Text:
                    var textMessage = (TextMessage) message;
                    domainChatMessage = new ChatMessage
                    {
                        Text = textMessage.Text,
                        ChatId = this.MainViewModel.CurrentChat.ChatId,
                        SenderLogin = this.MainViewModel.CurrentUser.Login,
                        DateSent = DateTime.Now
                    };
                    break;
                default:
                    throw new ArgumentException($"Unsupported message type : {message.MessageType}");
            }

            return domainChatMessage;
        }
    }
}
