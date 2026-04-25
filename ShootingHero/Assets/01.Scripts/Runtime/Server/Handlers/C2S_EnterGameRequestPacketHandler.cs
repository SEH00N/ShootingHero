using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_EnterGameRequestPacket))]
    public class C2S_EnterGameRequestPacketHandler : IPacketHandler<C2S_EnterGameRequestPacket>
    {
        private readonly GameManager gameManager = null;
        private readonly GameServer gameServer = null;
        private readonly DataTableManager dataTableManager = null;
        private readonly ServerDataTableManager serverDataTableManager = null;

        public C2S_EnterGameRequestPacketHandler(GameManager gameManager, GameServer gameServer, DataTableManager dataTableManager, ServerDataTableManager serverDataTableManager)
        {
            this.gameManager = gameManager;
            this.gameServer = gameServer;
            this.dataTableManager = dataTableManager;
            this.serverDataTableManager = serverDataTableManager;
        }

        ValueTask IPacketHandler<C2S_EnterGameRequestPacket>.HandlePacket(Session session, C2S_EnterGameRequestPacket packet)
        {
            string playerID = Guid.NewGuid().ToString();
            gameServer.AddPlayer(playerID, session);

            SpawnPositionTableRow spawnPositionTableRow = serverDataTableManager.unitSpawnPositionTable.PickRandom();
            Vector2 spawnPosition = spawnPositionTableRow?.position ?? Vector2.zero;
            int spawnHeight = spawnPositionTableRow?.height ?? 0;

            Unit unitPrefab = dataTableManager.gameConfigTable.GetUnitPrefab(packet.CharacterID);
            Unit unit = Object.Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            unit.Initialize(packet.CharacterID, playerID, spawnHeight, int.MaxValue, -1, null);
            gameManager.AddPlayer(playerID, unit);

            Dictionary<string, UnitDataDTO> players = new Dictionary<string, UnitDataDTO>();
            gameManager.ForEachPlayer((otherPlayerID, otherUnit) => {
                players[otherPlayerID] = new CreateUnitData(otherUnit).unitData;
            });

            Dictionary<string, ItemDataDTO> items = new Dictionary<string, ItemDataDTO>();
            gameManager.ForEachItem((itemUUID, item) => {
                items[itemUUID] = new CreateItemData(item).itemData;
            });

            S2C_EnterGameResponsePacket responsePacket = new S2C_EnterGameResponsePacket() {
                PlayerID = playerID,
                Players = players,
                Items = items
            };
            session.SendAsync(responsePacket);

            S2C_EnterGameBroadcastPacket broadcastPacket = new S2C_EnterGameBroadcastPacket() {
                PlayerID = playerID,
                UnitData = new CreateUnitData(unit).unitData
            };
            gameServer.Send(broadcastPacket, (sessionID, session) => sessionID != playerID);

            return new ValueTask();
        }
    }
}