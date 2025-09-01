namespace STIGRADOR.MVVM
{
    public interface IInvoker
    {
        void Invoke(string eventName);
        void Invoke<A>(string eventName, A arg);
        void Invoke<A, B>(string eventName, A arg1, B arg2);
        void Invoke<A, B, C>(string eventName, A arg1, B arg2, C arg3);
    }
}