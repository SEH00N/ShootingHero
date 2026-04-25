using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_SpawnItemPacket))]
    public class S2C_SpawnItemPacketHandler : IPacketHandler<S2C_SpawnItemPacket>
    {
        private readonly GameManager gameManager = null;
        private readonly DataTableManager dataTableManager = null;

        public S2C_SpawnItemPacketHandler(GameManager gameManager, DataTableManager dataTableManager)
        {
            this.gameManager = gameManager;
            this.dataTableManager = dataTableManager;
        }

        ValueTask IPacketHandler<S2C_SpawnItemPacket>.HandlePacket(Session session, S2C_SpawnItemPacket packet)
        {
            ItemDataDTO itemData = packet.ItemData;
            ItemTableRow itemTableRow = dataTableManager.itemTable.GetRow(itemData.ItemID);
            if(itemTableRow == null)
                return new ValueTask();

            ItemBase item = Object.Instantiate(itemTableRow.itemPrefab, itemData.Position, Quaternion.identity);
            item.Initialize(itemData.ItemID, packet.ItemUUID, itemData.Height);
            gameManager.AddItem(packet.ItemUUID, item);

            return new ValueTask();
        }
    }
}