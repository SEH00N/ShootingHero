using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected int weaponID = 0;
        protected Unit owner = null;

        public int WeaponID => weaponID;
        public abstract bool IsReloading { get; }

        protected virtual void OnInitialize(string weaponStatus) { }
        public void Initialize(int weaponID, string weaponStatus)
        {
            this.weaponID = weaponID;
            OnInitialize(weaponStatus);
        }

        public void SetOwner(Unit owner)
        {
            this.owner = owner;
        }

        protected abstract void OnReload();
        public void Reload()
        {
            OnReload();
        }

        public abstract bool GetIsFireEnable();
        protected abstract void OnFire(Vector2 position);
        public void Fire(Vector2 position)
        {
            if(GetIsFireEnable() == false)
                return;

            OnFire(position);
        }

        public abstract string GetStatus();
    }
}
