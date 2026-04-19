using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected int weaponID = 0;
        protected Unit owner = null;

        public int WeaponID => weaponID;

        public void Initialize(int weaponID)
        {
            this.weaponID = weaponID;
        }

        public void SetOwner(Unit owner)
        {
            this.owner = owner;
        }

        protected abstract void OnFire(Vector2 direction);
        public void Fire(Vector2 direction)
        {
            OnFire(direction);
        }
    }
}
