namespace ShootingHero.LobbyServer
{
    public class ExitRoomRequest
    {
        public string RoomUUID { get; set; }
        public string Nickname { get; set; }
    }

    public class ExitRoomResponse
    {
        public bool Result { get; set; }
    }
}