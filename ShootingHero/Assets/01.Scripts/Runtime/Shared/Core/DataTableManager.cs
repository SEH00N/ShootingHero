using UnityEngine;

namespace ShootingHero.Shared
{
    [CreateAssetMenu(menuName = "ShootingHero/DataTableManager")]
    public class DataTableManager : ScriptableObject
    {
        public DataTable<ItemTableRow> itemTable = null;
    }
}