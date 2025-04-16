using System;
using Newtonsoft.Json;

namespace STIGRADOR
{
    public abstract class JsonStorageBase : IStorageService
    {
        public virtual void Save(string key, object data, Action<bool> callback = null)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            SaveJson(key, json);

            callback?.Invoke(true);
        }

        public virtual void Load<T>(string key, Action<T> callback)
        {
            var json = GetJson(key);
                
            var data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            callback.Invoke(data);
        }

        protected abstract void SaveJson(string key, string json);

        protected abstract string GetJson(string key);
    }
}
