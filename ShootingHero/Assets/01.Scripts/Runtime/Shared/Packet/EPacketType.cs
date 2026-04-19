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
        C2S_FireWeaponPacket,
        S2C_FireWeaponBroadcastPacket,
        S2C_UnitDamagedPacket,
        C2S_ReloadWeaponPacket,
        S2C_ReloadWeaponBroadcastPacket,
        S2C_UnitDeadPacket,
        S2C_UnitRespawnPacket
    }
}