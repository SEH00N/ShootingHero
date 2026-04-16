using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_InteractItemPacket))]
    public class C2S_InteractItemPacketHandler : IPacketHandler<C2S_InteractItemPacket>
    {
        private readonly GameServer gameServer = null;
        private readonly Server server = null;

        public C2S_InteractItemPacketHandler(GameServer gameServer, Server server)
        {
            this.gameServer = gameServer;
            this.server = server;
        }

        ValueTask IPacketHandler<C2S_InteractItemPacket>.HandlePacket(Session session, C2S_InteractItemPacket packet)
        {
            string playerID = gameServer.GetPlayerID(session);
            if(string.IsNullOrEmpty(playerID) == true)
                return new ValueTask();
            
            Unit player = gameServer.GetPlayer(playerID);
            if(player == null)
                return new ValueTask();

            ItemBase item = gameServer.GetItem(packet.ItemUUID);
            if(item == null)
                return new ValueTask();

            Vector2 distance = item.transform.position - player.transform.position;
            if(distance.sqrMagnitude > GameDefine.UNIT_INTERACT_DISTANCE * GameDefine.UNIT_INTERACT_DISTANCE)
                return new ValueTask();

            item.Interact(player);

            S2C_InteractItemBroadcastPacket broadcastPacket = new S2C_InteractItemBroadcastPacket() {
                PlayerID = playerID,
                ItemUUID = packet.ItemUUID
            };
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(broadcastPacket);

            return new ValueTask();
        }
    }
}