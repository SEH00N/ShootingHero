using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public class UnitAnimationComponentServer : DedicatedMonoBehaviour
    {
        [SerializeField]
        private Animator animatorComponent = null;

        protected override EPlayMode DedicatedType => EPlayMode.Server;

        protected override void OnAwake()
        {
            base.OnAwake();
            Destroy(animatorComponent);
        }
    }
}