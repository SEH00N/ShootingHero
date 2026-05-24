using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ShootingHero.LobbyServer
{
    public class RoomManager : Room.ICallback
    {
        private readonly ConcurrentDictionary<string, Room> roomList = null;
        private readonly GameInstanceManager gameInstanceManager = null;

        public RoomManager(GameInstanceManager gameInstanceManager)
        {
            this.gameInstanceManager = gameInstanceManager;
            roomList = new ConcurrentDictionary<string, Room>();
        }

        public string CreateRoom()
        {
            string uuid;
            
            while(true)
            {
                uuid = Guid.NewGuid().ToString();
                if(roomList.TryAdd(uuid, null) == true)
                    break;
            }

            roomList[uuid] = new Room(this);
            return uuid;
        }

        public bool TryGetRoom(string uuid, out Room room)
        {
            return roomList.TryGetValue(uuid, out room);
        }

        Task<string> Room.ICallback.StartGameAsync()
        {
            return gameInstanceManager.PublishGameAsync();
        }
    }
}