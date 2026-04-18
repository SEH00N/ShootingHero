using UnityEngine;

namespace ShootingHero.Shared
{
    public interface IProjectileCollider
    {
        public int GetHeight();
        public void Collide(Projectile projectile, Vector2 point);
    }
}