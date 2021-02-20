using Instant.Client.WPF.Enums;
using Instant.Client.WPF.Services.Abstract;
using Instant.Client.WPF.ViewModels;
using Instant.Client.WPF.ViewModels.ChatSettingsDialog;
using Instant.Client.WPF.Views.ChatSettingsDialog;
using Instant.Client.WPF.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Instant.Client.WPF.Commands
{
    public class ShowChatCreationDialogCommand : ICommand
    {
        public IViewModelFactoryService ViewModelFactoryService;

        public MainViewModel MainViewModel { get; set; }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.ViewModelFactoryService != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new NullReferenceException("Can not create chat, because some services are null");
                }

                var chatOperationType = (ChatOperationTypes)parameter;

                var dialogBox = new ChatSettingsDialogWindow();
                switch (chatOperationType)
                {
                    case ChatOperationTypes.CreateChat:
                    {
                        var dialogViewModel = this.ViewModelFactoryService.CreateDialogViewModel(
                            ChatSettingsDialogViewTypes.CreateChat,
                            dialogBox);
                        dialogBox.DataContext = dialogViewModel;
                        dialogBox.Content = new CreateChatDialogView();
                        break;
                    }
                    case ChatOperationTypes.UpdateChat:
                    {
                        var dialogViewModel = this.ViewModelFactoryService.CreateDialogViewModel(
                            ChatSettingsDialogViewTypes.UpdateChat,
                            dialogBox);
                            ((UpdateChatViewModel)dialogViewModel).MainViewModel = this.MainViewModel;
                            dialogBox.DataContext = dialogViewModel;
                            dialogBox.Content = new UpdateChatDialogView();
                        break;
                    }
                }

                dialogBox.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not create chat",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
