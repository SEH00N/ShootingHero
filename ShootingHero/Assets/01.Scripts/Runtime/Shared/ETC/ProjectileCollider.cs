using System;
using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class ProjectileCollider : MonoBehaviour
    {
        public event Action<Projectile, Vector2> OnCollideEvent = null;

        public abstract bool GetIsCollidable(Projectile projectile);

        public void Collide(Projectile projectile, Vector2 point)
        {
            OnCollideEvent?.Invoke(projectile, point);
        }
    }
}