using Instant.Client.Core.Models;
using Instant.Client.Core.Models.Enums;
using Instant.Client.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace Instant.Client.Core.Implementation.Services
{
    public class ChatService : IChatService
    {
        private readonly IStorageService storageService;
        private readonly IUserService userService;
        private readonly IChatMessageService chatMessageService;

        public ChatService(
            IStorageService storageService,
            IUserService userService,
            IChatMessageService chatMessageService)
        {
            this.storageService = storageService;
            this.userService = userService;
            this.chatMessageService = chatMessageService;
        }

        public void AddOrUpdateChat(Chat chat)
        {
            this.storageService.SaveOrUpdateData(chat);
        }

        public Chat GetChat(int chatId)
        {
            var chats = this.storageService.LoadData<Chat>();
            var chat = chats.FirstOrDefault(ch => ch.ChatId == chatId);
            return chat;
        }

        public void DeleteChat(int chatId)
        {
            var chat = GetChat(chatId);
            if (chat != null)
            {
                this.userService.DeleteClientChatPermission(chatId);
                this.chatMessageService.DeleteMessages(chatId);
                this.storageService.DeleteData(chat);
            }
        }

        public IList<Chat> GetAvailableChats()
        {
            var currentClientUser = this.userService.GetCurrentClientUserData();

            var availableChats = storageService.LoadData<Chat>();
            var availablePrivateChats = availableChats
                ?.Where(ch => ch.ChatType == ChatTypes.PrivateChat)
                .ToList();

            if (availablePrivateChats != null)
            {
                availablePrivateChats
                    .ToList()
                    .ForEach(pch => pch.Title = pch.MembersLogins
                        .First(ml => ml != currentClientUser.Login));
            }

            return availableChats;
        }

        public IList<string> GetMembersLoginsOfChat(int chatId)
        {
            var availableChats = GetAvailableChats();            
            var selectedChat = availableChats.First(c => c.ChatId == chatId);
            return selectedChat.MembersLogins;
        }

        public IList<ChatMessage> GetMessagesInChat(int chatId)
        {
            var chatMessages = chatMessageService.GetChatMessages()
                ?? new List<ChatMessage>();
            var messagesInChat = chatMessages
                .Where(m => m.ChatId == chatId)
                .ToList();
            return messagesInChat;
        }
    }
}
