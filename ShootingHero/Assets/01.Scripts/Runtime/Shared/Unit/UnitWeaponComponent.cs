using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnitWeaponComponent : MonoBehaviour
    {
        [SerializeField]
        private DataTableManager dataTableManager = null;

        [SerializeField]
        private Unit unit = null;

        [SerializeField]
        private Transform weaponContainer = null;

        private WeaponBase weapon = null;
        public WeaponBase Weapon => weapon;

        public void SetWeapon(int weaponID)
        {
            weapon = null;

            WeaponTableRow tableRow = dataTableManager.weaponTable.GetRow(weaponID);
            if(tableRow == null)
                return;

            weapon = Instantiate(tableRow.weaponPrefab, unit.transform.position, Quaternion.identity);
            weapon.Initialize(weaponID);
            weapon.SetOwner(unit);

            weapon.transform.SetParent(weaponContainer);
            weapon.transform.localPosition = Vector3.zero;
        }

        public void FireWeapon(Vector2 direction)
        {
            if(weapon == null)
                return;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weapon.Fire(direction);
        }
    }
}