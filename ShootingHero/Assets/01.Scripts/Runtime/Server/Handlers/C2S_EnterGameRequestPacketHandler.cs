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
        private readonly GameServer gameServer = null;
        private readonly Server server = null;
        private readonly DataTableManager dataTableManager = null;
        private readonly ServerDataTableManager serverDataTableManager = null;

        public C2S_EnterGameRequestPacketHandler(GameServer gameServer, Server server, DataTableManager dataTableManager, ServerDataTableManager serverDataTableManager)
        {
            this.gameServer = gameServer;
            this.server = server;
            this.dataTableManager = dataTableManager;
            this.serverDataTableManager = serverDataTableManager;
        }

        ValueTask IPacketHandler<C2S_EnterGameRequestPacket>.HandlePacket(Session session, C2S_EnterGameRequestPacket packet)
        {
            string playerID = Guid.NewGuid().ToString();
            server.Rooms.Room(ServerDefine.ROOM_ID).Add(playerID, session);

            SpawnPositionTableRow spawnPositionTableRow = serverDataTableManager.unitSpawnPositionTable.PickRandom();
            Vector2 spawnPosition = spawnPositionTableRow?.position ?? Vector2.zero;
            int spawnHeight = spawnPositionTableRow?.height ?? 0;

            Unit unitPrefab = dataTableManager.gameConfigTable.GetRow("UnitPrefab").objectValue as Unit;
            Unit unit = Object.Instantiate(unitPrefab, gameServer.transform);
            unit.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            unit.Initialize(playerID, spawnHeight, int.MaxValue, -1, null);
            gameServer.AddPlayer(session, playerID, unit);

            Dictionary<string, UnitDataDTO> players = new Dictionary<string, UnitDataDTO>();
            gameServer.ForEachPlayer((otherPlayerID, otherUnit) => {
                players[otherPlayerID] = CreateUnitData(otherUnit);
            });

            Dictionary<string, ItemDataDTO> items = new Dictionary<string, ItemDataDTO>();
            gameServer.ForEachItem((itemUUID, item) => {
                items[itemUUID] = CreateItemData(item);
            });

            S2C_EnterGameResponsePacket responsePacket = new S2C_EnterGameResponsePacket() {
                PlayerID = playerID,
                Players = players,
                Items = items
            };
            session.SendAsync(responsePacket);

            S2C_EnterGameBroadcastPacket broadcastPacket = new S2C_EnterGameBroadcastPacket() {
                PlayerID = playerID,
                UnitData = CreateUnitData(unit)
            };
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(broadcastPacket, (sessionID, session) => sessionID != playerID);

            return new ValueTask();
        }

        private static UnitDataDTO CreateUnitData(Unit unit)
        {
            Vector2 position = unit.transform.position;
            int height = unit.GetHeight();
            int currentHP = unit.UnitHealthComponent.CurrentHP;
            WeaponBase weapon = unit.UnitWeaponComponent.Weapon;
            int weaponID = weapon == null ? -1 : weapon.WeaponID;
            string weaponStatus = weapon == null ? null : weapon.GetStatus();

            return new UnitDataDTO() {
                Position = position,
                Height = height,
                CurrentHP = currentHP,
                CurrentWeaponID = weaponID,
                CurrentWeaponStatus = weaponStatus
            };
        }

        private static ItemDataDTO CreateItemData(ItemBase item)
        {
            return new ItemDataDTO() {
                ItemID = item.ItemID, 
                Position = item.transform.position
            };
        }
    }
}