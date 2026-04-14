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
        private readonly Unit unitPrefab = null;

        public C2S_EnterGameRequestPacketHandler(GameServer gameServer, Server server, Unit unitPrefab)
        {
            this.gameServer = gameServer;
            this.server = server;
            this.unitPrefab = unitPrefab;
        }

        ValueTask IPacketHandler<C2S_EnterGameRequestPacket>.HandlePacket(Session session, C2S_EnterGameRequestPacket packet)
        {
            string playerID = Guid.NewGuid().ToString();
            server.Rooms.Room(ServerDefine.ROOM_ID).Add(playerID, session);

            Unit unit = Object.Instantiate(unitPrefab, gameServer.transform);
            unit.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            gameServer.AddPlayer(playerID, unit);

            Dictionary<string, Vector2> players = new Dictionary<string, Vector2>();
            gameServer.ForEachPlayer((otherPlayerID, otherUnit) => {
                players[otherPlayerID] = otherUnit.transform.position;
            });

            S2C_EnterGameResponsePacket responsePacket = new S2C_EnterGameResponsePacket() {
                PlayerID = playerID,
                Players = players
            };
            session.SendAsync(responsePacket);

            S2C_EnterGameBroadcastPacket broadcastPacket = new S2C_EnterGameBroadcastPacket() {
                PlayerID = playerID,
                Position = unit.transform.position
            };
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(broadcastPacket, (sessionID, session) => sessionID != playerID);

            return new ValueTask();
        }
    }
}