namespace ShootingHero.Networks
{
    internal static class NetworkDefine
    {
        internal const int PACKET_SIZE_HEADER = sizeof(ushort);
        internal const int PACKET_MAX_SIZE = ushort.MaxValue;

        internal const int PACKET_ID_HEADER = sizeof(ushort);
    }
}