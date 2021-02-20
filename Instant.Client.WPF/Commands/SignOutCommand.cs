using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class SignOutCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IServerChatService serverChatService;

        private readonly MainWindowViewModel mainWindowViewModel;

        public SignOutCommand(
            IUserService userService,
            IServerChatService serverChatService,
            MainWindowViewModel mainWindowViewModel)
        {
            this.userService = userService;
            this.serverChatService = serverChatService;
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.mainWindowViewModel != null
                && this.userService != null;

        public void Execute(object parameter)
        {
            try 
            { 
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Illegal credentials were provided");
                }

                var currentClientUserData = this.userService.GetCurrentClientUserData();
                this.serverChatService.SignOut(currentClientUserData.Login, currentClientUserData.Password);
                this.userService.ClearUserData();

                this.mainWindowViewModel.CurrentViewType = ViewTypes.SignIn;
            }
            catch (Exception)
            {
                MessageBox.Show("Sign out operation failed",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
