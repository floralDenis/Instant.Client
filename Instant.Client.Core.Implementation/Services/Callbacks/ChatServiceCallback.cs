using AutoMapper;
using Instant.Client.Core.EventConnectors;
using Instant.Client.Core.Implementation.InstantServer.ChatService;
using Instant.Client.Core.Models;
using Instant.Client.Core.Services;

namespace Instant.Client.Core.Implementation.Services.Callbacks
{
    public class ChatServiceCallback : IChatServiceCallback
    {
        private readonly Core.Services.IChatService chatService;
        private readonly IChatMessageService chatMessageService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        
        public ChatServiceCallback(
            Core.Services.IChatService chatService,
            IChatMessageService chatMessageService,
            IUserService userService,
            IMapper mapper)
        {
            this.chatService = chatService;
            this.chatMessageService = chatMessageService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public void ReceiveMessage(SendMessageOptions sendMessageOptions)
        {
            var chatMessage = this.mapper.Map<ChatMessage>(sendMessageOptions);
            this.chatMessageService.AddChatMessage(chatMessage);

            ChatEventsConnector.OnReceiveMessage(chatMessage);
        }

        public void AddOrUpdateChat(CreateOrUpdateChatOptions options)
        {
            var chat = this.mapper.Map<Chat>(options);
            this.chatService.AddOrUpdateChat(chat);

            var chatPermissionOptions = (AddOrUpdateChatPermissionOptions) options.ExtraData;
            var chatPermission = this.mapper.Map<ChatPermission>(chatPermissionOptions);
            this.userService.SaveClientChatPermission(chatPermission);

            ChatEventsConnector.OnAddOrUpdateChat(chat);
            ChatEventsConnector.OnUpdateChatPermission(chatPermission);
        }

        public void UpdateChatPermission(AddOrUpdateChatPermissionOptions options)
        {
            var chatPermission = this.mapper.Map<ChatPermission>(options);
            if (this.userService.IsCurrentClientUser(options.ChatMemberLogin))
            {
                this.userService.SaveClientChatPermission(chatPermission);
            }

            ChatEventsConnector.OnUpdateChatPermission(chatPermission);
        }

        public void RemoveChat(int chatId)
        {
            this.chatService.DeleteChat(chatId);

            ChatEventsConnector.OnDeleteChat(chatId);
        }

        public void RemoveMessage(int chatMessageId)
        {
            this.chatMessageService.DeleteMessage(chatMessageId);

            ChatEventsConnector.OnDeleteMessage(chatMessageId);
        }
    }
}