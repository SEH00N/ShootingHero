using System.Collections.Generic;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    [System.Serializable]
    public class SpawnPositionTableRow : DataTableRow
    {
        public Vector2 position = Vector2.zero;
        public int height = 0;
    }

    [System.Serializable]
    public class SpawnPositionTable : DataTable<SpawnPositionTableRow>
    {
        private List<SpawnPositionTableRow> tableRowAsList = null; 

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            tableRowAsList = new List<SpawnPositionTableRow>();
            foreach(SpawnPositionTableRow tableRow in this)
                tableRowAsList.Add(tableRow);
        }

        public SpawnPositionTableRow PickRandom()
        {
            if(tableRowAsList.Count <= 0)
                return null;

            int index = Random.Range(0, tableRowAsList.Count);
            return tableRowAsList[index];
        }
    }
}