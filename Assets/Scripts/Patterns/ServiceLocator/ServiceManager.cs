using UnityEngine;

namespace STIGRADOR
{
    public class ServiceManager : BaseServiceManager
    {
        private static ServiceManager _instance;

        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var newGameObject = new GameObject("[ServiceManager]");

                    _instance = newGameObject.AddComponent<ServiceManager>();
                    _instance.Initialize();

                    DontDestroyOnLoad(newGameObject);
                }

                return _instance;
            }
        }

        public override T GetService<T>()
        {
            return _instance._serviceLocator.GetService<T>();
        }

        public override void ServiceRegister(IService service)
        {
            _serviceLocator.ServiceRegister(service);
        }

        public override void UnRegisterService<T>()
        {
            _instance._serviceLocator.UnRegisterService<T>();
        }
    }
}
