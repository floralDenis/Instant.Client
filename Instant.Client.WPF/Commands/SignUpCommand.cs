using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.ViewModels;
using System;
using System.Security.Authentication;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class SignUpCommand : ICommand
    {
        private readonly IServerChatService serverChatService;

        private readonly MainWindowViewModel mainWindowViewModel;

        public SignUpCommand(
            MainWindowViewModel mainWindowViewModel,
            IServerChatService serverChatService)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.serverChatService = serverChatService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.mainWindowViewModel != null
                && this.serverChatService != null;

        public void Execute(object parameter)
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
                string confirmedPassword = (string) credentials[2];

                this.serverChatService.SignUp(
                    login,
                    password,
                    confirmedPassword);

                this.mainWindowViewModel.CurrentViewType = ViewTypes.SignIn;
            }
            catch (Exception)
            {
                MessageBox.Show("Can not create account with given credentials!",
                    "Authorization Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
