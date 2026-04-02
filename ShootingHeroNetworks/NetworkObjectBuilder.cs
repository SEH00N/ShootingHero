using System;
using System.Reflection;

namespace ShootingHero.Networks
{
    public abstract partial class NetworkObjectBuilder<TNetworkObject> : INetworkObjectBuilder where TNetworkObject : NetworkObject
    {
        private readonly DIContainer diContainer = null;

        public NetworkObjectBuilder()
        {
            diContainer = new DIContainer();
        }

        DIContainer INetworkObjectBuilder.GetDIContainer()
        {
            return diContainer;
        }

        public NetworkObjectBuilder<TNetworkObject> AddSingleton<TInstance>(TInstance instance) where TInstance : class
        {
            diContainer.AddInstance<TInstance>(instance);
            return this;
        }

        public NetworkObjectBuilder<TNetworkObject> AddSingleton(Type type, object instance)
        {
            diContainer.AddInstance(type, instance);
            return this;
        }

        protected abstract TNetworkObject OnBuild();
        public TNetworkObject Build(params Assembly[] assemblies)
        {
            diContainer.AddInstance<PacketHandlerFactory>(PacketHandlerFactory.Builder.Build(assemblies, diContainer));
            diContainer.AddInstance<PacketSerializer>(PacketSerializer.Builder.Build(assemblies));

            TNetworkObject networkObject = OnBuild();
            return networkObject;
        }
    }
}