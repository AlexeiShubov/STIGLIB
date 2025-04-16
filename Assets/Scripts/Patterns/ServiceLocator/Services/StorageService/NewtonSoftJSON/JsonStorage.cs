using System;
using Newtonsoft.Json;
using UnityEngine;

namespace STIGRADOR
{
    public class JsonStorage : IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            
            PlayerPrefs.SetString(key, json);

            callback?.Invoke(true);
        }

        public void Load<T>(string key, Action<T> callback)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                callback?.Invoke(default);
                
                return;
            }

            var json = PlayerPrefs.GetString(key);
                
            var data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            callback.Invoke(data);
        }
    }
}
