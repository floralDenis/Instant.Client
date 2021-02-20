using Instant.Client.WPF.ViewModels.ChatSettingsDialog;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class RemoveChatMemberFromGroupListCommand : ICommand
    {
        public ChatSettingsBaseViewModel ChatSettingsViewModel { get; set; }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.ChatSettingsViewModel != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Can not add empty user login to chat members list");
                }

                var chatMemberLogin = (string) parameter;
                this.ChatSettingsViewModel.ChatMembersLogins.Remove(chatMemberLogin);
            }
            catch (Exception)
            {
                MessageBox.Show("Can not remove user login from chat members list",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
