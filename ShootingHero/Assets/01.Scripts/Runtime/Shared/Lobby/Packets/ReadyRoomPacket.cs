namespace ShootingHero.LobbyServer
{
    public class ReadyRoomRequest
    {
        public string RoomUUID { get; set; }
        public string Nickname { get; set; }
    }

    public class ReadyRoomResponse
    {
    }
}