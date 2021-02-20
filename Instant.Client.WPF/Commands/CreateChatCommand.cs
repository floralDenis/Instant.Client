using Instant.Client.Core.Models;
using Instant.Client.Core.Models.Enums;
using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class CreateChatCommand : ICommand
    {
        private readonly IServerChatService serverChatService;

        public ChatSettingsDialogWindow Window { get; set; }

        public CreateChatCommand(IServerChatService serverChatService)
        {
            this.serverChatService = serverChatService;
        }


        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.Window != null
                && this.serverChatService != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(null))
                {
                    throw new NullReferenceException("Can not close dialog window, because it is null");
                }

                var chatCreationParameters = (object[]) parameter;
                var chatType = (ChatTypes) chatCreationParameters[0];
                var interlocutorUserLogin = (string) chatCreationParameters[1];
                var groupTitle = (string) chatCreationParameters[2];
                var chatMembersLogins = ((IEnumerable<string>) chatCreationParameters[3]).ToList();

                var chat = new Chat();
                chat.ChatType = chatType;

                switch (chatType)
                {
                    case ChatTypes.PrivateChat:
                        var membersList = new List<string> { interlocutorUserLogin };
                        chat.MembersLogins = membersList;
                        break;
                    case ChatTypes.PublicGroup:
                        chat.Title = groupTitle;
                        chat.MembersLogins = chatMembersLogins.ToList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Can not create chat of unknow type {((int) chatType).ToString()}");
                }
                this.serverChatService.CreateOrUpdateChat(chat);

                this.Window.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not create or update chat",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
