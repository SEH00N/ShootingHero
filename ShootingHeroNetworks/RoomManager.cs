using System;
using System.Collections.Concurrent;

namespace ShootingHero.Networks
{
    public class RoomManager : IRoomManager
    {
        private readonly ConcurrentDictionary<string, Lazy<Room>> rooms = null;
        private readonly Lazy<PacketSerializer> packetSerializer = null;

        public RoomManager(DIContainer diContainer)
        {
            rooms = new ConcurrentDictionary<string, Lazy<Room>>();
            packetSerializer = new Lazy<PacketSerializer>(() => diContainer.GetInstance<PacketSerializer>());
        }

        public Room Room(string roomID)
        {
            if(string.IsNullOrEmpty(roomID) == true)
                return null;
            
            Lazy<Room> room = rooms.GetOrAdd(roomID, _ => new Lazy<Room>(() => new Room(packetSerializer.Value)));
            return room.Value;
        }
    }
}