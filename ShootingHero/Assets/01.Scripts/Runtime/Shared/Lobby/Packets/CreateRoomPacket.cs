namespace ShootingHero.LobbyServer
{
    public class CreateRoomRequest
    {
        public string Nickname { get; set; }
    }

    public class CreateRoomResponse
    {
        public string RoomUUID { get; set; }
    }
}