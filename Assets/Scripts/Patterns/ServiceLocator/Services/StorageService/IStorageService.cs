using System;

namespace STIGRADOR
{
    public interface IStorageService
    {
        public abstract void Save(string key, object data, Action<bool> callback = null);
        public abstract void Load<T>(string key, Action<T> callback);
    }
}
