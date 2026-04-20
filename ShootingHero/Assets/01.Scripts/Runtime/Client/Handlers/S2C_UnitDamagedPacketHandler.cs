using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_UnitDamagedPacket))]
    public class S2C_UnitDamagedPacketHandler : IPacketHandler<S2C_UnitDamagedPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_UnitDamagedPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_UnitDamagedPacket>.HandlePacket(Session session, S2C_UnitDamagedPacket packet)
        {
            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();
            
            Unit attackerUnit = null;
            if(string.IsNullOrEmpty(packet.AttackerID) == false)
                attackerUnit = gameManager.GetPlayer(packet.AttackerID);
            
            unit.UnitHealthComponent.GetDamage(attackerUnit, packet.Damage);
            return new ValueTask();
        }
    }
}