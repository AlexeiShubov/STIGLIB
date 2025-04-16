namespace STIGRADOR
{
    public interface IServiceLocator<T>
    {
        public abstract void ServiceRegister<TP>(TP service) where TP : T;
        public abstract TP GetService<TP>() where TP : T;
    }
}
