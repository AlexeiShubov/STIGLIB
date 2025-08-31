using System;
using System.Collections.Generic;

namespace STIGRADOR.MVVM
{
    public abstract class BaseModel
    {
        private readonly Dictionary<string, object> _dataObject = new Dictionary<string, object>();

        public EventManager EventManager { get; }

        public BaseModel(EventManager eventManager)
        {
            EventManager = eventManager;
        }

        public void Set<T>(string name, T value, bool invokeEvent = true)
        {
            if (_dataObject.ContainsKey(name))
            {
                _dataObject[name] = value;
            }
            else
            {
                _dataObject.Add(name, value);
            }
            
            if (!invokeEvent) return;
            
            EventManager.Invoke($"On{name}Changed", value);
        }

        public T Get<T>(string name, T defaultValue = default)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }
            
            return (T)_dataObject[name];
        }
        
        public void Inc(string name, int value = 1)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                Set(name, value);
            }
            else
            {
                var obj = _dataObject[name];
                
                switch (obj)
                {
                    case long l:
                        Set(name, l + value);
                        break;
                    case int i:
                        Set(name, i + value);
                        break;
                    case float f:
                        Set(name, f + value);
                        break;
                    case double d:
                        Set(name, d + value);
                        break;
                }
            }
        }

        public void Dec(string name, int value = 1)
        {
            Inc(name, -value);
        }
        
        #region Specialized getters

        public bool GetBool(string name, bool defaultValue = false)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToBoolean(_dataObject[name]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public int GetInt(string name, int defaultValue = 0)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(_dataObject[name]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public float GetFloat(string name, float defaultValue = 0f)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSingle(_dataObject[name]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public long GetLong(string name, long defaultValue = 0L)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(_dataObject[name]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public double GetDouble(string name, double defaultValue = 0.0)
        {
            if (!_dataObject.ContainsKey(name) || _dataObject[name] == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(_dataObject[name]);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion
    }
}