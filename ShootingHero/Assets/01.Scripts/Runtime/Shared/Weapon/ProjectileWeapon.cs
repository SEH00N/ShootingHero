using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class ProjectileWeapon : WeaponBase
    {
        private class WeaponStatus
        {
            public int CurrentAmmonCount { get; set; }

            public string Serialize()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }

            public static WeaponStatus Deserialize(string weaponStatus)
            {
                if(string.IsNullOrEmpty(weaponStatus) == true)
                    return new WeaponStatus();
                
                return Newtonsoft.Json.JsonConvert.DeserializeObject<WeaponStatus>(weaponStatus);
            }
        }

        [SerializeField]
        private Transform firePosition = null;

        private ProjectileWeaponInfoTableRow tableRow = null;
        private int currentAmmoCount = 0;
        private float lastFireTime = 0f;
        private bool isReloading = false;

        public override bool IsReloading => isReloading;

        protected override void OnInitialize(string weaponStatus)
        {
            base.OnInitialize(weaponStatus);

            WeaponStatus weaponStatusInfo = WeaponStatus.Deserialize(weaponStatus);
            currentAmmoCount = weaponStatusInfo.CurrentAmmonCount;
            isReloading = false;

            DataTableManager dataTableManager = GameInstance.DataTableManager;
            WeaponTableRow weaponTableRow = dataTableManager.weaponTable.GetRow(weaponID);
            if(weaponTableRow == null)
                return;

            tableRow = dataTableManager.projectileWeaponInfoTable.GetRow(weaponTableRow.weaponInfoID);
        }

        protected override async void OnReload()
        {
            if(tableRow == null)
                return;
            
            if(isReloading == true)
                return;

            isReloading = true;
            await UniTask.Delay((int)(tableRow.reloadTime * 1000));
            isReloading = false; 

            currentAmmoCount = tableRow.magazineCapacity;
        }

        protected override void OnFire(Vector2 firePosition)
        {
            Vector2 weaponDirection = (firePosition - (Vector2)transform.position).normalized;
            Vector2 weaponDirectionAsRight = weaponDirection;
            weaponDirectionAsRight.x = Mathf.Abs(weaponDirectionAsRight.x);
            float weaponAngle = Mathf.Atan2(weaponDirectionAsRight.y, weaponDirectionAsRight.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, weaponAngle);

            lastFireTime = Time.time;
            currentAmmoCount -= 1;

            Vector2 projectileDirection = (firePosition - (Vector2)this.firePosition.position).normalized;
            float projectileAngle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
            Projectile projectile = Instantiate(tableRow.projectilePrefab, this.firePosition.position, Quaternion.Euler(0, 0, projectileAngle));
            projectile.Initialize(owner, tableRow.projectileDamage, owner.GetHeight(), projectileDirection * tableRow.projectileSpeed);
        }

        public override bool GetIsFireEnable()
        {
            if(tableRow == null)
                return false;

            if(isReloading == true)
                return false;
            
            if(Time.time - lastFireTime < tableRow.fireInterval)
                return false;
            
            if(currentAmmoCount <= 0)
                return false;
            
            return true;
        }

        public override string GetStatus()
        {
            WeaponStatus weaponStatus = new WeaponStatus() {
                CurrentAmmonCount = isReloading == true ? tableRow.magazineCapacity : currentAmmoCount
            };
            return weaponStatus.Serialize();
        }
    }
}