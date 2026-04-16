namespace ShootingHero.Networks
{
    internal enum EPacketType
    {
        C2S_TestPacket,
        S2C_TestPacket,
        C2S_EnterGameRequestPacket,
        S2C_EnterGameResponsePacket,
        S2C_EnterGameBroadcastPacket,
        C2S_MoveInputPacket,
        S2C_MoveInputBroadcastPacket,
        S2C_SpawnItemPacket,
        C2S_InteractItemPacket,
        S2C_InteractItemBroadcastPacket,
    }
}