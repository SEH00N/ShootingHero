using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_UnitDeadPacket))]
    public class S2C_UnitDeadPacketHandler : IPacketHandler<S2C_UnitDeadPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_UnitDeadPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_UnitDeadPacket>.HandlePacket(Session session, S2C_UnitDeadPacket packet)
        {
            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();
            
            unit.gameObject.SetActive(false);
            return new ValueTask();
        }
    }
}