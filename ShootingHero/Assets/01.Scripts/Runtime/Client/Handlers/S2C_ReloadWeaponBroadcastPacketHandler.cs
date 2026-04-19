using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_ReloadWeaponBroadcastPacket))]
    public class S2C_ReloadWeaponBroadcastPacketHandler : IPacketHandler<S2C_ReloadWeaponBroadcastPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_ReloadWeaponBroadcastPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_ReloadWeaponBroadcastPacket>.HandlePacket(Session session, S2C_ReloadWeaponBroadcastPacket packet)
        {
            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();
            
            unit.UnitWeaponComponent.ReloadWeapon();
            return new ValueTask();
        }
    }
}