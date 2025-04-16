using System;
using System.Collections.Generic;

namespace STIGRADOR
{
    public class ServiceLocator<T> : IServiceLocator<T>
    {
        private readonly Dictionary<Type, T> _serviceMap;

        public ServiceLocator()
        {
            _serviceMap = new Dictionary<Type, T>();
        }

        public void ServiceRegister<TP>(TP service) where TP : T
        {
            var type = service.GetType();

            if (_serviceMap.ContainsKey(type))
            {
                _serviceMap[type] = service;

                throw new Exception($"Service {type} already exist!");
            }

            _serviceMap.Add(type, service);
        }

        public void UnRegisterService<TP>() where TP : T
        {
            var type = typeof(TP);
            
            if (!_serviceMap.ContainsKey(type))
            {
                throw new Exception($"Type {type} is not found!");
            }

            _serviceMap.Remove(type);
        }

        public TP GetService<TP>() where TP : T
        {
            var type = typeof(TP);

            if (!_serviceMap.ContainsKey(type))
            {
                throw new Exception($"Type {type} is not found!");
            }

            return (TP)_serviceMap[type];
        }
    }
}

