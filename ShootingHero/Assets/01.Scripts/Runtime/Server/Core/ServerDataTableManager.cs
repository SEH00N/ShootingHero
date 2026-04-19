using UnityEngine;

namespace ShootingHero.Servers
{
    [CreateAssetMenu(menuName = "ShootingHero/ServerDataTableManager")]
    public class ServerDataTableManager : ScriptableObject
    {
        public SpawnPositionTable unitSpawnPositionTable = null;
        public SpawnPositionTable itemSpawnPositionTable = null;

        private void OnEnable()
        {
            unitSpawnPositionTable.Initialize();
            itemSpawnPositionTable.Initialize();
        }
    }
}