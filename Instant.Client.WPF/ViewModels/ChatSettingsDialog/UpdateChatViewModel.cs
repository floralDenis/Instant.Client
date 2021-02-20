using Instant.Client.WPF.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels.ChatSettingsDialog
{
    public class UpdateChatViewModel : ChatSettingsBaseViewModel
    {
        public ICommand UpdateChatCommand { get; set; }
        public ICommand PromoteChatMemeberCommand { get; set; }
        public ICommand DeleteChatCommand { get; set; }

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel 
        {
            set
            {
                this.mainViewModel = value;

                this.ChatType = mainViewModel.CurrentChat.ChatType;
                ((PromoteChatMemberCommand)this.PromoteChatMemeberCommand).UpdateChatViewModel = this;
                ((PromoteChatMemberCommand) this.PromoteChatMemeberCommand).MainViewModel = this.mainViewModel;
                ((DeleteChatCommand) this.DeleteChatCommand).MainViewModel = this.mainViewModel;
                ((UpdateChatCommand) this.UpdateChatCommand).MainViewModel = this.mainViewModel;

                switch (this.ChatType)
                {
                    case Core.Models.Enums.ChatTypes.PrivateChat:
                        base.InterlocutorUserLogin = this.mainViewModel.CurrentChat.Title;
                        break;
                    case Core.Models.Enums.ChatTypes.PublicGroup:
                        base.GroupTitle = this.mainViewModel.CurrentChat.Title;
                        base.ChatMembersLogins = new ObservableCollection<string>(this.mainViewModel.MembersOfCurrentChat
                            .Select(m => m.Login));
                        break;
                }
            }
        }

        public UpdateChatViewModel(
            ICommand cancelCommand,
            ICommand addChatMemberToGroupListCommand,
            ICommand updateChatCommand,
            ICommand promoteChatMemeberCommand,
            ICommand deleteChatCommand
            ) : base(cancelCommand, addChatMemberToGroupListCommand)
        {
            this.UpdateChatCommand = updateChatCommand;
            this.PromoteChatMemeberCommand = promoteChatMemeberCommand;
            this.DeleteChatCommand = deleteChatCommand;
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
