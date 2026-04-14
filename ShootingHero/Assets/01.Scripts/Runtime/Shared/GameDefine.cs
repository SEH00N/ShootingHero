namespace ShootingHero.Shared
{
    public static class GameDefine
    {
        public enum ELayerMask : int
        {
            ItemLayer = 1 << 6
        }

        public const float UNIT_INTERACT_DISTANCE = 1.5f;
    }
}
