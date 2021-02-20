using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class SignInCommand : ICommand
    {
        private const string SUCCESS_SIGN_UP = "success";

        private readonly IUserService userService;
        private readonly IServerChatService serverChatService;

        private readonly MainWindowViewModel mainWindowViewModel;

        public SignInCommand(
            MainWindowViewModel mainWindowViewModel,
            IUserService userService,
            IServerChatService serverChatService)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.userService = userService;
            this.serverChatService = serverChatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.mainWindowViewModel != null
                && this.serverChatService != null;

        public async void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Illegal credentials were provided");
                }

                var credentials = (object[]) parameter;
                string login = (string) credentials[0];
                string password = (string) credentials[1];

                if (!this.userService.IsCurrentClientUser(login))
                {
                    this.userService.ClearUserData();
                }
                var userData = await this.serverChatService.SignIn(login, password);
                this.userService.SaveCurrentClientUserData(userData);

                this.mainWindowViewModel.CurrentViewType = ViewTypes.Main;
            }
            catch (Exception)
            {
                MessageBox.Show("Can not find user with given credentials!",
                    "Authorization Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
