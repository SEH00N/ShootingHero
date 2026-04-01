using System;
using System.Collections.Generic;

namespace ShootingHero.Networks
{
    public partial class PacketHandlerFactory
    {
        private DIContainer diContainer = null;
        private Dictionary<Type, Func<DIContainer, IPacketHandlerBase>> factories = null;

        private PacketHandlerFactory()
        {
        }

        public IPacketHandlerBase Create(Type packetType)
        {
            if(factories.TryGetValue(packetType, out Func<DIContainer, IPacketHandlerBase> factory) == false)
                return null;
            
            IPacketHandlerBase packetHandler = factory(diContainer);
            return packetHandler;
        }
    }
}