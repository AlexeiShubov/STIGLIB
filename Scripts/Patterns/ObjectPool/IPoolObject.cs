namespace STIGRADOR
{
    public interface IPoolObject
    {
        public abstract void Construct(IPool<IPoolObject> pool);
        public abstract void Return();
    }
}
