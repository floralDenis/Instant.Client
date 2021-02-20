using Instant.Client.WPF.Enums;
using System.Windows.Input;

namespace Instant.Client.WPF.Services.Abstract
{
    public interface ICommandFactoryService
    {
        ICommand CreateCommand(CommandTypes commandType);
    }
}
