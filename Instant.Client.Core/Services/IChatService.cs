using Instant.Client.Core.Models;
using System.Collections.Generic;

namespace Instant.Client.Core.Services
{
    public interface IChatService
    {
        Chat GetChat(int chatId);
        IList<ChatMessage> GetMessagesInChat(int chatId);
        IList<string> GetMembersLoginsOfChat(int chatId);
        IList<Chat> GetAvailableChats();
        void AddOrUpdateChat(Chat chat);
        void DeleteChat(int chatId);
    }
}
