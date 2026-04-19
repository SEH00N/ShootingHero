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
        private readonly DataTableManager dataTableManager = null;

        public C2S_InteractItemPacketHandler(GameServer gameServer, Server server, DataTableManager dataTableManager)
        {
            this.gameServer = gameServer;
            this.server = server;
            this.dataTableManager = dataTableManager;
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
            float interactDistance = dataTableManager.gameConfigTable.GetUnitInteractDistance();
            if(distance.sqrMagnitude > interactDistance * interactDistance)
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