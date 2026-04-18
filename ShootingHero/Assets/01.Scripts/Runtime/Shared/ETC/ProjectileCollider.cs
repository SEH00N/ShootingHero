using UnityEngine;

namespace ShootingHero.Shared
{
    public class ProjectileCollider : MonoBehaviour, IProjectileCollider
    {
        [SerializeField]
        private int height = 0;

        public int GetHeight()
        {
            return height;
        }

        public void Collide(Projectile projectile, Vector2 point)
        {
            
        }
    }
}