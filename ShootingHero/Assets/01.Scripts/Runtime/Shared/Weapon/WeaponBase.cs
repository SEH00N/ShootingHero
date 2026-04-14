using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected Unit owner = null;

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
