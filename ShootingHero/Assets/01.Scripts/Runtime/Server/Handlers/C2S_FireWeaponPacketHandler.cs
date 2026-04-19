using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_FireWeaponPacket))]
    public class C2S_FireWeaponPacketHandler : IPacketHandler<C2S_FireWeaponPacket>
    {
        private readonly GameManager gameManager = null;
        private readonly GameServer gameServer = null;

        public C2S_FireWeaponPacketHandler(GameManager gameManager, GameServer gameServer)
        {
            this.gameManager = gameManager;
            this.gameServer = gameServer;
        }

        ValueTask IPacketHandler<C2S_FireWeaponPacket>.HandlePacket(Session session, C2S_FireWeaponPacket packet)
        {
            string playerID = gameServer.GetPlayerID(session);
            if(string.IsNullOrEmpty(playerID) == true)
                return new ValueTask();
            
            Unit player = gameManager.GetPlayer(playerID);
            if(player == null)
                return new ValueTask();

            player.UnitWeaponComponent.FireWeapon(packet.Direction);

            S2C_FireWeaponBroadcastPacket broadcastPacket = new S2C_FireWeaponBroadcastPacket() {
                PlayerID = playerID,
                Position = player.transform.position,
                Direction = packet.Direction
            };
            gameServer.Send(broadcastPacket);

            return new ValueTask();
        }
    }
}