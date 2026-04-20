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

        private void HandleDamaged(Unit attacker, int damage)
        {
            string attackerID = null;
            if(attacker != null)
                attackerID = attacker.PlayerID;

            S2C_UnitDamagedPacket unitDamagedPacket = new S2C_UnitDamagedPacket() {
                PlayerID = unit.PlayerID,
                AttackerID = attackerID,
                Damage = damage
            };

            ServerInstance.GameServer.Send(unitDamagedPacket);
        }

        private async void HandleDead(Unit attacker)
        {
            await UniTask.Delay(500);

            string attackerID = null;
            int attackerScore = 0;
            if(attacker != null)
            {
                LeaderBoard leaderBoard = GameManager.Instance.LeaderBoard;
                int currentScore = leaderBoard.Get(attacker.PlayerID);
                int addScore = GameInstance.DataTableManager.gameConfigTable.GetKillScore();
                leaderBoard.Set(attacker.PlayerID, currentScore + addScore);

                attackerID = attacker.PlayerID;
                attackerScore = currentScore + addScore;
            }

            gameObject.SetActive(false);
            S2C_UnitDeadPacket unitDeadPacket = new S2C_UnitDeadPacket() {
                PlayerID = unit.PlayerID,
                AttackerID = attackerID,
                AttackerScore = attackerScore
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