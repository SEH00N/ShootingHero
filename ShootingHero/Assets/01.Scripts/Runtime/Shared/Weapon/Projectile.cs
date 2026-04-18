using UnityEngine;

namespace ShootingHero.Shared
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D projectileRigidbody = null;

        [SerializeField]
        private int damage = 0;

        private Unit owner = null;
        private int height = 0;

        public int Damage => damage;

        public void Initialize(Unit owner, int height, Vector2 velocity)
        {
            this.owner = owner;
            this.height = height;
            projectileRigidbody.linearVelocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.TryGetComponent<IProjectileCollider>(out IProjectileCollider projectileCollider) == false)
                return;
            
            if(projectileCollider is Unit unit)
            {
                if(owner == unit)
                    return;
            }
            
            if(projectileCollider.GetHeight() != height)
                return;
            
            Vector2 hitPoint = collider.ClosestPoint(transform.position);
            projectileCollider.Collide(this, hitPoint);

            Destroy(gameObject);
        }
    }
}