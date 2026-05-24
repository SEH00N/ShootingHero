namespace ShootingHero.Shared
{
    public static class GameDefine
    {
        public enum ELayerMask
        {
            ItemLayer = 1 << 6
        }

        public const string LOBBY_SERVER_CONNECTION = "http://localhost:9696";
    }
}
