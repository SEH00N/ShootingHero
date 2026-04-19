using Cysharp.Threading.Tasks;
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
            unit.UnitHealthComponent.OnDeadEvent += HandleDead;
        }

        private void HandleDamaged(int damage)
        {
            S2C_UnitDamagedPacket unitDamagedPacket = new S2C_UnitDamagedPacket() {
                PlayerID = unit.PlayerID,
                Damage = damage
            };

            ServerInstance.GameServer.Send(unitDamagedPacket);
        }

        private async void HandleDead()
        {
            await UniTask.Delay(1000);

            gameObject.SetActive(false);
            S2C_UnitDeadPacket unitDeadPacket = new S2C_UnitDeadPacket() {
                PlayerID = unit.PlayerID,
            };

            ServerInstance.GameServer.Send(unitDeadPacket);
            HandleRespawn();
        }

        private async void HandleRespawn()
        {
            float respawnTime = GameInstance.DataTableManager.gameConfigTable.GetUnitRespawnTime();
            await UniTask.Delay((int)(respawnTime * 1000));
            
            SpawnPositionTableRow tableRow = ServerInstance.ServerDataTableManager.unitSpawnPositionTable.PickRandom();
            unit.transform.position = tableRow.position;
            unit.Respawn(tableRow.height);

            S2C_UnitRespawnPacket unitRespawnPacket = new S2C_UnitRespawnPacket() {
                PlayerID = unit.PlayerID,
                Position = unit.transform.position,
                Height = unit.GetHeight()
            };

            ServerInstance.GameServer.Send(unitRespawnPacket);
        }
    }
}