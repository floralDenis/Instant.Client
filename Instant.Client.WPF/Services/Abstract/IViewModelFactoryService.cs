using Instant.Client.WPF.Enums;
using System.Windows;

namespace Instant.Client.WPF.Services.Abstract
{
    public interface IViewModelFactoryService
    {
        object CreateViewModel(ViewTypes viewType);
        object CreateDialogViewModel(ChatSettingsDialogViewTypes viewType, Window dialogWindow);
    }
}
