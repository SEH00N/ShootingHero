using System;

namespace ShootingHero.Networks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PacketHandlerAttribute : Attribute
    {
        private readonly Type packetType = null;
        public Type PacketType => packetType;

        public PacketHandlerAttribute(Type packetType)
        {
            this.packetType = packetType;
        }
    }
}