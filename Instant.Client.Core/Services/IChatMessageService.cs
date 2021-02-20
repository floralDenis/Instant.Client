using Instant.Client.Core.Models;
using System.Collections.Generic;

namespace Instant.Client.Core.Services
{
    public interface IChatMessageService
    {
        ChatMessage GetChatMessage(int chatMessageId);
        IList<ChatMessage> GetChatMessages();
        IList<ChatMessage> GetChatMessages(int chatId);
        void AddChatMessage(ChatMessage chatMessage);
        void DeleteMessage(int chatMessageId);
        void DeleteMessages(int chatId);
    }
}
