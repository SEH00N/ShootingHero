using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_EnterGameBroadcastPacket))]
    public class S2C_EnterGameBroadcastPacketHandler : IPacketHandler<S2C_EnterGameBroadcastPacket>
    {
        private readonly GameManager gameManager = null;
        private readonly Unit unitPrefab = null;

        public S2C_EnterGameBroadcastPacketHandler(GameManager gameManager, Unit unitPrefab)
        {
            this.gameManager = gameManager;
            this.unitPrefab = unitPrefab;
        }

        ValueTask IPacketHandler<S2C_EnterGameBroadcastPacket>.HandlePacket(Session session, S2C_EnterGameBroadcastPacket packet)
        {
            Unit unit = Object.Instantiate(unitPrefab, packet.Position, Quaternion.identity);
            gameManager.AddPlayer(packet.PlayerID, unit);

            return new ValueTask();
        }
    }
}