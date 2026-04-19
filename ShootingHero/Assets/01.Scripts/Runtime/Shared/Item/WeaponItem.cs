using UnityEngine;

namespace ShootingHero.Shared
{
    public class WeaponItem : ItemBase
    {
        [SerializeField]
        private int weaponID = 0;

        protected override void OnInteract(Unit unit)
        {

            unit.UnitWeaponComponent.SetWeapon(weaponID);

            DestroyItem();
        }
    }
}