using UnityEngine;

namespace ShootingHero.Shared
{
    [System.Serializable]
    public class ItemTableRow : DataTableRow
    {
        public string itemName = string.Empty;
        public Sprite itemSprite = null;
        public ItemBase itemPrefab = null;
    }

    [System.Serializable]
    public class ItemTable : DataTable<ItemTableRow>
    {
        
    }
}