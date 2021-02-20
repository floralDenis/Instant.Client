using Instant.Client.WPF.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private readonly AuthorizationTypes authorizationType;

        private string login;
        private string password;
        private string confirmedPassword;

        public string Login
        {
            get => this.login;
            set 
            {
                this.login = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmedPassword
        {
            get => confirmedPassword;
            set
            {
                this.confirmedPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand AuthorizationCommand { get; set; }
        public ICommand SwichToAlternativeAuthorizationCommand { get; set; }

        public object Credentials
        {
            get
            {
                object credentials;

                switch (this.authorizationType)
                {
                    case AuthorizationTypes.SignIn:
                        credentials = (this.login, this.password);
                        break;
                    case AuthorizationTypes.SignUp:
                        credentials = (this.login, this.password, this.confirmedPassword);
                        break;
                    default:
                        throw new ArgumentException($"Unknown authorization type provided: {this.authorizationType}");
                }

                return credentials;
            }
        }

        public AuthorizationViewModel(
            AuthorizationTypes authorizationType,
            ICommand authorizationCommand,
            ICommand switchTolternativeAuthorizationCommand)
        {
            this.authorizationType = authorizationType;
            this.AuthorizationCommand = authorizationCommand;
            this.SwichToAlternativeAuthorizationCommand = switchTolternativeAuthorizationCommand;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
