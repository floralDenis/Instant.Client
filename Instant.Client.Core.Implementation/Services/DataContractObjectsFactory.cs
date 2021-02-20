using Instant.Client.Core.Implementation.InstantServer.ChatService;
using System;

namespace Instant.Client.Core.Implementation.Services
{
    public class DataContractObjectsFactory
    {
        public AuthorizeUserOptions CreateAuthorizeUserOptions(
            string login, 
            string password, 
            DateTime lastOnline = default)
        {
            var options = new AuthorizeUserOptions();
            options.Login = login;
            options.Password = password;
            options.LastOnline = lastOnline;
            return options;
        }

        public AddOrUpdateChatPermissionOptions CreateAddOrUpdateChatPermissionOptions(
            string userLogin,
            int chatId,
            ChatPermissionTypes permissionType,
            string initiatorLogin)
        {
            var options = new AddOrUpdateChatPermissionOptions();
            options.ChatId = chatId;
            options.ChatMemberLogin = userLogin;
            options.PermissionType = permissionType;
            options.InitiatorLogin = initiatorLogin;
            return options;
        }

        public DeleteChatOptions CreateDeleteChatOptions(
            int chatId,
            string initiatorLogin)
        {
            var options = new DeleteChatOptions();
            options.ChatId = chatId;
            options.InitiatorLogin = initiatorLogin;
            return options;
        }
    }
}