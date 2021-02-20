using Instant.Client.Core.Models;
using System;
using System.Collections.Generic;

namespace Instant.Client.Core.Services
{
    public interface IUserService
    {
        UserCredentials GetCurrentClientUserData();
        void SaveCurrentClientUserData(string login, string password);
        void SaveCurrentClientUserData(UserCredentials clientUserData);
        void UpdateClientLastOnline(DateTime lastOnline);
        ChatPermission GetClientChatPermission(int chatId);
        void SaveClientChatPermission(ChatPermission chatPermission);
        void DeleteClientChatPermission(int chatId);
        bool IsCurrentClientUser(string userLogin);
        IList<User> GetUsers();
        User GetUserData(string userLogin);
        void SaveUserData(User user);
        void ClearUserData();
    }
}
