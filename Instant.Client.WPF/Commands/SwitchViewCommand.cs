using Instant.Client.WPF.Enums;
using Instant.Client.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class SwitchViewCommand : ICommand
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        private ViewTypes targetViewType;
        public ViewTypes TargetViewType
        {
            get => this.targetViewType;
            set
            {
                this.targetViewType = value;
            }
        }

        public SwitchViewCommand(
            MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return Enum.IsDefined(typeof(ViewTypes), targetViewType.ToString())
                && this.mainWindowViewModel != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Change to unknown type of view requested");
                }

                this.mainWindowViewModel.CurrentViewType = targetViewType;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
