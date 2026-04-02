using System;

namespace ShootingHero.Networks
{
    public abstract class NetworkObject : IDIContainer
    {
        private readonly DIContainer diContainer = null;

        public NetworkObject(INetworkObjectBuilder builder)
        {
            diContainer = builder.GetDIContainer();
        }

        public TInstance GetInstance<TInstance>() where TInstance : class
        {
            return diContainer.GetInstance<TInstance>();
        }

        public object GetInstance(Type type)
        {
            return diContainer.GetInstance(type);
        }
    }
}