using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MemoryPack;

namespace ShootingHero.Networks
{
    public partial class PacketSerializer
    {
        public static class Builder
        {
            public static PacketSerializer Build(Assembly[] assemblies)
            {
                PacketSerializer packetSerializer = new PacketSerializer() {
                    packetIDMap = new Dictionary<Type, ushort>(),
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

                    packetSerializer.packetIDMap[packetType] = packetAttribute.PacketID;
                    packetSerializer.factories[packetAttribute.PacketID] = (packetData) => CreatePacket(packetType, packetData);
                }
                
                return packetSerializer;
            }

            private static IPacket CreatePacket(Type packetType, ArraySegment<byte> packetData)
            {
                return MemoryPackSerializer.Deserialize(packetType, packetData) as IPacket;
            }
        }
    }
}