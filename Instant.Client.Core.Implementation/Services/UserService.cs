using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instant.Client.Core.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IStorageService storageService;

        public UserService(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public UserCredentials GetCurrentClientUserData()
        {
            var currentClientUserData = this.storageService.LoadData<UserCredentials>();
            return currentClientUserData?.FirstOrDefault();
        }

        public IList<User> GetUsers()
        {
            var users = this.storageService.LoadData<User>();
            return users;
        }

        public void SaveCurrentClientUserData(string login, string password)
        {
            var clientUserData = new UserCredentials
            {
                Login = login,
                Password = password
            };

            SaveCurrentClientUserData(clientUserData);
        }

        public void SaveCurrentClientUserData(UserCredentials clientUserData)
        {
            var currentClientData = this.storageService.LoadData<UserCredentials>()?.FirstOrDefault()
                ?? clientUserData;
            if (currentClientData != null)
            {
                currentClientData.Login = clientUserData.Login;
                currentClientData.Password = clientUserData.Password;
                currentClientData.LastOnline = clientUserData.LastOnline > currentClientData.LastOnline
                    ? clientUserData.LastOnline
                    : currentClientData.LastOnline;
            }
            this.storageService.SaveOrUpdateData(currentClientData);
        }

        public void UpdateClientLastOnline(DateTime lastOnline)
        {
            var currentClientData = this.storageService.LoadData<UserCredentials>();
            if (currentClientData?.FirstOrDefault() != null)
            {
                currentClientData.First().LastOnline = lastOnline;
            }
            this.storageService.WriteData(currentClientData);
        }

        public bool IsCurrentClientUser(string userLogin)
        {
            var clientUserData = GetCurrentClientUserData();
            return clientUserData == null
                || clientUserData.Login == userLogin;
        }

        public void ClearUserData()
        {
            this.storageService.ClearAllData();
        }

        public ChatPermission GetClientChatPermission(int chatId)
        {
            var chatPermissions = this.storageService.LoadData<ChatPermission>();
            var chatPermissionInChat = chatPermissions.FirstOrDefault(ch => ch.ChatId == chatId);
            return chatPermissionInChat;
        }

        public void SaveClientChatPermission(ChatPermission chatPermission)
        {
            this.storageService.SaveOrUpdateData(chatPermission);
        }

        public User GetUserData(string userLogin)
        {
            var users = GetUsers();
            var user = users?.FirstOrDefault(u => u.Login == userLogin);
            return user;
        }

        public void SaveUserData(User user)
        {
            this.storageService.SaveOrUpdateData(user);
        }

        public void DeleteClientChatPermission(int chatId)
        {
            var chatPermission = GetClientChatPermission(chatId);
            if (chatPermission != null)
            {
                this.storageService.DeleteData(chatPermission);
            }
        }
    }
}
