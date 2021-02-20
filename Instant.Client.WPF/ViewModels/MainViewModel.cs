using AutoMapper;
using Instant.Client.Core.EventConnectors;
using Instant.Client.Core.Models;
using Instant.Client.Core.Models.Enums;
using Instant.Client.Core.Services;
using Instant.Client.WPF.Commands;
using Instant.Client.WPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Instant.Client.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IChatService chatService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        private ChatMemberModel currentUser;
        private ChatViewModel currentChat;
        private ChatPermission currentChatPermission;
        private IList<ChatMessageModel> messagesInCurrentChat;

        public ChatMemberModel CurrentUser 
        { 
            get => this.currentUser; 
            set
            {
                this.currentUser = value;
                OnPropertyChanged();
            }
        }
        public ChatViewModel CurrentChat 
        {
            get => this.currentChat;
            set
            {
                this.currentChat = value;
                OnPropertyChanged();
            }
        }
        public ChatPermission CurrentChatPermission 
        {
            get => this.currentChatPermission;
            set
            {
                this.currentChatPermission = value;
                OnPropertyChanged();
                OnPropertyChanged("IsMessageInputEnabled");
            }
        }

        public ObservableCollection<ChatMemberModel> MembersOfCurrentChat { get; set; }
        public ObservableCollection<ChatMessageModel> MessagesInCurrentChat 
        { 
            get => (ObservableCollection<ChatMessageModel>) this.messagesInCurrentChat; 
            set
            {
                this.messagesInCurrentChat = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ChatViewModel> AvailableChatPreviews { get; set; }
        
        public bool IsMessageInputEnabled
        {
            get
            {
                return this.CurrentChatPermission?.PermissionType == ChatPermissionTypes.ReadWrite
                    || this.CurrentChatPermission?.PermissionType == ChatPermissionTypes.Moderate
                    || this.CurrentChatPermission?.PermissionType == ChatPermissionTypes.Administrate;
            }
        }

        public ICommand SendMessageCommand { get; set; }
        public ICommand ShowChatCreationDialogCommand { get; set; }
        public ICommand SwitchChatCommand { get; set; }
        public ICommand SignOutCommand { get; set; }

        public MainViewModel(
            ICommand sendMessageCommand,
            ICommand showChatCreationDialogCommand,
            ICommand switchChatCommand,
            ICommand signOutCommand,
            IChatService chatService,
            IUserService userService,
            IMapper mapper)
        {
            this.chatService = chatService;
            this.userService = userService;
            this.mapper = mapper;

            ConfigureAndSetCommands(
                sendMessageCommand, 
                showChatCreationDialogCommand,
                switchChatCommand,
                signOutCommand);
            ConfigureChatEvents();
            LoadData();
        }

        private void LoadData()
        {
            var domainAvailableChats = this.chatService.GetAvailableChats();
            this.AvailableChatPreviews = new ObservableCollection<ChatViewModel>(
                this.mapper.Map<IList<ChatViewModel>>(domainAvailableChats));

            var domainUser = this.userService.GetCurrentClientUserData();
            this.CurrentUser = this.mapper.Map<ChatMemberModel>(domainUser);
        }

        private void ConfigureAndSetCommands(
            ICommand sendMessageCommand,
            ICommand showChatCreationDialogCommand,
            ICommand switchChatCommand,
            ICommand signOutCommand)
        {
            var castedSendMessageCommand = (SendMessageCommand) sendMessageCommand;
            castedSendMessageCommand.MainViewModel = this;
            this.SendMessageCommand = castedSendMessageCommand;

            var castedShowChatCreationDialogCommand = (ShowChatCreationDialogCommand) showChatCreationDialogCommand;
            castedShowChatCreationDialogCommand.MainViewModel = this;
            this.ShowChatCreationDialogCommand = castedShowChatCreationDialogCommand;

            var castedSwitchChatCommand = (SwitchChatCommand) switchChatCommand;
            castedSwitchChatCommand.MainViewModel = this;
            this.SwitchChatCommand = castedSwitchChatCommand;

            this.SignOutCommand = signOutCommand;
        }

        private void ConfigureChatEvents()
        {
            ChatEventsConnector.AddOrUpdateChat += (chat) =>
            {
                _ = chat ?? throw new ArgumentNullException("Can not add or update chat, because it is null");

                var chatModel = this.mapper.Map<ChatViewModel>(chat);
                if (string.IsNullOrEmpty(chatModel.Title))
                {
                    chatModel.Title = chat.MembersLogins.First();
                }

                var existingChatPreview = this.AvailableChatPreviews
                    .FirstOrDefault(chm => chm.ChatId == chatModel.ChatId);
                if (existingChatPreview != null)
                {
                    existingChatPreview.Title = chatModel.Title;
                }
                else
                {
                    this.AvailableChatPreviews.Add(chatModel);
                }
            };

            ChatEventsConnector.ReceiveMessage += (chatMessage) =>
            {
                _ = chatMessage ?? throw new ArgumentNullException("Can not add chat message, because it is null");

                if (this.currentChat != null
                    && chatMessage.ChatId == this.CurrentChat.ChatId)
                {
                    var chatMessageModel = this.mapper.Map<ChatMessageModel>(chatMessage);
                    if (chatMessage.SenderLogin == this.currentUser.Login)
                    {
                        chatMessageModel.TelerikAuthor = this.currentUser.TelerikAuthor;
                    }
                    else
                    {
                        var messageSender = this.MembersOfCurrentChat.First(m => m.Login == chatMessage.SenderLogin);
                        chatMessageModel.TelerikAuthor = messageSender.TelerikAuthor;
                    }

                    if (this.MessagesInCurrentChat == null)
                    {
                        this.messagesInCurrentChat = new ObservableCollection<ChatMessageModel>();
                    }
                    this.MessagesInCurrentChat.Add(chatMessageModel);
                }
            };

            ChatEventsConnector.DeleteMessage += (chatMessageId) =>
            {
                if (this.MessagesInCurrentChat.Any(m => m.MessageId == chatMessageId))
                {
                    var chatMessageModel = this.MessagesInCurrentChat.First(m => m.MessageId == chatMessageId);
                    this.MessagesInCurrentChat.Remove(chatMessageModel);
                }
            };

            ChatEventsConnector.DeleteChat += (chatId) =>
            {
                var removedChat = this.AvailableChatPreviews.FirstOrDefault(ch => ch.ChatId == chatId);
                if (removedChat != null)
                {
                    if (this.currentChat != null
                        && removedChat.ChatId == this.currentChat.ChatId)
                    {
                        this.MessagesInCurrentChat = null;
                    }

                    this.AvailableChatPreviews.Remove(removedChat);
                }
            };
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
