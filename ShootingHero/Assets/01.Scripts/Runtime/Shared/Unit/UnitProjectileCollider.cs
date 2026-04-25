using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnitProjectileCollider : ProjectileCollider
    {
        [SerializeField]
        private Unit unit = null;

        public override bool GetIsCollidable(Projectile projectile)
        {
            if(projectile.Owner == unit)
                return false;
            
            return projectile.Height == unit.GetHeight();
        }
    }
}