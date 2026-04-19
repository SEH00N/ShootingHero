using System.Collections.Generic;
using UnityEngine;

namespace ShootingHero.Shared
{
    [System.Serializable]
    public class GameConfigTableRow : DataTableRow
    {
        public string key = string.Empty;
        public float numberValue = 0f;
        public Object objectValue = null;
    }
    
    [System.Serializable]
    public class GameConfigTable : DataTable<GameConfigTableRow>
    {
        private Dictionary<string, GameConfigTableRow> tableRowByKey = null;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            tableRowByKey = new Dictionary<string, GameConfigTableRow>();
            foreach(GameConfigTableRow tableRow in this)
                tableRowByKey[tableRow.key] = tableRow;
        }

        private GameConfigTableRow GetRow(string key)
        {
            tableRowByKey.TryGetValue(key, out GameConfigTableRow tableRow);
            return tableRow;
        }

        public Unit GetUnitPrefab()
        {
            return GetRow("UnitPrefab").objectValue as Unit;
        }

        public float GetUnitInteractDistance()
        {
            return GetRow("UnitInteractDistance").numberValue;
        }

        public float GetUnitRespawnTime()
        {
            return GetRow("UnitRespawnTime").numberValue;
        }

        public int GetUnitMaxHP()
        {
            return (int)GetRow("UnitMaxHP").numberValue;
        }
    }
}