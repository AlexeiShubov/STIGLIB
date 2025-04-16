using System;
using UnityEngine;

namespace STIGRADOR
{
    public class PlayerPrefsJsonStorage : JsonStorageBase
    {
        public virtual void Load<T>(string key, Action<T> callback)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                callback?.Invoke(default);
                
                return;
            }
            
            base.Load(key, callback);
        }

        protected override void SaveJson(string key, string json)
        {
            PlayerPrefs.SetString(key, json);
        }

        protected override string GetJson(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}
