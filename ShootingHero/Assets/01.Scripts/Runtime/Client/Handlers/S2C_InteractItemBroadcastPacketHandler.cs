using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_InteractItemBroadcastPacket))]
    public class S2C_InteractItemBroadcastPacketHandler : IPacketHandler<S2C_InteractItemBroadcastPacket>
    {
        private readonly GameManager gameManager = null;

        public S2C_InteractItemBroadcastPacketHandler(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        ValueTask IPacketHandler<S2C_InteractItemBroadcastPacket>.HandlePacket(Session session, S2C_InteractItemBroadcastPacket packet)
        {
            Unit unit = gameManager.GetPlayer(packet.PlayerID);
            if(unit == null)
                return new ValueTask();
            
            ItemBase item = gameManager.GetItem(packet.ItemUUID);
            if(unit == null)
                return new ValueTask();
            
            item.Interact(unit);
            return new ValueTask();
        }
    }
}