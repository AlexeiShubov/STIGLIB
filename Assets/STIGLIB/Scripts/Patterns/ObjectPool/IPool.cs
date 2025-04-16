namespace STIGRADOR
{
    public interface IPool<T> where T : IPoolObject
    {
        public abstract T GetObject();
        public abstract void ReturnObject(T obj);
    }
}
