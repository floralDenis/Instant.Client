using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class UpdateLastOnlineCommand : ICommand
    {
        private readonly IUserService userService;


        public UpdateLastOnlineCommand(
            IUserService userService)
        {
            this.userService = userService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => this.userService != null;

        public void Execute(object parameter)
        {
            try 
            { 
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Illegal credentials were provided");
                }

                this.userService.UpdateClientLastOnline(DateTime.Now);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to update client last online time",
                    "Update Client Data Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
