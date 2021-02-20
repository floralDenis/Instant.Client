using AutoMapper;
using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using Instant.Client.WPF.Models;
using Instant.Client.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.ConversationalUI;

namespace Instant.Client.WPF.Commands
{
    public class SwitchChatCommand : ICommand
    {
        private readonly IChatService chatService;
        private readonly IUserService userService;
        private readonly IServerChatService serverChatService;
        private readonly IMapper mapper;

        public MainViewModel MainViewModel { get; set; }

        public SwitchChatCommand(
            IChatService chatService,
            IUserService userService,
            IServerChatService serverChatService,
            IMapper mapper)
        {
            this.chatService = chatService;
            this.userService = userService;
            this.serverChatService = serverChatService;
            this.mapper = mapper;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return this.chatService != null
                && this.userService != null
                && this.MainViewModel != null;
        }

        public async void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    throw new ArgumentException("Change to unknown type of view requested");
                }

                var selectedChat = (ChatViewModel) parameter;

                if (selectedChat != null)
                {
                    var chatPermission = this.userService.GetClientChatPermission(selectedChat.ChatId);
                    this.MainViewModel.CurrentChatPermission = chatPermission;

                    var chatMembers = new ObservableCollection<ChatMemberModel>(await GetChatMembers(selectedChat.ChatId));
                    this.MainViewModel.MembersOfCurrentChat = chatMembers;

                    var messages = GetChatMessages(selectedChat.ChatId, chatMembers);
                    this.MainViewModel.MessagesInCurrentChat = messages;

                    this.MainViewModel.CurrentChat = selectedChat;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Can not switch chat!",
                    "Operation Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task<IList<ChatMemberModel>> GetChatMembers(int chatId)
        {
            var chatMemberLogins = this.chatService.GetMembersLoginsOfChat(chatId);

            var domainChatMembers = new List<User>();
            foreach (var chatMemberLogin in chatMemberLogins)
            {
                try
                {
                    var domainChatMember = this.userService.GetUserData(chatMemberLogin);
                    if (domainChatMember == null)
                    {
                        domainChatMember = await this.serverChatService.GetUserData(chatMemberLogin);
                        this.userService.SaveUserData(domainChatMember);
                    }

                    if (domainChatMember != null)
                    {
                        domainChatMembers.Add(domainChatMember);
                    }
                }
                catch (Exception exception)
                {
                    // Logging
                }
            }

            var chatMembers = this.mapper.Map<IList<ChatMemberModel>>(domainChatMembers);
            return chatMembers;
        }

        private ObservableCollection<ChatMessageModel> GetChatMessages(int chatId, IList<ChatMemberModel> chatMembers)
        {
            var domainMessages = this.chatService.GetMessagesInChat(chatId);
            var messages = new ObservableCollection<ChatMessageModel>(this.mapper.Map<IList<ChatMessageModel>>(domainMessages));
            foreach (var message in messages)
            {
                string senderLogin = domainMessages
                    .First(m => m.MessageId == message.MessageId)
                    .SenderLogin;

                var author = senderLogin == this.MainViewModel.CurrentUser.Login
                    ? this.MainViewModel.CurrentUser.TelerikAuthor
                    : chatMembers.FirstOrDefault(chm => chm.Login == senderLogin).TelerikAuthor
                        ?? new Author("DELETED");

                message.TelerikAuthor = author;
            }

            return messages;
        }
    }
}
