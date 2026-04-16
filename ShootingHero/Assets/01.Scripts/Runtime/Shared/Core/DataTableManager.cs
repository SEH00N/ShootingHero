using UnityEngine;

namespace ShootingHero.Shared
{
    [CreateAssetMenu(menuName = "ShootingHero/DataTableManager")]
    public class DataTableManager : ScriptableObject
    {
        public GameConfigTable gameConfigTable = null;
        public ItemTable itemTable = null;

        private void OnEnable()
        {
            gameConfigTable.Initialize();
            itemTable.Initialize();
        }
    }
}