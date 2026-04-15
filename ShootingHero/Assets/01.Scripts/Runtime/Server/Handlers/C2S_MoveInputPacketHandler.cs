using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_MoveInputPacket))]
    public class C2S_MoveInputPacketHandler : IPacketHandler<C2S_MoveInputPacket>
    {
        private readonly GameServer gameServer = null;
        private readonly Server server = null;

        public C2S_MoveInputPacketHandler(GameServer gameServer, Server server)
        {
            this.gameServer = gameServer;
            this.server = server;
        }

        ValueTask IPacketHandler<C2S_MoveInputPacket>.HandlePacket(Session session, C2S_MoveInputPacket packet)
        {
            string playerID = gameServer.GetPlayerID(session);
            if(string.IsNullOrEmpty(playerID) == true)
                return new ValueTask();
            
            Unit player = gameServer.GetPlayer(playerID);
            if(player == null)
                return new ValueTask();

            player.UnitMovementComponent.SetMovementInput(packet.MoveInput);

            S2C_MoveInputBroadcastPacket broadcastPacket = new S2C_MoveInputBroadcastPacket() {
                PlayerID = playerID,
                Position = player.transform.position,
                MoveInput = packet.MoveInput
            };
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(broadcastPacket);

            return new ValueTask();
        }
    }
}