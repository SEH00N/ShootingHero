using System;

namespace ShootingHero.Networks
{
    public abstract class NetworkObject
    {
        private readonly DIContainer diContainer = null;

        public NetworkObject(INetworkObjectBuilder builder)
        {
            diContainer = builder.GetDIContainer();
        }

        public TInstance GetSingleton<TInstance>() where TInstance : class
        {
            return diContainer.GetInstance<TInstance>();
        }

        public object GetSingleton(Type type)
        {
            return diContainer.GetInstance(type);
        }
    }
}