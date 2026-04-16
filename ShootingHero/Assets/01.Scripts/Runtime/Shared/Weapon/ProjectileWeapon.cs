using UnityEngine;

namespace ShootingHero.Shared
{
    public class ProjectileWeapon : WeaponBase
    {
        [SerializeField]
        private Transform firePosition = null;

        [SerializeField]
        private Projectile projectilePrefab = null;

        [SerializeField]
        private float projectileSpeed = 10f;

        protected override void OnFire(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Projectile projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.Euler(0, 0, angle));
            projectile.Initialize(direction * projectileSpeed);
        }
    }
}