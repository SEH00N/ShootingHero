using UnityEngine;

namespace ShootingHero.Shared
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D projectileRigidbody = null;

        private Unit owner = null;
        private int damage = 0;
        private int height = 0;

        public Unit Owner => owner;
        public int Damage => damage;
        public int Height => height;

        public void Initialize(Unit owner, int damage, int height, Vector2 velocity)
        {
            this.owner = owner;
            this.damage = damage;
            this.height = height;
            projectileRigidbody.linearVelocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.TryGetComponent<ProjectileCollider>(out ProjectileCollider projectileCollider) == false)
                return;
            
            if(projectileCollider.GetIsCollidable(this) == false)
                return;
            
            Vector2 hitPoint = collider.ClosestPoint(transform.position);
            projectileCollider.Collide(this, hitPoint);

            Destroy(gameObject);
        }
    }
}