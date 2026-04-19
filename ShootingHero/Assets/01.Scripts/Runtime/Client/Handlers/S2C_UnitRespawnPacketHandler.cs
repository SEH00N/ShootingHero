using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_UnitRespawnPacket))]
    public class S2C_UnitRespawnPacketHandler : IPacketHandler<S2C_UnitRespawnPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_UnitRespawnPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_UnitRespawnPacket>.HandlePacket(Session session, S2C_UnitRespawnPacket packet)
        {
            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();
            
            if(packet.PlayerID == ClientInstance.MyPlayerID)
                InputManager.EnableInput<PlayerInputReader>();
            
            unit.transform.position = packet.Position;
            unit.Respawn(packet.Height);
            return new ValueTask();
        }
    }
}