namespace ShootingHero.LobbyServer
{
    public class JoinRoomRequest
    {
        public string RoomUUID { get; set; }
        public string Nickname { get; set; }
    }

    public class JoinRoomResponse
    {
        public bool Result { get; set; }
    }
}