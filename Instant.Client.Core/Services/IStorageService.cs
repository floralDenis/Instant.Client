using System.Collections.Generic;

namespace Instant.Client.Core.Services
{
    public interface IStorageService
    {
        void SaveData<T>(T data);
        void WriteData<T>(List<T> data);
        void SaveOrUpdateData<T>(T data);
        List<T> LoadData<T>();
        void DeleteData<T>(T data);
        void ClearAllData();
    }
}