namespace ShootingHero.Shared
{
    [System.Serializable]
    public class WeaponTableRow : DataTableRow
    {
        public int weaponInfoID = 0;
        public WeaponBase weaponPrefab = null;
    }

    [System.Serializable]
    public class WeaponTable : DataTable<WeaponTableRow>
    {
        
    }
}