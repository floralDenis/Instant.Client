using System;
using System.Collections.Generic;
using System.IO;
using Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using Newtonsoft.Json;

namespace Instant.Client.Core.Implementation.Services
{
    public class StorageService : IStorageService
    {
        private readonly string applicationDataPath;
        
        private const string APP_FOLDER_NAME = "\\";
        private const string APP_CREDENTIALS_FILENAME = "Credentials.txt";
        private const string APP_USERS_FILENAME = "Users.txt";
        private const string APP_CHATS_FILENAME = "UserChats.txt";
        private const string APP_MESSAGES_FILENAME = "UserChatsMessages.txt";
        private const string APP_USER_CHAT_PERMISSIONS_FILENAME = "UserChatPermissions.txt";
        
        public StorageService()
        {
            this.applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + APP_FOLDER_NAME;
        }

        private string GetFilePathForType(Type type)
        {
            string filepath = this.applicationDataPath;
            if (type == typeof(UserCredentials)) filepath += APP_CREDENTIALS_FILENAME;
            else if (type == typeof(Chat)) filepath += APP_CHATS_FILENAME;
            else if (type == typeof(ChatPermission)) filepath += APP_USER_CHAT_PERMISSIONS_FILENAME;
            else if (type == typeof(ChatMessage)) filepath += APP_MESSAGES_FILENAME;
            else if (type == typeof(User)) filepath += APP_USERS_FILENAME;
            else
            {
                throw new InvalidOperationException("Can not decide filepath, unknown type");
            }

            return filepath;
        }

        public void SaveData<T>(T data)
        {
            var existingData = LoadData<T>() ?? new List<T>();
            existingData.Add(data);
            
            WriteData(existingData);
        }

        public void WriteData<T>(List<T> data)
        {
            string filepath = GetFilePathForType(typeof(T));
            if (!File.Exists(filepath))
            {
                var fileStream = File.Create(filepath);
                fileStream.Close();
            }
            
            string contentToBeAdded = JsonConvert.SerializeObject(data);
            File.WriteAllText(filepath, contentToBeAdded);
        }

        public void SaveOrUpdateData<T>(T data)
        {
            var existingData = LoadData<T>() ?? new List<T>();
            var dataIndex = existingData.IndexOf(data);

            if (dataIndex >= 0)
            {
                existingData[dataIndex] = data;
            }
            else
            {
                existingData.Add(data);
            }
            
            WriteData(existingData);
        }

        public List<T> LoadData<T>()
        {
            string filepath = GetFilePathForType(typeof(T));
            if (!File.Exists(filepath)) return new List<T>();

            string fileContent = File.ReadAllText(filepath);

            var data = JsonConvert.DeserializeObject<List<T>>(fileContent);
            return data;
        }

        public void DeleteData<T>(T data)
        {
            var existingData = LoadData<T>();
            if (!existingData.Contains(data))
            {
                throw new ArgumentException($"Can not delete {typeof(T)} record, because it was not found");
            }

            existingData.Remove(data);
            WriteData(existingData);
        }

        public void ClearAllData()
        {
            File.WriteAllText(this.applicationDataPath + APP_CHATS_FILENAME, string.Empty);
            File.WriteAllText(this.applicationDataPath + APP_USERS_FILENAME, string.Empty);
            File.WriteAllText(this.applicationDataPath + APP_MESSAGES_FILENAME, string.Empty);
            File.WriteAllText(this.applicationDataPath + APP_CREDENTIALS_FILENAME, string.Empty);
            File.WriteAllText(this.applicationDataPath + APP_USER_CHAT_PERMISSIONS_FILENAME, string.Empty);
        }
    }
}