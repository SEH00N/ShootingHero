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

                foreach(KeyValuePair<string, Vector2> element in packet.Players)
                {
                    Unit unit = Object.Instantiate(unitPrefab, element.Value, Quaternion.identity);
                    gameManager.AddPlayer(element.Key, unit);
                }

                foreach(KeyValuePair<string, (int itemID, Vector2 position)> element in packet.Items)
                {
                    ItemTableRow itemTableRow = dataTableManager.itemTable.GetRow(element.Value.itemID);
                    if(itemTableRow == null)
                        continue;

                    ItemBase item = Object.Instantiate(itemTableRow.itemPrefab, element.Value.position, Quaternion.identity);
                    item.Initialize(element.Value.itemID, element.Key, () => {
                        Object.Destroy(item.gameObject);
                        gameManager.RemoveItem(element.Key);
                    });
                    gameManager.AddItem(element.Key, item);
                }

                Unit myPlayer = gameManager.GetPlayer(packet.PlayerID);
                myPlayer.gameObject.AddComponent<UnitInputComponent>();
            };

            return new ValueTask();
        }
    }
}