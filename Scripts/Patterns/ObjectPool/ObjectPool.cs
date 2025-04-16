using System.Collections.Generic;

namespace STIGRADOR
{
    public abstract class ObjectPool<T> : IPool<IPoolObject> where T : IPoolObject
    {
        private readonly IFactory<T> _factory;
        private readonly Queue<T> _queue;

        protected ObjectPool(IFactory<T> factory)
        {
            _factory = factory;
            _queue = new Queue<T>();
        }

        public IPoolObject GetObject()
        {
            return _queue.Count > 0 ? _queue.Dequeue() : CreateObject();
        }

        public virtual void ReturnObject(IPoolObject obj)
        {
            _queue.Enqueue((T)obj);
        }

        protected virtual T CreateObject()
        {
            var poolObject = _factory.Create();

            poolObject.Construct(this);

            return poolObject;
        }
    }
}
