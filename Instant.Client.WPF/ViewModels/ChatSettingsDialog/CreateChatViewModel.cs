using Instant.Client.Core.Models.Enums;
using Instant.Client.WPF.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels.ChatSettingsDialog
{
    public class CreateChatViewModel : ChatSettingsBaseViewModel
    {
        public ICommand CreateChatCommand { get; set; }
        public ICommand RemoveChatMemberFromGroupListCommand { get; set; }

        public CreateChatViewModel(
            ICommand cancelCommand,
            ICommand addChatMemberToGroupListCommand,
            ICommand createChatCommand,
            ICommand removeChatMemberFromGroupListCommand
            ) : base(cancelCommand, addChatMemberToGroupListCommand)
        {
            this.ChatType = ChatTypes.PrivateChat;

            this.CreateChatCommand = createChatCommand;
            var castedRemoveChatMemberToGroupListCommand = (RemoveChatMemberFromGroupListCommand) removeChatMemberFromGroupListCommand;
            castedRemoveChatMemberToGroupListCommand.ChatSettingsViewModel = this;
            this.RemoveChatMemberFromGroupListCommand = castedRemoveChatMemberToGroupListCommand;
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
