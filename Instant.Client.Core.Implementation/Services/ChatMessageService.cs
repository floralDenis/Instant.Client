using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace Instant.Client.Core.Implementation.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IStorageService storageService;

        public ChatMessageService(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public ChatMessage GetChatMessage(int chatMessageId)
        {
            var chatMessages = this.storageService.LoadData<ChatMessage>();
            var chatMessage = chatMessages.FirstOrDefault(chm => chm.MessageId == chatMessageId);
            return chatMessage;
        }

        public IList<ChatMessage> GetChatMessages()
        {
            var chatMessages = this.storageService.LoadData<ChatMessage>();
            return chatMessages;
        }

        public IList<ChatMessage> GetChatMessages(int chatId)
        {
            var availableChatMessages = this.storageService.LoadData<ChatMessage>();
            var chatMessagesFromTargetChat = availableChatMessages?
                .Where(m => m.ChatId == chatId)
                .ToList();

            return chatMessagesFromTargetChat;
        }

        public void AddChatMessage(ChatMessage chatMessage)
        {
            this.storageService.SaveData(chatMessage);   
        }

        public void DeleteMessage(int chatMessageId)
        {
            var chatMessage = this.GetChatMessage(chatMessageId);
            if (chatMessage != null)
            {
                this.storageService.DeleteData(chatMessage);
            }
        }

        public void DeleteMessages(int chatId)
        {
            var chatMessages = this.GetChatMessages(chatId);
            if (chatMessages != null && chatMessages.Count > 0)
            {
                foreach (var chatMessage in chatMessages)
                {
                    this.storageService.DeleteData(chatMessage);
                }
            }
        }
    }
}
