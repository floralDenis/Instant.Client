using Instant.Client.Core.Services;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.Services.Abstract;
using Instant.Client.WPF.Services.Implementation;
using Ninject;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IViewModelFactoryService viewModelFactoryService;
        private readonly ICommandFactoryService commandFactoryService;

        private ViewTypes currentViewType;
        private object currentViewModel;

        public ViewTypes CurrentViewType 
        {
            get => currentViewType;
            set
            {
                SwitchCurrentViewModel(value);

                currentViewType = value;
                OnPropertyChanged();
            }
        }

        public object CurrentViewModel 
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateLastOnlineCommand { get; set; }

        public MainWindowViewModel()
        {
            var ninjectKernel = SetUpNinjectKernel();

            this.commandFactoryService = new CommandFactoryService(ninjectKernel, this);
            this.viewModelFactoryService = new ViewModelFactoryService(ninjectKernel, this.commandFactoryService);
            this.UpdateLastOnlineCommand = this.commandFactoryService.CreateCommand(CommandTypes.UpdateLastOnlineCommand);

            this.CurrentViewType = ViewTypes.SignIn;
        }

        private IKernel SetUpNinjectKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        private void SwitchCurrentViewModel(ViewTypes targetViewType)
        {
            try
            {
                var targetViewModel = this.viewModelFactoryService.CreateViewModel(targetViewType);
                this.CurrentViewModel = targetViewModel;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
