using Instant.Client.WPF.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class CloseWindowCommand : ICommand
    {
        public Window Window { get; set; }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.Window != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(null))
                {
                    throw new NullReferenceException("Can not close dialog window, because it is null");
                }

                this.Window.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not cancel operation",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
