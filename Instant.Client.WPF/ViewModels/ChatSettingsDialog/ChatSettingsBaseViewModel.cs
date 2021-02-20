using Instant.Client.Core.Models.Enums;
using Instant.Client.WPF.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels.ChatSettingsDialog
{
    public class ChatSettingsBaseViewModel : INotifyPropertyChanged
    {
        private ChatTypes chatType;
        public ChatTypes ChatType
        {
            get => chatType;
            set
            {
                this.chatType = value;
                OnPropertyChanged();
            }
        }

        private string interlocutorUserLogin;
        public string InterlocutorUserLogin
        {
            get => this.interlocutorUserLogin;
            set
            {
                this.interlocutorUserLogin = value;
                OnPropertyChanged();
            }
        }

        private string groupTitle;
        public string GroupTitle
        {
            get => this.groupTitle;
            set
            {
                this.groupTitle = value;
                OnPropertyChanged();
            }
        }

        private string selectedgroupMemberLogin;
        public string SelectedGroupMemberLogin
        {
            get => this.selectedgroupMemberLogin;
            set
            {
                this.selectedgroupMemberLogin = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ChatMembersLogins { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand AddChatMemberToGroupListCommand { get; set; }

        public ChatSettingsBaseViewModel(
            ICommand cancelCommand,
            ICommand addChatMemberToGroupListCommand)
        {
            this.ChatType = chatType;
            this.ChatMembersLogins = new ObservableCollection<string>();

            ConfigureCommands(cancelCommand,
                addChatMemberToGroupListCommand);
        }

        private void ConfigureCommands(
            ICommand cancelCommand,
            ICommand addChatMemberToGroupList)
        {
            this.CancelCommand = cancelCommand;

            ((AddChatMemberToGroupListCommand) addChatMemberToGroupList).ChatSettingsViewModel = this;
            this.AddChatMemberToGroupListCommand = addChatMemberToGroupList;
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
