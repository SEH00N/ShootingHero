using System;
using System.Collections.Generic;

namespace ShootingHero.Networks
{
    public partial class PacketHandlerFactory
    {
        private Dictionary<Type, Func<DIContainer, IPacketHandlerBase>> factories = null;

        private PacketHandlerFactory()
        {
        }

        public IPacketHandlerBase Create(Type packetType, DIContainer diContainer)
        {
            if(factories.TryGetValue(packetType, out Func<DIContainer, IPacketHandlerBase> factory) == false)
                return null;
            
            IPacketHandlerBase packetHandler = factory(diContainer);
            return packetHandler;
        }
    }
}