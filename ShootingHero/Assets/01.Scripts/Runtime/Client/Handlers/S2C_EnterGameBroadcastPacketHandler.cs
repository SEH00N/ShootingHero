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
        private readonly DataTableManager dataTableManager = null;

        public S2C_EnterGameBroadcastPacketHandler(GameManager gameManager, DataTableManager dataTableManager)
        {
            this.gameManager = gameManager;
            this.dataTableManager = dataTableManager;
        }

        ValueTask IPacketHandler<S2C_EnterGameBroadcastPacket>.HandlePacket(Session session, S2C_EnterGameBroadcastPacket packet)
        {
            Unit unitPrefab = dataTableManager.gameConfigTable.GetRow("UnitPrefab").objectValue as Unit;
            Unit unit = Object.Instantiate(unitPrefab, packet.UnitData.Position, Quaternion.identity);
            unit.Initialize(packet.PlayerID, packet.UnitData.Height, packet.UnitData.CurrentHP, packet.UnitData.CurrentWeaponID, packet.UnitData.CurrentWeaponStatus);
            gameManager.AddPlayer(packet.PlayerID, unit);

            return new ValueTask();
        }
    }
}