namespace STIGRADOR
{
    public class BaseMonoPool<T> : ObjectPool<T> where T : BasePoolObject
    {
        public BaseMonoPool(PoolMonoFactory<T> factory) : base(factory)
        {
        }
    }
}
