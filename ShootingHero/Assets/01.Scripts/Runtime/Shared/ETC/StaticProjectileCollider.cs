using UnityEngine;

namespace ShootingHero.Shared
{
    public class StaticProjectileCollider : ProjectileCollider
    {
        [SerializeField]
        private int height = 0;
        
        public override bool GetIsCollidable(Projectile projectile)
        {
            return projectile.Height == height;
        }
    }
}