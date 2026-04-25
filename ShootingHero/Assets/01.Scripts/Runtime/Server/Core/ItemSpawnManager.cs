using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ShootingHero.Shared;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ShootingHero.Servers
{
    public class ItemSpawnManager
    {
        private readonly GameConfigTable gameConfigTable = null;
        private readonly ItemTable itemTable = null;
        private readonly SpawnPositionTable spawnPositionTable = null;
        private readonly GameServer gameServer = null;
        private readonly GameManager gameManager = null;

        public readonly Dictionary<ItemBase, SpawnPositionTableRow> spawnPositionMap = null;
        private readonly HashSet<SpawnPositionTableRow> spawnPositionFlags = null;

        public ItemSpawnManager(GameConfigTable gameConfigTable, ItemTable itemTable, SpawnPositionTable spawnPositionTable, GameServer gameServer, GameManager gameManager)
        {
            this.gameConfigTable = gameConfigTable;
            this.itemTable = itemTable;
            this.spawnPositionTable = spawnPositionTable;
            this.gameServer = gameServer;
            this.gameManager = gameManager;

            spawnPositionMap = new Dictionary<ItemBase, SpawnPositionTableRow>();
            spawnPositionFlags = new HashSet<SpawnPositionTableRow>();
        }

        public void Initialize()
        {
            for(int i = 0; i < gameConfigTable.GetWorldItemCount(); ++i)
                SpawnRandomItem();
        }

        private ItemBase SpawnRandomItem()
        {
            ItemTableRow itemTableRow = itemTable.PickRandom();
            SpawnPositionTableRow spawnPositionTableRow = GetRandomSpawnPositionTableRow();

            string uuid = Guid.NewGuid().ToString();
            ItemBase item = Object.Instantiate(itemTableRow.itemPrefab, spawnPositionTableRow.position, Quaternion.identity);
            item.Initialize(itemTableRow.id, uuid, spawnPositionTableRow.height, HandleItemDestory);

            gameManager.AddItem(uuid, item);

            return item;
        }

        private SpawnPositionTableRow GetRandomSpawnPositionTableRow()
        {
            const int MAX_LOOP_COUNT = 30;

            for(int i = 0; i < MAX_LOOP_COUNT; ++i)
            {
                SpawnPositionTableRow spawnPositionTableRow = spawnPositionTable.PickRandom();
                if(spawnPositionFlags.Contains(spawnPositionTableRow) == false)
                    return spawnPositionTableRow;
            }

            return spawnPositionTable.PickRandom();
        }

        private async void HandleItemDestory()
        {
            float delaySeconds = Random.Range(gameConfigTable.GetItemSpawnDelaySecondsMin(), gameConfigTable.GetItemSpawnDelaySecondsMax());
            await UniTask.Delay((int)(delaySeconds * 1000));

            ItemBase item = SpawnRandomItem();
            gameServer.Send(new S2C_SpawnItemPacket() { 
                ItemUUID = item.UUID,
                ItemData = new CreateItemData(item).itemData 
            });
        }
    }
}