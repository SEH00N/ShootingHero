using UnityEngine;

namespace ShootingHero.Shared
{
    public class WeaponItem : ItemBase
    {
        [SerializeField]
        private WeaponBase weaponPrefab = null;

        protected override void OnInteract(Unit unit)
        {
            WeaponBase weaponBase = Instantiate(weaponPrefab, unit.transform.position, Quaternion.identity);
            weaponBase.SetOwner(unit);
            unit.UnitWeaponComponent.SetWeapon(weaponBase);

            DestroyItem();
        }
    }
}