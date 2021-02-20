using Instant.Client.Core.Models.Enums;
using Instant.Client.Core.Services;
using Instant.Client.WPF.ViewModels;
using Instant.Client.WPF.ViewModels.ChatSettingsDialog;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class PromoteChatMemberCommand : ICommand
    {
        private readonly IServerChatService serverChatService;

        public MainViewModel MainViewModel { get; set; }
        
        public UpdateChatViewModel UpdateChatViewModel { get; set; }

        public PromoteChatMemberCommand(IServerChatService serverChatService)
        {
            this.serverChatService = serverChatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.MainViewModel != null
                && this.serverChatService != null;

        public async void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Illegal credentials were provided");
                }

                int chatId = this.MainViewModel.CurrentChat.ChatId;
                string initiatorLogin = this.MainViewModel.CurrentUser.Login;
                var parameters = (object[]) parameter;
                string userLogin = (string) parameters[0];
                userLogin = string.IsNullOrEmpty(userLogin)
                    ? initiatorLogin
                    : userLogin;
                var permissionType = (ChatPermissionTypes) parameters[1];

                await this.serverChatService.PromoteChatMember(
                    userLogin,
                    chatId,
                    permissionType,
                    initiatorLogin);

                if (permissionType == ChatPermissionTypes.Banned ||
                    permissionType == ChatPermissionTypes.None)
                {
                    this.UpdateChatViewModel.ChatMembersLogins.Remove(userLogin);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not promote user",
                    "Authorization Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
