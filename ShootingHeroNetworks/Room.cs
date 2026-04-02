using System;

namespace ShootingHero.Networks
{
    public class Room
    {
        private readonly PacketSerializer packetSerializer = null;

        public Room(PacketSerializer packetSerializer)
        {
            this.packetSerializer = packetSerializer;
        }

        public void Add(string sessionID, Session session)
        {   
        }

        public void Remove(string sessionID)
        {
        }

        public void Send(IPacket packet, Func<string, Session, bool> filter = null)
        {
        }

        public Session Session(string sessionID)
        {
        }
    }
}