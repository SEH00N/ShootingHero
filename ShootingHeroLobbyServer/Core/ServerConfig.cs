namespace ShootingHero.LobbyServer
{
    public class ServerConfig
    {
        public string Host { get; set; }
        public int PortQueueRangeMin { get; set; }
        public int PortQueueRangeMax { get; set; }
        public string ExecutablePath { get; set; }
    }
}