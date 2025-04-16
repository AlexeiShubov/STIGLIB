using UnityEngine;

namespace STIGRADOR
{
    public class PoolMonoFactory<T> : GameObjectFactory<T> where T : BasePoolObject
    {
        private readonly T _prefab;
        private readonly Transform _container;

        public PoolMonoFactory(T prefab, Transform container) : base(prefab)
        {
            _prefab = prefab;
            _container = container;
        }

        public override T Create()
        {
            return Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _container);
        }
    }
}
