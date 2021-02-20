using Instant.Client.Core.Services;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class DeleteChatCommand : ICommand
    {
        private readonly IServerChatService serverChatService;

        public MainViewModel MainViewModel { get; set; }
        public Window Window { get; set; }

        public DeleteChatCommand(IServerChatService serverChatService)
        {
            this.serverChatService = serverChatService;
        }


        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.Window != null
                && this.MainViewModel != null
                && this.serverChatService != null;
        }

        public async void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(null))
                {
                    throw new NullReferenceException("Can not close dialog window, because it is null");
                }

                await this.serverChatService.DeleteChat(
                    this.MainViewModel.CurrentChat.ChatId, 
                    this.MainViewModel.CurrentUser.Login);
                this.Window.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Can not delete chat",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
