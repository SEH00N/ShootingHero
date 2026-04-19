namespace ShootingHero.Shared
{
    [System.Serializable]
    public class ProjectileWeaponInfoTableRow : DataTableRow
    {
        public int magazineCapacity = 0;
        public float reloadTime = 0f;
        public float fireInterval = 0f;
        public int projectileDamage = 0;
        public float projectileSpeed = 0f;
        public Projectile projectilePrefab = null;
    }

    [System.Serializable]
    public class ProjectileWeaponInfoTable : DataTable<ProjectileWeaponInfoTableRow>
    {
        
    }
}