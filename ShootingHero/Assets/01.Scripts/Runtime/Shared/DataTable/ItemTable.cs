using System.Collections.Generic;
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
        private List<ItemTableRow> tableRowAsList = null; 

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            tableRowAsList = new List<ItemTableRow>();
            foreach(ItemTableRow tableRow in this)
                tableRowAsList.Add(tableRow);
        }

        public ItemTableRow PickRandom()
        {
            if(tableRowAsList.Count <= 0)
                return null;

            int index = Random.Range(0, tableRowAsList.Count);
            return tableRowAsList[index];
        }
    }
}