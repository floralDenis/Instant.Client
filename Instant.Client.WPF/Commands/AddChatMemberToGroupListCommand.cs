using Instant.Client.WPF.ViewModels.ChatSettingsDialog;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class AddChatMemberToGroupListCommand : ICommand
    {
        public ChatSettingsBaseViewModel ChatSettingsViewModel { get; set; }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            string chatUserLogin = (string) parameter;
            return this.ChatSettingsViewModel != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                string chatUserLogin = (string) parameter;

                if (!CanExecute(parameter)
                    || string.IsNullOrEmpty(chatUserLogin)
                    || this.ChatSettingsViewModel.ChatMembersLogins.Contains(chatUserLogin))
                {
                    throw new ArgumentException("Can not add empty or already existing user login to chat members list");
                }

                var chatMemberLogin = (string) parameter;
                this.ChatSettingsViewModel.ChatMembersLogins.Add(chatMemberLogin);
            }
            catch (Exception)
            {
                MessageBox.Show("Can not add empty or already existing user login to chat members list",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
