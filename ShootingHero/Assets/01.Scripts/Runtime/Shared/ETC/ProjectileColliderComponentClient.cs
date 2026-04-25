using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class ProjectileColliderComponentClient : DedicatedMonoBehaviour
    {
        [SerializeField]
        private ProjectileCollider projectileCollider = null;

        [SerializeField]
        private ParticleSystem hitParticlePrefab = null;

        protected override EPlayMode DedicatedType => EPlayMode.Client;

        protected override void OnAwake()
        {
            base.OnAwake();
            projectileCollider.OnCollideEvent += HandleCollide;
        }

        private async void HandleCollide(Projectile projectile, Vector2 point)
        {
            Vector3 direction = projectile.transform.eulerAngles;
            direction.z += 180f;

            ParticleSystem effect = Instantiate(hitParticlePrefab, point, Quaternion.Euler(direction));
            effect.Play();

            await UniTask.Delay(1000);
            Destroy(effect.gameObject);
        }
    }
}