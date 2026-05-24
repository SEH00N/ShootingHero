namespace ShootingHero.LobbyServer
{
    public class GetGameConnectionRequest
    {
        public string GameUUID { get ; set; }
    }

    public class GetGameConnectionResponse
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}