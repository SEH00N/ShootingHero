using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public class UnitHealthComponentServer : DedicatedMonoBehaviour
    {
        [SerializeField]
        private Unit unit = null;

        protected override EPlayMode DedicatedType => EPlayMode.Server;

        protected override void OnAwake()
        {
            base.OnAwake();
            unit.UnitHealthComponent.OnDamagedEvent += HandleDamaged;
        }

        private void HandleDamaged(int damage)
        {
            S2C_UnitDamagedPacket unitDamagedPacket = new S2C_UnitDamagedPacket() {
                PlayerID = unit.PlayerID,
                Damage = damage
            };

            Server server = GameServer.Instance.Server;
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(unitDamagedPacket);
        }
    }
}