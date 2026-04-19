namespace ShootingHero.Shared
{
    [System.Serializable]
    public class WeaponTableRow : DataTableRow
    {
        public WeaponBase weaponPrefab = null;
    }

    [System.Serializable]
    public class WeaponTable : DataTable<WeaponTableRow>
    {
        
    }
}