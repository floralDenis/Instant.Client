using Instant.Client.Core.Models;

namespace Instant.Client.Core.EventConnectors
{
    public static class ChatEventsConnector
    {
        public delegate void ReceiveMessageHandler(ChatMessage chatMessage);
        public static event ReceiveMessageHandler ReceiveMessage;

        public delegate void DeleteMessageHandler(int messageId);
        public static event DeleteMessageHandler DeleteMessage;

        public delegate void AddOrUpdateChatHandler(Chat chat);
        public static event AddOrUpdateChatHandler AddOrUpdateChat;

        public delegate void DeleteChatHandler(int messageId);
        public static event DeleteChatHandler DeleteChat;

        public delegate void AddOrUpdateUserHandler(User user);
        public static event AddOrUpdateUserHandler AddOrUpdateUser;

        public delegate void UpdateChatPermissionHandler(ChatPermission chatPermission);
        public static event UpdateChatPermissionHandler UpdateChatPermission;

        public static void OnReceiveMessage(ChatMessage chatMessage)
        {
            ReceiveMessage?.Invoke(chatMessage);
        }

        public static void OnDeleteMessage(int chatMessageId)
        {
            DeleteMessage?.Invoke(chatMessageId);
        }

        public static void OnAddOrUpdateChat(Chat chat)
        {
            AddOrUpdateChat?.Invoke(chat);
        }

        public static void OnDeleteChat(int chatId)
        {
            DeleteChat?.Invoke(chatId);
        }

        public static void OnAddOrUpdateUser(User user)
        {
            AddOrUpdateUser?.Invoke(user);
        }

        public static void OnUpdateChatPermission(ChatPermission chatPermission)
        {
            UpdateChatPermission?.Invoke(chatPermission);
        }
    }
}
