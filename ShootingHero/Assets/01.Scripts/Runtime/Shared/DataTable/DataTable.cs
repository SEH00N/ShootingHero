using System.Collections.Generic;
using UnityEngine;

namespace ShootingHero.Shared
{
    [System.Serializable]
    public class DataTable<TRow> where TRow : DataTableRow
    {
        [SerializeField]
        private List<TRow> tableRowList = null;
        private readonly Dictionary<int, TRow> tableRows = null;
        
        public DataTable()
        {
            tableRowList = new List<TRow>();
            tableRows = new Dictionary<int, TRow>();
        }

        public void Initialize()
        {
            foreach(TRow tableRow in tableRowList)
                tableRows[tableRow.id] = tableRow;
        }

        public TRow GetRow(int id)
        {
            tableRows.TryGetValue(id, out TRow tableRow);
            return tableRow;
        }
    }
}