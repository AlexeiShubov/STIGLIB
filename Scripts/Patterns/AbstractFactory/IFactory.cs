namespace STIGRADOR
{
    public interface IFactory<out T>
    {
        public abstract T Create();
    }
}
