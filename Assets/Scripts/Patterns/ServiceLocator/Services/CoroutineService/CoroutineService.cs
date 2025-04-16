using System.Collections;
using UnityEngine;

namespace STIGRADOR
{
    public sealed class CoroutineService : MonoBehaviour, IService
    {
        private static CoroutineService _instance;
        
        public static CoroutineService Instance
        {
            get
            {
                if (_instance == null)
                {
                    var newGameObject = new GameObject("[CoroutineService]");
                    
                    _instance = newGameObject.AddComponent<CoroutineService>();
                    
                    DontDestroyOnLoad(newGameObject);
                }

                return _instance;
            }
        }

        public void StopAllRoutines()
        {
            Instance.StopAllCoroutines();
        }

        public Coroutine StartRoutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public void StopRoutine(Coroutine enumerator)
        {
            if (enumerator != null)
            {
                Instance.StopCoroutine(enumerator);
            }
        }
    }
}
