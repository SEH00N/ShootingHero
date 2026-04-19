using System;
using System.Collections;
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

        protected override void OnReload()
        {
            if(tableRow == null)
                return;
            
            if(isReloading == true)
                return;

            isReloading = true;
            StartCoroutine(DelayCoroutine(tableRow.reloadTime, () => {
                currentAmmoCount = tableRow.magazineCapacity;
                isReloading = false; 
            }));
        }

        protected override void OnFire(Vector2 direction)
        {          
            lastFireTime = Time.time;
            currentAmmoCount -= 1;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Projectile projectile = Instantiate(tableRow.projectilePrefab, firePosition.position, Quaternion.Euler(0, 0, angle));
            projectile.Initialize(owner, tableRow.projectileDamage, owner.GetHeight(), direction * tableRow.projectileSpeed);
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

        private IEnumerator DelayCoroutine(float delayTime, Action callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke();
        }
    }
}