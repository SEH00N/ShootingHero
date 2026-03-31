using System;

namespace ShootingHero.Networks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PacketAttribute : Attribute
    {
        private readonly ushort packetID = 0;
        public ushort PacketID => packetID;

        public PacketAttribute(ushort packetID)
        {
            this.packetID = packetID;
        }
    }
}