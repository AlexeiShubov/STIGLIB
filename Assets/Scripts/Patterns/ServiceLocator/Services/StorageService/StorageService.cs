using System;

namespace STIGRADOR
{
    public sealed class StorageService : IService
    {
        private readonly IStorageService _storageService;

        public StorageService()
        {
            _storageService = new JsonStorage();
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
            _storageService.Save(key, data, callback);
        }

        public void Load<T>(string key, Action<T> callback)
        {
            _storageService.Load(key, callback);
        }
    }
}
