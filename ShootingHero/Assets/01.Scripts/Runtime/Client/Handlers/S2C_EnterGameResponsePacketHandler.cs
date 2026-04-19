using System.Collections.Generic;
using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_EnterGameResponsePacket))]
    public class S2C_EnterGameResponsePacketHandler : IPacketHandler<S2C_EnterGameResponsePacket>
    {
        private readonly GameManager gameManager = null;
        private readonly DataTableManager dataTableManager = null;

        public S2C_EnterGameResponsePacketHandler(GameManager gameManager, DataTableManager dataTableManager)
        {
            this.gameManager = gameManager;
            this.dataTableManager = dataTableManager;
        }

        ValueTask IPacketHandler<S2C_EnterGameResponsePacket>.HandlePacket(Session session, S2C_EnterGameResponsePacket packet)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            asyncOperation.completed += _ => {
                Unit unitPrefab = dataTableManager.gameConfigTable.GetRow("UnitPrefab").objectValue as Unit;

                foreach(KeyValuePair<string, UnitDataDTO> element in packet.Players)
                {
                    string playerID = element.Key;
                    UnitDataDTO unitData = element.Value;

                    Unit unit = Object.Instantiate(unitPrefab, unitData.Position, Quaternion.identity);
                    unit.Initialize(playerID, unitData.CurrentHP, unitData.CurrentWeaponID);
                    gameManager.AddPlayer(playerID, unit);
                }

                foreach(KeyValuePair<string, ItemDataDTO> element in packet.Items)
                {
                    string itemUUID = element.Key;
                    ItemDataDTO itemData = element.Value;

                    ItemTableRow itemTableRow = dataTableManager.itemTable.GetRow(itemData.ItemID);
                    if(itemTableRow == null)
                        continue;

                    ItemBase item = Object.Instantiate(itemTableRow.itemPrefab, itemData.Position, Quaternion.identity);
                    item.Initialize(itemData.ItemID, itemUUID, () => {
                        Object.Destroy(item.gameObject);
                        gameManager.RemoveItem(itemUUID);
                    });
                    gameManager.AddItem(itemUUID, item);
                }

                Unit myPlayer = gameManager.GetPlayer(packet.PlayerID);
                myPlayer.gameObject.AddComponent<UnitInputComponent>();

                ClientInstance.MainVCam.Follow = myPlayer.transform;
            };

            return new ValueTask();
        }
    }
}