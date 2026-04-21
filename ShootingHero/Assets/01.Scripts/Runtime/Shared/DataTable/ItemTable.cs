using UnityEngine;

namespace ShootingHero.Shared
{
    [System.Serializable]
    public class ItemTableRow : DataTableRow
    {
        public string itemName = string.Empty;
        public ItemBase itemPrefab = null;
    }

    [System.Serializable]
    public class ItemTable : DataTable<ItemTableRow>
    {
        
    }
}