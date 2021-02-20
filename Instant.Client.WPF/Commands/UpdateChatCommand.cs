using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using Instant.Client.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class UpdateChatCommand : ICommand
    {
        private readonly IServerChatService serverChatService;

        public MainViewModel MainViewModel { get; set; }

        public Window Window { get; set; }

        public UpdateChatCommand(IServerChatService serverChatService)
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

                var chatUpdateParameters = (object[]) parameter;
                string groupTitle = (string) chatUpdateParameters[0];
                var membersLogins = ((IEnumerable<string>) chatUpdateParameters[1]).ToList();

                var chat = new Chat();
                chat.Title = groupTitle;
                chat.MembersLogins = membersLogins;
                chat.ChatId = this.MainViewModel.CurrentChat.ChatId;
                chat.ChatType = this.MainViewModel.CurrentChat.ChatType;

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
