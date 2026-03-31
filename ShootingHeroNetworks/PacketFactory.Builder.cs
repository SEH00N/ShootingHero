using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MemoryPack;

namespace ShootingHero.Networks
{
    public partial class PacketFactory
    {
        public static class Builder
        {
            public static PacketFactory Build(Assembly[] assemblies)
            {
                PacketFactory packetFactory = new PacketFactory() {
                    factories = new Dictionary<ushort, Func<ArraySegment<byte>, IPacket>>()
                };

                Type[] packetTypes = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => typeof(IPacket).IsAssignableFrom(t))
                        .Where(t => t.IsDefined(typeof(PacketAttribute), false))
                        .Where(t => t.IsDefined(typeof(MemoryPackableAttribute), false))
                        .Where(t => t.IsAbstract == false && t.IsInterface == false)
                        .ToArray();

                foreach (Type packetType in packetTypes)
                {
                    PacketAttribute packetAttribute = packetType.GetCustomAttribute<PacketAttribute>(false);
                    if(packetAttribute == null)
                        continue;

                    packetFactory.factories[packetAttribute.PacketID] = (packetData) => CreatePacket(packetType, packetData);
                }
                
                return packetFactory;
            }

            private static IPacket CreatePacket(Type packetType, ArraySegment<byte> packetData)
            {
                return MemoryPackSerializer.Deserialize(packetType, packetData) as IPacket;
            }
        }
    }
}