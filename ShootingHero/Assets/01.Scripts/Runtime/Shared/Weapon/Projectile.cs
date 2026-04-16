using UnityEngine;

namespace ShootingHero.Shared
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D projectileRigidbody = null;

        public void Initialize(Vector2 velocity)
        {
            projectileRigidbody.linearVelocity = velocity;
        }
    }
}