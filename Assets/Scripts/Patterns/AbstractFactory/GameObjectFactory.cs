using UnityEngine;

namespace STIGRADOR
{
    public class GameObjectFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private readonly T _prefab;

        public GameObjectFactory(T prefab)
        {
            _prefab = prefab;
        }

        public virtual T Create()
        {
            return Object.Instantiate(_prefab);
        }
    }
}
