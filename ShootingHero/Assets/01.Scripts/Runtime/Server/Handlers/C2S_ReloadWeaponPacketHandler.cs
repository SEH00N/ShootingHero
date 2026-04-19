using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_ReloadWeaponPacket))]
    public class C2S_ReloadWeaponPacketHandler : IPacketHandler<C2S_ReloadWeaponPacket>
    {
        private readonly GameManager gameManager = null;
        private readonly GameServer gameServer = null;
        private readonly Server server = null;

        public C2S_ReloadWeaponPacketHandler(GameManager gameManager, GameServer gameServer, Server server)
        {
            this.gameManager = gameManager;
            this.gameServer = gameServer;
            this.server = server;
        }

        ValueTask IPacketHandler<C2S_ReloadWeaponPacket>.HandlePacket(Session session, C2S_ReloadWeaponPacket packet)
        {
            string playerID = gameServer.GetPlayerID(session);
            if(string.IsNullOrEmpty(playerID) == true)
                return new ValueTask();
            
            Unit player = gameManager.GetPlayer(playerID);
            if(player == null)
                return new ValueTask();

            player.UnitWeaponComponent.ReloadWeapon();

            S2C_ReloadWeaponBroadcastPacket broadcastPacket = new S2C_ReloadWeaponBroadcastPacket() {
                PlayerID = playerID,
            };
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(broadcastPacket);

            return new ValueTask();
        }
    }
}