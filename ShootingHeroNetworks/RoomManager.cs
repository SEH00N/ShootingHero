using System;
using System.Collections.Concurrent;

namespace ShootingHero.Networks
{
    public class RoomManager : IRoomManager, ISessionDispatcher, IPacketDispatcher
    {
        private readonly ConcurrentDictionary<string, Lazy<Room>> rooms = null;
        private readonly Lazy<PacketSerializer> packetSerializer = null;

        public RoomManager(DIContainer diContainer)
        {
            rooms = new ConcurrentDictionary<string, Lazy<Room>>();
            packetSerializer = new Lazy<PacketSerializer>(() => diContainer.GetInstance<PacketSerializer>());
        }

        Room IRoomManager.Room(string roomID)
        {
            if(string.IsNullOrEmpty(roomID) == true)
                return null;
            
            Lazy<Room> room = rooms.GetOrAdd(roomID, _ => new Lazy<Room>(() => new Room(packetSerializer.Value)));
            return room.Value;
        }

        void ISessionDispatcher.Dispatch(Session session)
        {
            throw new NotImplementedException();
        }

        void IPacketDispatcher.Dispatch(Session session, IPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}