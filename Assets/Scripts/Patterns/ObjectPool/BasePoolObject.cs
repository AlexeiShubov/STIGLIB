using UnityEngine;

namespace STIGRADOR
{
    public abstract class BasePoolObject : MonoBehaviour, IPoolObject
    {
        protected IPool<IPoolObject> _pool;

        public virtual void Construct(IPool<IPoolObject> pool)
        {
            _pool = pool;
        }

        public virtual void Return()
        {
            _pool.ReturnObject(this);
        }
    }
}