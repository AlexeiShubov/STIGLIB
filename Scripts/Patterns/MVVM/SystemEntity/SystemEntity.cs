namespace STIGRADOR.MVVM
{
    public class SystemEntity    
    {
        public SystemModel SystemModel { get; }
        public ScopeModel ScopeModel { get; }
        public Binder SystemBinder { get; }
        public Binder ScopeBinder { get; }
        public SystemEventManager SystemInvoker { get; }
        public ScopeEventManager ScopeInvoker { get; }

        public SystemEntity(
            SystemModel systemModel, 
            ScopeModel scopeModel,
            Binder systemBinder,
            Binder scopeBinder,
            SystemEventManager systemEventManager, 
            ScopeEventManager scopeEventManager
            )
        {
            SystemModel = systemModel;
            ScopeModel = scopeModel;
            SystemBinder = systemBinder;
            ScopeBinder = scopeBinder;
            SystemInvoker = systemEventManager;
            ScopeInvoker = scopeEventManager;
        }
    }
}