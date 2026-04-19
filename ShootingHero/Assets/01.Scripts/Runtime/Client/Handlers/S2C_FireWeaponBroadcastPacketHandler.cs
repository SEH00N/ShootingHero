using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_FireWeaponBroadcastPacket))]
    public class S2C_FireWeaponBroadcastPacketHandler : IPacketHandler<S2C_FireWeaponBroadcastPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_FireWeaponBroadcastPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_FireWeaponBroadcastPacket>.HandlePacket(Session session, S2C_FireWeaponBroadcastPacket packet)
        {
            if(ClientInstance.MyPlayerID == packet.PlayerID)
                ClientInstance.IsFireWeaponPacketProcessing = false;

            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();

            unit.transform.position = packet.Position;
            unit.UnitWeaponComponent.FireWeapon(packet.Direction);
            return new ValueTask();
        }
    }
}