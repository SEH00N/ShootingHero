using System.Collections.Generic;

namespace ShootingHero.LobbyServer
{
    public class UpdateRoomCommandRequest
    {
        public string RoomUUID { get; set; }
        public int Cursor { get; set; }
    }

    public class UpdateRoomCommandResponse
    {
        public List<RoomCommand> RoomCommands { get; set; }
    }
}