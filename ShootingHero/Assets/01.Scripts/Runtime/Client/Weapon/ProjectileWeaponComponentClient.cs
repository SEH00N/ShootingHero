using Cysharp.Threading.Tasks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class ProjectileWeaponComponentClient : DedicatedMonoBehaviour
    {
        [SerializeField]
        private ProjectileWeapon projectileWeapon = null;

        [SerializeField]
        private Transform firePosition = null;

        [SerializeField]
        private ParticleSystem fireParticlePrefab = null;

        protected override EPlayMode DedicatedType => EPlayMode.Client;

        protected override void OnAwake()
        {
            base.OnAwake();
            projectileWeapon.OnFireEvent += HandleFire;
        }

        private async void HandleFire()
        {
            ParticleSystem fireParticle = Instantiate(fireParticlePrefab, firePosition.position + (firePosition.right * 0.25f), firePosition.rotation);
            fireParticle.Play();

            await UniTask.Delay(1000);
            Destroy(fireParticle.gameObject);
        }
    }
}