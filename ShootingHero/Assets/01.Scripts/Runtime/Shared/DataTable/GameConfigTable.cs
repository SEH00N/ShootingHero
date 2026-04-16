using System.Collections.Generic;
using UnityEngine;

namespace ShootingHero.Shared
{
    [System.Serializable]
    public class GameConfigTableRow : DataTableRow
    {
        public string key = string.Empty;
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

        public GameConfigTableRow GetRow(string key)
        {
            tableRowByKey.TryGetValue(key, out GameConfigTableRow tableRow);
            return tableRow;
        }
    }
}